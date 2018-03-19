using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BerlinClock
{
    public class TimeConverter : ITimeConverter
    {
        public string convertTime(string aTime)
        {            
            if (!IsValidTime(aTime))
            {
                throw new Exception("Time is invalid");
            }            

            int[] timeParts = aTime.Split(':').Select(x => int.Parse(x)).ToArray();

            int hours = timeParts[0];
            int minutes = timeParts[1];
            int seconds = timeParts[2];

            var Row1Int = seconds % 2;  // active when number of seconds is even
            var Row2Int = hours / 5;  // number of active clusters in row #2
            var Row3Int = hours % 5; // number of active clusters in row #3
            var Row4Int = minutes / 5; // number of active clusters in row #4
            var Row5Int = minutes % 5;  // number of active clusters in row #5

            
            var Row1Str = Row1Int == 0 ? "Y" : "O"; // Y if number of seconds is even
            var Row2Str = (new string('R', Row2Int)).PadRight(4, 'O'); // max length = 4; R means the cluster is active (red light); 0 stands for inactive
            var Row3Str = (new string('R', Row3Int)).PadRight(4, 'O');         
            var Row4Str = string.Concat(Enumerable.Repeat("YYR", 4)).Substring(0, Row4Int).PadRight(11, 'O'); // the sequence is Yellow then Yellow then Red
            var Row5Str = (new string('Y', Row5Int)).PadRight(4, 'O'); // Y means the cluster is active (yellow light)

            return 
                Row1Str + Environment.NewLine + 
                Row2Str + Environment.NewLine + 
                Row3Str + Environment.NewLine + 
                Row4Str + Environment.NewLine + 
                Row5Str;
        }

        bool IsValidTime(string aTime)
        {
            TimeSpan a;
            TimeSpan.TryParse(aTime, out a);
            bool isValidTime = a.TotalSeconds > 0 || aTime == "00:00:00";
            return isValidTime;            
        }
    }
}
