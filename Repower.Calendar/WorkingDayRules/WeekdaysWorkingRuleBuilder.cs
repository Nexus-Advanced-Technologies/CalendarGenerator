﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repower.Calendar
{
    public class WeekdaysWorkingRuleBuilder
    {
        private Dictionary<DayOfWeek, List<TimePeriod>> settings = new Dictionary<DayOfWeek, List<TimePeriod>>();

        public void AddRule(DayOfWeek dayOfWeek, IEnumerable<TimePeriod> timePeriods)
        {
            if (timePeriods == null) throw new ArgumentNullException(nameof(timePeriods));
            if (settings.TryGetValue(dayOfWeek, out var curTimePeriods))
            {
                curTimePeriods.AddRange(timePeriods);
            }
            else
            {
                curTimePeriods = new List<TimePeriod>(timePeriods);
                settings.Add(dayOfWeek, curTimePeriods);
            }
        }

        public void AddRule(DayOfWeek dayOfWeek, params TimePeriod[] timePeriods) =>
            AddRule(dayOfWeek, timePeriods.ToList());

        public void AddRule(DayOfWeek dayOfWeek, short beginHour, short beginMinute, short endHour, short endMinute) =>
            AddRule(dayOfWeek, new TimePeriod(beginHour, beginMinute, endHour, endMinute));

        public void Clear() => settings.Clear();

        /// <summary>
        /// Retrieve the settings suitable to instantiate a new <see cref="WeekdaysWorkingRule"/>.
        /// </summary>
        /// <returns></returns>
        public WeekdaysWorkingRuleSettings GetSettings() =>
            new WeekdaysWorkingRuleSettings(settings.Select(s =>
            new WeekdaysWorkingRuleSettings.DaySetting(s.Key, s.Value)));
        /// <summary>
        /// Retrieve a new <see cref="WeekdaysWorkingRule"/> based of the settings provided so far.
        /// </summary>
        /// <returns></returns>
        public WeekdaysWorkingRule GetRule() => new WeekdaysWorkingRule(GetSettings());
        
    }

    public class WeekdaysNonWorkingRuleBuilder
    {
        private HashSet<DayOfWeek> settings = new HashSet<DayOfWeek>();

        public void AddRule(DayOfWeek dayOfWeek) => settings.Add(dayOfWeek);

        public void Clear() => settings.Clear();

        /// <summary>
        /// Retrieve the settings suitable to instantiate a new <see cref="WeekdaysWorkingRule"/>.
        /// </summary>
        /// <returns></returns>
        public WeekdaysNonWorkingRuleSettings GetSettings() =>
            new WeekdaysNonWorkingRuleSettings(settings.Select( s => new WeekdaysNonWorkingRuleSettings.DaySetting(s)));
        /// <summary>
        /// Retrieve a new <see cref="WeekdaysWorkingRule"/> based of the settings provided so far.
        /// </summary>
        /// <returns></returns>
        public WeekdaysNonWorkingRule GetRule() => new WeekdaysNonWorkingRule(GetSettings());

    }
}