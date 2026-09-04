using Otex.BuildingBlocks.Application.Caching;
using Otex.BuildingBlocks.Application.Cqrs;
using Otex.BuildingBlocks.Domain.Results;
using Otex.Micros.Identity.Application.Identity.Services;
using Otex.Micros.Identity.Domain.Identity.Errors;

namespace Otex.Micros.Identity.Application.Identity.Features.Commands.VerifyMobile;

public class VerifyMobileCommandHandler(
    ICacheService cacheService,
    IIdentityService identityService) : ICommandHandler<VerifyMobileCommand, VerifyMobileResult>
{
    public async Task<Result<VerifyMobileResult>> Handle(VerifyMobileCommand command, CancellationToken cancellationToken)
    {
        var code = await cacheService.GetAsync<string?>($"Otp:{command.Mobile}", cancellationToken);

        if (code is null || code != command.Code)
        {
            return Result.Failure<VerifyMobileResult>(IdentityErrors.VerifyCodeIsNotValid);
        }
        
        await cacheService.RemoveAsync($"Otp:{command.Mobile}", cancellationToken);

        string tempToken = $"Verified:{Guid.CreateVersion7()}";
        
        await cacheService.SetAsync(
            tempToken, 
            command.Mobile,
            new TimeSpan(0,5,0), cancellationToken);

        string? identityId = await identityService.FindByMobileAsync(command.Mobile);
        
        bool isNewUser = identityId == null;
        
        return new VerifyMobileResult(isNewUser, tempToken);
    }
}