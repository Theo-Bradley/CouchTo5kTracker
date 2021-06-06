using System;
using System.Collections.Generic;
using System.Text;

namespace CouchTo5kTracker.Classes
{
    public class Run
    {
        public string runName; //must be public for serialization
        public Date runDate;
        public int runWeek;
        public int runRun;
        public Time runLength;


        public enum WeekEnum
        {
            W1,
            W2,
            W3,
            W4,
            W5
        }

        public enum RunEnum
        {
            R1,
            R2,
            R3
        }

        public Run(string name, Date date, WeekEnum week, RunEnum run, Classes.Time time)
        {
            if (name.Length > 0 && name.Length <= 25)
            runName = name;
            else
                name = "error";
            
            runDate = date;

            runWeek = (int)week + 1;
            runRun = (int)run + 1;

            runLength = time;
        }

        public Run()
        {
            //used for JSON deserialisation
        }

        public string getName()
        {
            return runName;
        }

        public Classes.Date getDate()
        {
            return runDate;
        }

        public int getWeek()
        {
            return runWeek;
        }

        public int getRun()
        {
            return runRun;
        }

        public Classes.Time getLengthTime()
        {
            return runLength;
        }

        public int getLengthSeconds()
        {
            return runLength.getTimeinSeconds();
        }
    }
}
