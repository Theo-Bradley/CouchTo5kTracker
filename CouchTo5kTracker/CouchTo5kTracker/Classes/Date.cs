using System;
using System.Collections.Generic;
using System.Text;

namespace CouchTo5kTracker.Classes
{
    public class Date
    {
        public int dateDay;
        public int dateMonth;
        public Date(int day, int month)
        {
            if (day > 32)
                dateDay = 32;
            if (day < 0)
                dateDay = 0;
            else
                dateDay = day;

            if (month > 12)
                dateMonth = 12;
            if (month < 0)
                dateMonth = 0;
            else
                dateMonth = month;
        }
        
        public Date()
        {
            //used for JSON deserialisation
        }

        public int getDay()
        {
            return dateDay;
        }

        public int getMonth()
        {
            return dateMonth;
        }
    }
}
