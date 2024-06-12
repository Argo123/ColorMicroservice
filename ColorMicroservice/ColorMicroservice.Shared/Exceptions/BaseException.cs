using System.Net;

namespace ColorMicroservice.Shared.Exceptions;

public abstract class BaseException : Exception
{
    public virtual string Reason => GetType().Name;
    public virtual HttpStatusCode StatusCode => HttpStatusCode.InternalServerError;

    protected BaseException(string message): base(message)
    { }

    protected BaseException(string message, Exception innerException) : base(message, innerException) 
    { }
}
