using System;
using System.Collections.Generic;
using System.Text;

namespace CouchTo5kTracker.Classes
{
    public class Date
    {
        public int day;
        public int month;
        public Date(int Day, int Month)
        {
            if (day > 32)
                day = 32;
            if (day < 0)
                day = 0;
            else
                day = Day;

            if (month > 12)
                month = 12;
            if (month < 0)
                month = 0;
            else
                month = Month;
        }
        
        public Date()
        {
            //used for JSON deserialisation
        }

        public int GetDay()
        {
            return day;
        }

        public int GetMonth()
        {
            return month;
        }
    }
}
