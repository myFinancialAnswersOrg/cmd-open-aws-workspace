using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWS_Restarter
{
    class Program
    {
        static void Main(string[] args)
        {
            Restarter restarter = new Restarter("<REGISTRATION CODE HERE>");
            restarter.Restart("<ORGANIZATION NAME HERE>", "<USERNAME HERE>", "<PASSWORD HERE>");
        }
    }
}
