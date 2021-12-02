using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace IMSProject
{
    public class Time
    {
        public static String add(String time, int minutes)
        {
            DateTime calendar = DateTime.Parse(time);
            calendar = calendar.AddMinutes(minutes);

            return calendar.ToString();
        }

        public static bool compare(String time1, String time2)
        {
            DateTime date1 = DateTime.Parse(time1);
            DateTime date2 = DateTime.Parse(time2);
            //                                Monday  Monday Tuesday

            int flag = date1.CompareTo(date2);

            return !(flag <= 0);
        }
    }
}
