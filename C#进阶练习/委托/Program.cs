using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 委托
{
    class Program
    {
        static FileStream fs;
        static StreamWriter sw;
        // 委托声明
        public delegate void printString(string s);

        /// <summary>
        /// 该方法打印到控制台
        /// </summary>
        /// <param name="str"></param>
        public static void WriteToScreen(string str)
        {
            Console.WriteLine("The String is: {0}", str);
        }
        /// <summary>
        /// 该方法打印到文件
        /// </summary>
        /// <param name="s"></param>
        public static void WriteToFile(string s)
        {
            fs = new FileStream("e:\\message.txt", FileMode.Append, FileAccess.Write);
            sw = new StreamWriter(fs);
            sw.WriteLine(s);
            sw.Flush();
            sw.Close();
            fs.Close();
        }
        /// <summary>
        /// 该方法把委托作为参数，并使用它调用方法
        /// </summary>
        /// <param name="ps"></param>
        public static void sendString(printString ps)
        {
            ps("Hello World");
        }

        //声明委托
        delegate int testDel(int param);
        static void Main(string[] args)
        {
            //委托是一个类，它定义了方法的类型，使得可以将方法当作另一个方法的参数来进行传递，这种将方法动态地赋给参数的做法，可以避免在程序中大量使用If-Else(Switch)语句，同时使得程序具有更好的可扩展性。
            //委托说白了就是可以将方法作为一个参数进行传递
            #region 委托（Delegate）

            if (true)
            {
                //C# 中的委托（Delegate）类似于 C 或 C++ 中函数的指针。委托（Delegate） 是存有对某个方法的引用的一种引用类型变量。引用可在运行时被改变。
                //委托（Delegate）特别用于实现事件和回调方法。所有的委托（Delegate）都派生自 System.Delegate 类。
                //声明委托（Delegate）：委托声明决定了可由该委托引用的方法。委托可指向一个与其具有相同标签的方法。
                //声明委托的语法如下：delegate <return type > < delegate- name > < parameter list >

                //    一旦声明了委托类型，委托对象必须使用 new 关键字来创建，且与一个特定的方法有关。当创建委托时，传递到 new 语句的参数就像方法调用一样书写，但是不带有参数。例如：
                //public delegate void printString(string s);
                //printString ps1 = new printString(WriteToScreen);
                //printString ps2 = new printString(WriteToFile);

                Console.WriteLine("开始的值：" + getNum());

                // 创建委托实例
                testDel td1 = new testDel(addNum); //该委托可用于引用带有一个整型参数的方法，并返回一个整型值
                testDel td2 = new testDel(multiplyNum);

                // 使用委托对象调用方法
                Console.WriteLine("调用方法addNum：" + getNum());
                td1(25);
                Console.WriteLine("Value of Num: {0}", getNum());
                Console.WriteLine("调用方法multiplyNum：" + getNum());
                td2(5);
                Console.WriteLine("Value of Num: {0}", getNum());

                //委托的多播（Multicasting of a Delegate）
                //委托对象可使用 "+" 运算符进行合并。一个合并委托调用它所合并的两个委托。只有相同类型的委托可被合并。"-"  运   算符 可用于从合并的委托中移除组件委托。          
                //使用委托的这个有用的特点，您可以创建一个委托被调用时要调用的方法的调用列表。这被称为委托的 多播（multicasting），也叫组播。
                testDel td;
                td = td1;
                td += td2;
                Console.WriteLine("调用多播委托：" + getNum());
                td(13);
                Console.WriteLine("Value of Num: {0}", getNum());

                printString ps1 = new printString(WriteToScreen);
                printString ps2 = new printString(WriteToFile);
                sendString(ps1);
                sendString(ps2);
               

            }

            #endregion


            #region Action、Action<T>、Func<T>、Predicate<T> 
            //Action、 Action < T >、Func < T >、Predicate < T >，其本质上都是delegate关键字来声明的

            //Action<T>
            //Action<T> 是Action的泛型实现，也没有返回值，但可以传入最多16个参数，两个参数的声明原型为：
            //public delegate void Action<in T1, in T2>(T1 arg1, T2 arg2);

            //private void ShowResult(int a, int b)
            //{
            //    Console.WriteLine(a + b);
            //}
            //Action<int, int> t = new Action<int, int>(ShowResult);//两个参数但没返回值的委托
            //t(2, 3);
            //同样也可以直接用Lambd表达式直接把方法定义在委托中，代码如下：
            //Action<int, int> t = (a, b) => { Console.WriteLine(a + b); };
            //t(2, 3);

            //Func<T>
            //Func委托始终都会有返回值，返回值的类型是参数中最后一个，可以传入一个参数，也可以最多传入16个参数，    但可  以传    入最多16个参数，两个参数一个返回值的声明原型为：

            //public delegate TResult Func<in T1, in T2, out TResult>(T1 arg1, T2 arg2);
            //用法如下：
            //public bool Compare(int a, int b)
            //{
            //    return a > b;
            //}
            //Func<int, int, bool> t = new Func<int, int, bool>(Compare);//传入两个int参数，返回bool值
            //bool result = t(2, 3);
            //同样也可以直接用Lambd表达式直接把方法定义在委托中，代码如下：
            //Func<int, int, bool> t = (a, b) => { return a > b; };
            //bool result = t(2, 3);

            //Predicate<T>
            //Predicate<T> 委托表示定义一组条件并确定指定对象是否符合这些条件的方法，返回值始终为bool类
            //    声明原型为：
            //public delegate bool Predicate<int T>(T obj);
            //用法如下：
            //public bool Match(int val)
            //{
            //    return val > 60;
            //}
            //Predicate<int> t = new Predicate<int>(Match);//定义一个比较委托
            //int[] arr = { 13, 45, 26, 98, 3, 56, 72, 24 };
            //int first = Array.Find(arr, t);//找到数组中大于60的第一个元素
            //同样也可以直接用Lambd表达式直接把方法定义在委托中，代码如下：
            //Predicate<int> t = val => { return val > 60; };//定义一个比较委托
            //int[] arr = { 13, 45, 26, 98, 3, 56, 72, 24 };
            //int first = Array.Find(arr, t);//找到数组中大于60的第一个元素

            //总结
            //如果要委托的方法没有参数也没有返回值就想到Action
            //有参数但没有返回值就想到Action<T>
            //无参数有返回值、有参数且有返回值就想到Func<T>
            //有bool类型的返回值，多用在比较器的方法，要委托这个方法就想到用Predicate<T>

            //例子
            //List<int> arr_Int = new List<int>() { 1, 2, 23, 42, 33, 221, 213 };
            //bool bIsExist = arr_Int.Exists(n => n > 250);
            //int c = arr_Int.Find(n => n == 2);
            //int nFirst = arr_Int.First<int>();
            //arr_Int.ForEach(n => { });
            //arr_Int.Insert(1, 11111);
            //arr_Int.Reverse();
            //List<int> INTArr = arr_Int.Select((n, Tresult) => Tresult = n).ToList();
            //arr_Int.Single(n => n == 23);
            //var query = (from i in arr_Int
            //             where i > 1111
            //             select i).ToArray();




            #endregion


            Console.ReadKey();
        }
        static int constNum = 10;
        public static int addNum(int num)
        {
            return constNum+=num;
        }
        public static int minusNum(int num)
        {
            return constNum -= num ;
        }
        public static int multiplyNum(int num)
        {
            return constNum *= num;
        }
        public static int eliminateNum(int num)
        {
            return constNum /= num;
        }
        public static int getNum()
        {
            return constNum;
        }
        
    }
}
