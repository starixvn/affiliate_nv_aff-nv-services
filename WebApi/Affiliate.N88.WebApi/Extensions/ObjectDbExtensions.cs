using System.Data;

namespace Affiliate.N88.WebApi.Extensions
{
    public static class ObjectDbExtensions
    {
        public static T GetVal<T>(this IDataReader reader, string rowName)
        {
            if (IsNullable(typeof(T)))
            {
                var underlyingType = Nullable.GetUnderlyingType(typeof(T));
                if (underlyingType == null)
                {
                    return default(T)!;
                }
                return reader[rowName] == DBNull.Value ? default(T)! : (T)Convert.ChangeType(reader[rowName], underlyingType);
            }
            return reader[rowName] == DBNull.Value ? default(T)! : (T)Convert.ChangeType(reader[rowName], typeof(T));
        }

        private static bool IsNullable(Type type) => Nullable.GetUnderlyingType(type) != null;
    }
}