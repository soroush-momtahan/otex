using System.Linq.Expressions;
using Hydro;

namespace Otex.Micros.Identity.Razor.Features.SignOn.Views.OtpVerify;

public class OtpVerify : HydroView
{
    public Expression? OnChangeMobile { get; set; }
    public string? OtpCode { get; set; }
}