using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IsLocked
{
    class Program
    {
        static void Main(string[] args)
        {

            if (args.Length > 0)
            {
                Console.WriteLine("Joe Ostrander");
                Console.WriteLine("Checks to see if workstation is locked");
                Console.WriteLine("(output can be used in scripts)");
                Console.WriteLine("2018.02.23");
            }
            else
            {
                WorkStationReader workStationReader = new WorkStationReader();
                WorkStationReader.LockStatus status = workStationReader.GetLockStatus();
                Console.WriteLine(status.Message);
            }
            
        }
    }
}
