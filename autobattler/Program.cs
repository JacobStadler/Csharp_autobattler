using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace autobattler
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Timer timer = new Timer(Mainblock,null,0,1000); // 1000 ms = 1 second
    
            // This line is used to prevent the program from exiting immediately.
            // You can use other synchronization mechanisms to control the program's lifecycle.
            Console.WriteLine("Press Enter to exit.");
            Console.ReadLine();
        }
        static void Mainblock(object state){
            Console.WriteLine($"Main block executed at {DateTime.Now}");
        }
    }
}