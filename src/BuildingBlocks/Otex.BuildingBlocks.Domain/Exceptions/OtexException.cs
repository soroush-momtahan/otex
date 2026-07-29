using Otex.BuildingBlocks.Domain.Errors;

namespace Otex.BuildingBlocks.Domain.Exceptions;

public sealed class OtexException(string requestName, Error? error = null, Exception? innerException = null)
    : Exception(error is not null ? error.Description : "خطای دامنه", innerException)
{
    public string RequestName { get; } = requestName;
    public Error? Error { get; } = error;
}
