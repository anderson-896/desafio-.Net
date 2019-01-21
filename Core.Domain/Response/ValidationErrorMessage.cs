namespace Core.Domain.Response
{
    public struct ValidationErrorMessage
    {
        public string Message { get; private set; }
        public int? ErrorCode { get; private set; }

        public ValidationErrorMessage(string message, int? errorCode = null)
        {
            this.Message = message;
            this.ErrorCode = errorCode;
        }
    }
}
