using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Threading.Tasks;
using AWS_Restarter.Utilities;

namespace AWS_Restarter
{

    class Program
    {
        static void Main(string[] args)
        {
            Restarter restarter = new Restarter(Settings.Get("REGISTRATION_CODE"));
            restarter.Restart(Settings.Get("ORGANIZATION"), Settings.Get("USERNAME"), Settings.Get("PASSWORD"));
        }
    }
}
