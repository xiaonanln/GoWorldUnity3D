using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoWorld
{
    public class Logger
    {
        public static void Debug(string subject, string msg, params object[] args)
        {
            Console.WriteLine(String.Format("DEBUG - " + subject + " - " + msg, args));
        }

        public static void Info(string subject, string msg, params object[] args)
        {
            Console.WriteLine(String.Format("INFO - " + subject + " - " + msg, args));
        }

        public static void Warn(string subject, string msg, params object[] args)
        {
            Console.WriteLine(String.Format("WARN - " + subject + " - " + msg, args));
        }

        public static void Error(string subject, string msg, params object[] args)
        {
            Console.WriteLine(String.Format("ERROR - " + subject + " - " + msg, args));
        }

        public static void Fatal(string subject, string msg, params object[] args)
        {
            Console.WriteLine(String.Format("FATAL - " + subject + " - " + msg, args));
        }

    }
}
