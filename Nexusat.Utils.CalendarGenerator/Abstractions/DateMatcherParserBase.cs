using System;
using System.Collections.Generic;

namespace Nexusat.Utils.CalendarGenerator
{
    public abstract class DateMatcherParserBase<T> : IDateMatcherParseMulti<T> where T : IDateMatcher
    {
        public abstract bool TryParse(string value, out T dateMatcher);

        public virtual bool TryParseMulti(string values, string separator, out IEnumerable<T> dateMatchers)
        {
            if (string.IsNullOrWhiteSpace(values))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(values));
            if (string.IsNullOrEmpty(separator))
                throw new ArgumentException("Value cannot be null or empty.", nameof(separator));
            dateMatchers = default;
            var varDateMatchers = new List<T>();
            var separators = new string[] { separator };
            foreach (var value in values.Split(separators, StringSplitOptions.RemoveEmptyEntries))
            {
                if (!TryParse(value, out var dateMatcher)) return false;
                varDateMatchers.Add(dateMatcher);
            }

            dateMatchers = varDateMatchers;
            return true;
        }

        public bool TryParseMulti(string values, out IEnumerable<T> dateMatchers)
        {
            return TryParseMulti(values, ",", out dateMatchers);
        }
    }
}