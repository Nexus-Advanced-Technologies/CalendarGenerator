using System;

namespace Nexusat.Utils.CalendarGenerator
{
    /// <summary>
    ///     Year matcher expressing a period condition on the year value and subsequent steps
    /// </summary>
    public class PeriodicYearMatcher : PeriodicNumberMatcher, IYearMatcher
    {
        public PeriodicYearMatcher(int left, int? right, int period) : base(left, right, period)
        {
        }

        public bool Match(DateTime date)
        {
            return Match(date.Year);
        }

        public bool IsOneYear => IsOneValue;

        public static bool TryParse(string value, out PeriodicYearMatcher periodicYearMatcher)
        {
            if (!TryParse(value, null, null, null, null, out var left, out var right, out var period))
            {
                periodicYearMatcher = null;
                return false;
            }

            periodicYearMatcher = new PeriodicYearMatcher(left, right, period);
            return true;
        }
    }
}