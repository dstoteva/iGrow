namespace iGrow.GCommon.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException() 
        {
        }
        public EntityNotFoundException(string message) 
            : base(message)
        {
        }
    }
}
