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

        public int getMinutes()
        {
            return timeMinutes;
        }

        public int getSeconds()
        {
            return timeSeconds;
        }

        public int getTimeinSeconds()
        {
            int seconds = 60 * timeMinutes;
            seconds += timeSeconds;
            return seconds;
        }
    }
}
