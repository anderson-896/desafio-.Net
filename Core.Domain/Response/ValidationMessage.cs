namespace Core.Domain.Response
{
    public struct ValidationMessage
    {
        public string Message { get; private set; }
        public int? StatusCode { get; private set; } 

        public ValidationMessage(string message, int? statusCode = null)
        {
            this.Message = message;
            this.StatusCode = statusCode;
        }
    }
}
