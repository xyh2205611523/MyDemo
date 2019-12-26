using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpRequestDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            //http://www.webwisdombuild.com/   http://www.webwisdombuild.com/Index/Index
            HttpRequestClient client = null;
            
            var strUrl = "";
            var isGAndP = "";
            var param = "";

            client = new HttpRequestClient();
            Console.WriteLine("请输入要请求的地址！");
            strUrl = Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("开始模拟请求!!!");
            //var result= client.httpPost(strUrl, HttpRequestClient.defaultHeaders, param, Encoding.UTF8);
            var result = client.httpGet(strUrl, HttpRequestClient.defaultHeaders);
            Console.WriteLine(result);

            strUrl = Console.ReadLine();
            //while (strUrl!="abcdefg")
            //{
            //    Console.WriteLine("请输入要请求的地址！");
            //    strUrl = Console.ReadLine();
            //    Console.ForegroundColor = ConsoleColor.DarkGreen;
            //    Console.WriteLine("开始模拟请求!!!");

            //    Console.ForegroundColor = ConsoleColor.DarkCyan;
            //    Console.WriteLine("get/post");

            //    client = new HttpRequestClient();
            //    client.httpGet(strUrl, HttpRequestClient.defaultHeaders);
            //    strUrl = Console.ReadLine();
            //}


        }
    }
}
