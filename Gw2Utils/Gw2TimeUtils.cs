using System;
using System.Collections.Generic;
using System.Linq;

namespace Gw2Utils
{
    public enum TimeOfDay
    {
        Dawn,
        Day,
        Dusk,
        Night
    }
    
    public static class Gw2TimeUtils
    {
        /// <summary>
        /// The number of seconds in a day.
        /// </summary>
        private const int SecondsInADay = 86400;

        /// <summary>
        /// The number of game days that occur with a real-time day.
        /// </summary>
        private const int GameDaysInRealDay = 12;
        
        /// <summary>
        /// Used to convert the current time of day (in Tyria) expressed as a second to a game day stage.
        /// </summary>
        private static readonly Dictionary<Range, TimeOfDay> TyrianSecondsToDayStateConversions = new Dictionary<Range, TimeOfDay>() {
            [0..17999] = TimeOfDay.Night,
            [18000..21599] = TimeOfDay.Dawn,
            [21600..71999] = TimeOfDay.Day,
            [72000..75599] = TimeOfDay.Dusk,
            [75600..86399] = TimeOfDay.Night
        };
        
        /// <summary>
        /// Converts a real-world time to the equivalent Tyrian time.
        /// </summary>
        public static TimeSpan ConvertToTyrianTime(DateTime realTime, int gameDaysInRealDay = GameDaysInRealDay)
        {
            var currentDayTimespan = realTime - realTime.Date;
            var currentCycleSeconds = currentDayTimespan.TotalSeconds % (SecondsInADay / GameDaysInRealDay);
            
            return TimeSpan.FromSeconds(currentCycleSeconds * GameDaysInRealDay);
        }

        /// <summary>
        /// Converts the current server time to Tyrian time.
        /// </summary>
        public static TimeSpan GetCurrentTyrianTime()
        {
            return ConvertToTyrianTime(DateTime.UtcNow);
        }

        /// <summary>
        /// Returns the TimeOfDay for the specified Tyrian time.
        /// </summary>
        public static TimeOfDay GetTyrianTimeOfDay(TimeSpan tyrianTime)
        {
            if (tyrianTime.TotalSeconds >= SecondsInADay)
                throw new ArgumentException("Must be a TimeSpan of less than 1 day.", nameof(tyrianTime));

            foreach (var dayStateConversion in TyrianSecondsToDayStateConversions) {

                var currentStageCheck = dayStateConversion.Key;
                
                if (tyrianTime.TotalSeconds >= currentStageCheck.Start.Value && tyrianTime.TotalSeconds <= currentStageCheck.End.Value) {
                    return dayStateConversion.Value;
                }
            }

            throw new Exception("Could not convert TimeSpam to TimeOfDay");
        }

        /// <summary>
        /// Returns the TimeOfDay that represents the current time of day in Tyria.
        /// </summary>
        /// <returns></returns>
        public static TimeOfDay GetCurrentTyrianTimeOfDay()
        {
            return GetTyrianTimeOfDay(GetCurrentTyrianTime());
        }
    }
}
