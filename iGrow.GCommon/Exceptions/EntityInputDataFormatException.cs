namespace iGrow.GCommon.Exceptions
{
    public class EntityInputDataFormatException : Exception
    {
        public EntityInputDataFormatException()
        {
        }
        public EntityInputDataFormatException(string message) 
            : base(message)
        {
        }
    }
}
