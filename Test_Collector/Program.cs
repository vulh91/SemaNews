using SemaNews.Collector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_Collector
{
    class Program
    {
        static void Main(string[] args)
        {
            CollectorEngine engine = new CollectorEngine();
            engine.Start();
            Console.ReadLine();
        
        }
    }
}
