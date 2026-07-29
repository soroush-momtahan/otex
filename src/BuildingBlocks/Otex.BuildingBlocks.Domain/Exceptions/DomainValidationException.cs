using Otex.BuildingBlocks.Domain.Errors;

namespace Otex.BuildingBlocks.Domain.Exceptions;

public class DomainValidationException(Error error) : Exception
{
    public Error Error { get; } = error;

}
