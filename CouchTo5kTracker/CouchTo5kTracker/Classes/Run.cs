using System;
using System.Collections.Generic;
using System.Text;

namespace CouchTo5kTracker.Classes
{
    public class Run
    {
        public string name; //must be public for serialization
        public Date date;
        public int week;
        public int run;
        public Time length;
        public List<Instruction> instructions;

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

        public Run(string Name, Date Date, WeekEnum Week, RunEnum Run, Classes.Time Time, List<Instruction> Instructions)
        {
            if (Name.Length > 0 && Name.Length <= 25)
            name = Name;
            else
                Name = "error";
            
            date = Date;

            week = (int)Week + 1;
            run = (int)Run + 1;

            length = Time;
        }

        public Run()
        {
            //used for JSON deserialisation
        }

        public string GetName()
        {
            return name;
        }

        public Classes.Date GetDate()
        {
            return date;
        }

        public int GetWeek()
        {
            return week;
        }

        public int GetRun()
        {
            return run;
        }

        public Classes.Time GetLengthTime()
        {
            return length;
        }

        public int GetLengthSeconds()
        {
            return length.GetTimeinSeconds();
        }
    }
}
