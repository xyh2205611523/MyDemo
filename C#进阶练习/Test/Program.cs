using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            string num = "5";
           bool bo= int.TryParse(num,out int numr);
            Console.WriteLine("结果："+bo+"   值："+numr);

            Console.ReadLine();
        }
    }
}
