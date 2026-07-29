using Otex.BuildingBlocks.Domain.Errors;
using Otex.Micros.Applicants.Domain.Cooperation.ValueObjects;

namespace Otex.Micros.Applicants.Domain.Cooperation.Errors;

public static class CooperationErrors
{
    public static Error FirstnameIsTooShort => new(
        "Cooperation.FirstnameIsTooShort",
        $"نام نمی تواند کمتر از {Fullname.MinLenght} حرف باشد.",
        ErrorType.Validation);
    
    public static Error FirstnameIsTooLong => new(
        "Cooperation.FirstnameIsTooLong",
        $"نام نمی تواند بیشتر از {Fullname.MaxLenght} حرف باشد.",
        ErrorType.Validation);
    
    public static Error LastnameIsTooShort => new(
        "Cooperation.LastnameIsTooShort",
        $"نام خانوادگی نمی تواند کمتر از {Fullname.MinLenght} حرف باشد.",
        ErrorType.Validation);
    
    public static Error LastnameIsTooLong => new(
        "Cooperation.LastnameIsTooLong",
        $"نام خانوادگی نمی تواند بیشتر از {Fullname.MaxLenght} حرف باشد.",
        ErrorType.Validation);
    public static Error MobileNotValid => new(
        "Cooperation.MobileNotValid",
        $"شماره موبایل معتبر نمی باشد.",
        ErrorType.Validation);
}