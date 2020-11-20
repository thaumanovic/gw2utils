using System;
using NUnit.Framework;
using Shouldly;

namespace Gw2Utils.Tests
{
    public class Gw2TimeUtilsTests
    {
        /// <summary>
        /// Valid times observed in-game.
        /// </summary>
        /// <remarks>
        /// These can be obtained by entering "/time" into the game chat as soon as the in-game server time clock ticks
        /// over to the next minute.
        ///
        /// If not done as such, there can be slight discrepancies as the in-game server time clock unfortunately
        /// does not show the current seconds and relying on the current seconds of another time source may be inaccurate
        /// due to slight differences in the current seconds.
        /// </remarks>
        internal static readonly object[] ValidTimeConversions = new[] {
            new object[] { DateTime.Parse("08:50"), "10:00" },
            new object[] { DateTime.Parse("08:53"), "10:36" }
        };

        internal static readonly object[] ValidTimesToTimeOfDayConversions = new[] {
            new  object[] { TimeSpan.Parse("00:00"), TimeOfDay.Night },
            new  object[] { TimeSpan.Parse("04:59"), TimeOfDay.Night },
            new  object[] { TimeSpan.Parse("05:00"), TimeOfDay.Dawn },
            new  object[] { TimeSpan.Parse("05:59"), TimeOfDay.Dawn },
            new  object[] { TimeSpan.Parse("06:00"), TimeOfDay.Day },
            new  object[] { TimeSpan.Parse("19:59"), TimeOfDay.Day },
            new  object[] { TimeSpan.Parse("20:00"), TimeOfDay.Dusk },
            new  object[] { TimeSpan.Parse("20:59"), TimeOfDay.Dusk },
            new  object[] { TimeSpan.Parse("21:00"), TimeOfDay.Night },
            new  object[] { TimeSpan.Parse("23:59"), TimeOfDay.Night }
        };

        [TestCaseSource(nameof(ValidTimeConversions))]
        public void TestConvertsToTyrianTimeCorrectly(DateTime realTime, string expectedTyrianTime)
        {
            var currentTyrianCycleTime = Gw2TimeUtils.ConvertToTyrianTime(realTime);
            
            currentTyrianCycleTime.ToString("hh\\:mm")
                .ShouldBe(expectedTyrianTime);
        }

        [TestCaseSource(nameof(ValidTimesToTimeOfDayConversions))]
        public void TestConvertsFromTimeSpanToTimeOfDayCorrectly(TimeSpan time, TimeOfDay expectedTimeOfDay)
        {
            var actualTimeOfDay = Gw2TimeUtils.GetTyrianTimeOfDay(time);
            
            actualTimeOfDay.ShouldBe(expectedTimeOfDay);
        }
    }
}
