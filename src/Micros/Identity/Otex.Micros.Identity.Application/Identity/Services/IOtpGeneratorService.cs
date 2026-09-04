namespace Otex.Micros.Identity.Application.Identity.Services;

public interface IOtpGeneratorService
{
    public string GenerateSecureOtp();
    public string GenerateMemorableOtp();
    public string GenerateMemorable6DigitOtp();
}