using Otex.BuildingBlocks.Domain.Results;
using Otex.Micros.Applicants.Domain.Cooperation.Errors;

namespace Otex.Micros.Applicants.Domain.Cooperation.ValueObjects;

public record ReservationDateTime
{
    internal const int MaxFutureDays = 10;
    internal const int StartWorkTime = 8;
    internal const int EndWorkTime = 13;
    public DateTime Value { get; init; }
    private ReservationDateTime(DateTime value) => Value = value;

    public static Result<ReservationDateTime> From(DateTime enteredDate)
    {
        var today = DateTime.Today;
        double daysDiff = Math.Abs((today - enteredDate.Date).TotalDays);
        TimeSpan time = enteredDate.TimeOfDay;
        TimeSpan startWork = new TimeSpan(StartWorkTime, 0, 0);   // ۸:۰۰ صبح
        TimeSpan endWork   = new TimeSpan(EndWorkTime, 0, 0);  // ۱:۰۰ بعدازظهر
        List<Result<ReservationDateTime>> results = [];
        if (enteredDate < today)
        {
            results.Add(Result.Failure<ReservationDateTime>(CooperationErrors.DateWasPast));
        }

        if (daysDiff > MaxFutureDays)
        {
            results.Add(Result.Failure<ReservationDateTime>(CooperationErrors.DateIsTooLong));
        }

        if (time < startWork && time > endWork)
        {
            results.Add(Result.Failure<ReservationDateTime>(CooperationErrors.EnteredDateIsOutOfWorkTime));
        }

        return results.Count != 0 ? Result.Combine(results) : new ReservationDateTime(enteredDate);
    }
}