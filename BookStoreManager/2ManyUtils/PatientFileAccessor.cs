using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TooManyUtils
{
    public static class PatientFileAccessor
    {
        /// <summary>
        /// ATTENTION: this WILL make the thread sleep a bit. You have been warned.
        /// </summary>
        /// <param name="targetPath"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public static String? ReadAllText(string targetPath, int timeout = 10)
        {
            bool success = false;
            String data = ";";

            if (!File.Exists(targetPath))
                return data;

            while (!success)
            {
                timeout--;

                try
                {
                    data = File.ReadAllText(targetPath);
                    success = true;
                }
                catch (IOException)
                {
                    if (timeout <= 0)
                        throw;

                    Thread.Sleep(5);
                }
            }

            return data;
        }

        /// <summary>
        /// ATTENTION: this WILL make the thread sleep a bit. You have been warned.
        /// </summary>
        /// <param name="targetPath"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public static string[] ReadAllLines(string targetPath, int timeout = 10)
        {
            bool success = false;
            string[] data = { };

            if (!File.Exists(targetPath))
                return data;

            while (!success)
            {
                timeout--;

                try
                {
                    data = File.ReadAllLines(targetPath);
                    success = true;
                }
                catch (IOException)
                {
                    if (timeout <= 0)
                        throw;

                    Thread.Sleep(5);
                }
            }

            return data;
        }

        /// <summary>
        /// ATTENTION: this WILL make the thread sleep a bit. You have been warned.
        /// </summary>
        /// <param name="targetPath"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public static byte[] ReadAllBytes(string targetPath, int timeout = 10)
        {
            bool success = false;
            byte[] data = { };

            if (!File.Exists(targetPath))
                return data;

            while (!success)
            {
                timeout--;

                try
                {
                    data = File.ReadAllBytes(targetPath);
                    success = true;
                }
                catch (IOException)
                {
                    if (timeout <= 0)
                        throw;

                    Thread.Sleep(5);
                }
            }

            return data;
        }
    }
}
