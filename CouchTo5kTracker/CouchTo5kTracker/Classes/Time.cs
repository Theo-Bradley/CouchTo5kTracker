using System;
using System.Collections.Generic;
using System.Text;

namespace CouchTo5kTracker.Classes
{
    public class Time
    {
        public int timeMinutes;
        public int timeSeconds;

        public Time(int minutes, int seconds)
        {
            if (minutes > 0)
                timeMinutes = minutes;
            if (seconds > 0)
                timeSeconds = seconds;
        }

        public Time()
        {
            //used for JSON deserialisation
        }

        public int GetMinutes()
        {
            return timeMinutes;
        }

        public int GetSeconds()
        {
            return timeSeconds;
        }

        public int GetTimeinSeconds()
        {
            int seconds = 60 * timeMinutes;
            seconds += timeSeconds;
            return seconds;
        }
    }
}
