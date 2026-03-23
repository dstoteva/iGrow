namespace iGrow.GCommon.Exceptions
{
    public class EntityCreatePersistFailureException : Exception
    {
        public EntityCreatePersistFailureException()
        {
        }
        public EntityCreatePersistFailureException(string message)
            : base(message)
        {
        }
        public EntityCreatePersistFailureException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
