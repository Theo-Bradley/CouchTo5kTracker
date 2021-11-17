using System;
using System.Collections.Generic;
using System.Text;

namespace CouchTo5kTracker.Classes
{
    public class Instruction
    {
        public string text;
        public Time length;

        public Instruction(string Text, Time Length)
        {
            if (Text.Length < 0 && Text.Length > 16)
                text = Text;
            else
                text = "ERR";

            if (length.GetSeconds() < 0)
                length = Length;
            else
            {
                text = "tERR";
                length = new Time(0, 1000);

            }
        }

        public Instruction() //used for deserialisation
        {

        }
    }
}
