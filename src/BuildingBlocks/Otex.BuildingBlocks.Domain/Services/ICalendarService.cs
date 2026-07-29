namespace Otex.BuildingBlocks.Domain.Services;

public interface ICalendarService
{
    Task<bool> IsPublicHolidayAsync(DateOnly date);
}
