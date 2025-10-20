using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Affiliate.N88.WebApi.Filters
{
    /// <summary>
    /// http://bitoftech.net/2014/12/15/secure-asp-net-web-api-using-api-key-authentication-hmac-authentication/
    /// </summary>
    public class HmacAuthorizationFilter : IAuthorizationFilter
    {
        private const long RequestMaxAgeInSeconds = 300;
        private const string TokenAuthenticationScheme = "amx";
        private readonly string sharedKey;
        private readonly IMemoryCache memoryCache;

        public HmacAuthorizationFilter(string sharedKey, IMemoryCache memoryCache)
        {
            this.sharedKey = sharedKey;
            this.memoryCache = memoryCache;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context), "Action Context cannot be null");
            }

            var allowAnonymousAttributes = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>();
            if (allowAnonymousAttributes.Any())
            {
                return;
            }

            var request = context.HttpContext.Request;

            if (request.Headers.Authorization.Count > 0)
            {
                var authHeader = request.Headers.Authorization.First();
                if (authHeader.StartsWith(TokenAuthenticationScheme + " "))
                {
                    var rawAuthzHeader = authHeader.Substring(TokenAuthenticationScheme.Length + 1);
                    var autherizationHeaderArray = GetAutherizationHeaderValues(rawAuthzHeader);

                    if (autherizationHeaderArray != null)
                    {
                        var machineIP = autherizationHeaderArray[0];
                        var incomingBase64Signature = autherizationHeaderArray[1];
                        var nonce = autherizationHeaderArray[2];
                        var requestTimeStamp = autherizationHeaderArray[3];

                        var isValid = IsValidRequest(request, machineIP, incomingBase64Signature, nonce, requestTimeStamp);

                        if (!isValid)
                        {
                            HandleUnauthorizedRequest(context);
                        }
                    }
                    else
                    {
                        HandleUnauthorizedRequest(context);
                    }
                }
                else
                {
                    HandleUnauthorizedRequest(context);
                }
            }
            else
            {
                HandleUnauthorizedRequest(context);
            }
        }

        private void HandleUnauthorizedRequest(AuthorizationFilterContext context)
        {
            context.Result = new UnauthorizedObjectResult("Request is not authenticated");
        }

        private string[] GetAutherizationHeaderValues(string rawAuthzHeader)
        {
            var credArray = rawAuthzHeader.Split(':');

            if (credArray.Length == 4)
            {
                return credArray;
            }
            else
            {
                return null;
            }
        }

        private bool IsValidRequest(HttpRequest req, string siteId, string incomingBase64Signature, string nonce, string requestTimeStamp)
        {
            string requestUri = HttpUtility.UrlEncode(req.Path + req.QueryString);
            string requestHttpMethod = req.Method;

            if (IsReplayRequest(nonce, requestTimeStamp))
            {
                return false;
            }

            string data = string.Format("{0}{1}{2}{3}{4}", siteId, requestHttpMethod, requestUri, requestTimeStamp, nonce);

            var secretKeyBytes = Convert.FromBase64String(sharedKey);

            byte[] signature = Encoding.UTF8.GetBytes(data);

            using (HMACSHA256 hmac = new HMACSHA256(secretKeyBytes))
            {
                byte[] signatureBytes = hmac.ComputeHash(signature);

                return incomingBase64Signature.Equals(Convert.ToBase64String(signatureBytes), StringComparison.Ordinal);
            }
        }

        private bool IsReplayRequest(string nonce, string requestTimeStamp)
        {
            if (memoryCache.TryGetValue(nonce, out _))
            {
                return true;
            }

            var epochStart = new DateTime(1970, 01, 01, 0, 0, 0, 0, DateTimeKind.Utc);
            var currentTimeSpan = DateTime.UtcNow - epochStart;

            var serverTotalSeconds = Convert.ToUInt64(currentTimeSpan.TotalSeconds);
            var requestTotalSeconds = Convert.ToUInt64(requestTimeStamp);

            if (((long)serverTotalSeconds - (long)requestTotalSeconds) > RequestMaxAgeInSeconds)
            {
                return true;
            }

            memoryCache.Set(nonce, requestTimeStamp, TimeSpan.FromSeconds(RequestMaxAgeInSeconds));

            return false;
        }
    }
}