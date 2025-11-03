namespace Affiliate.N88.Exceptions
{
    public class XmlElementNotFoundException : Exception
    {
        public XmlElementNotFoundException(string message) : base(message)
        {
        }

        public XmlElementNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public XmlElementNotFoundException()
        {
        }
    }
}
