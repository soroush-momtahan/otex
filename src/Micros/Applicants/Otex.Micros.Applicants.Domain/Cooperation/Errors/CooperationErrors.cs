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

    public static Error DateWasPast = new(
        "Cooperation.DateWasPast",
        "زمان تاریخ وارد شده گذشته است.",
        ErrorType.Validation);
    
    public static readonly Error DateIsTooLong = new(
        "Cooperation.DateIsMoreThan10Days",
        $"تاریخ وارد شده نمی تواند بیش از {ReservationDateTime.MaxFutureDays} روز آینده باشد.",
        ErrorType.Validation);

    public static readonly Error EnteredDateIsOutOfWorkTime = new(
        "Cooperation.TimeIsOutOfWorkTime",
        $"زمان معتبر بین {ReservationDateTime.StartWorkTime} صبح تا {ReservationDateTime.EndWorkTime - 12} بعدازظهر می باشد.",
        ErrorType.Validation);
    
    public static readonly Error ActivityIsOutOfRang = new(
        "Cooperation.ActivityIsOutOfRang",
        "کد نوع فعالیت خاج از حد مجاز است.",
        ErrorType.Validation);
}