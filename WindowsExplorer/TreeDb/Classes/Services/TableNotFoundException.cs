using System.Runtime.Serialization;

namespace TreeDb.Classes.Services
{
    [Serializable]
    internal class TableNotFoundException : Exception
    {
        const string TableNotFoundMessage = "The Queried table could not be found.";
        
        public TableNotFoundException() : base(TableNotFoundMessage)
        {

        }
        public TableNotFoundException(Exception e) : base(TableNotFoundMessage, e)
        {

        }

        public TableNotFoundException(string? query) : base($"{TableNotFoundMessage}\nQuery was: {query}")
        {
        }

        public TableNotFoundException(string? query, Exception? innerException) : base($"{TableNotFoundMessage}\nQuery was: {query}", innerException)
        {
        }

        protected TableNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}