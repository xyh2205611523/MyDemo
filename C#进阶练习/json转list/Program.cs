using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace json转list
{
    class Program
    {
      public  static void Main(string[] args)
        {
            string jsonStr = "[{name:'小明',age:19},{name:'小明',age:20},{name:'小明',age:21},{name:'小明',age:22}]";

            List<student> list = new List<student>();
             var result=  JsonConvert.DeserializeObject< List< student >> (jsonStr);

            Console.Read();
        }


    }
}
