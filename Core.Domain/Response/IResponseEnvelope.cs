namespace Core.Domain.Response
{
    public interface IResponseEnvelope
    {
        bool Success { get; }
        ValidationMessage Message { get;  }
    }

    public interface IResponseEnvelope<T> : IResponseEnvelope
    {
        T Content { get; set; }
    }
}
