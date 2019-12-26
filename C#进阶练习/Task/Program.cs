using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task
{
    class Program
    {
        static void Main(string[] args)
        {
            //Task 类的表示单个操作不返回一个值，通常以异步方式执行。 Task 对象是一个的中心思想 基于任务的异步模式 首次引入.NET Framework 4 中。 因为由执行工作 Task 对象通常以异步方式执行在线程池线程上而不是以同步方式在主应用程序线程，您可以使用 Status 属性，以及 IsCanceled, ，IsCompleted, ，和 IsFaulted 属性，以确定任务的状态。 大多数情况下，lambda 表达式用于指定的任务是执行的工作。
            //    对于返回值的操作，您使用 Task 类。
            //    任务Task和线程Thread的区别：
            //    1、任务是架构在线程之上的，也就是说任务最终还是要抛给线程去执行。
            //    2、任务跟线程不是一对一的关系，比如开10个任务并不是说会开10个线程，这一点任务有点类似线程池，但是任务相比线程池有很小的开销和精确 的控   制。
            //    Task和Thread一样，位于System.Threading命名空间下!
             //Task是微软在.net framework 4.0发布的新的异步编程的利器，当然4.5新增了async、await，这儿我们先说Task相关。
　　          //在实际编程中，我们用的较多的是Task、Task.Factory.StarNew、Task.Run，接下来简单的表述下我的理解。

            //传入Func委托
            Task<int> task = new Task<int>(a=> { return (int)a + 1; },1);
            task.Start();
            Console.WriteLine(task.Result);

            //传入Action委托
            System.Threading.Tasks.Task task1 = new System.Threading.Tasks.Task(() => Console.WriteLine("传入Action委托"));
            task1.Start();

            //Task.Run 或 TaskFactory.StartNew（工厂创建） 方法是用于创建和计划计算的任务的首选的机制，但对于创建和计划必须分开的方案，您可以使用的构造函数（new一个出来），然后调用 Task.Start 方法来计划任务，以在稍后某个时间执行。
            //实例化的Task对象，需要调用Start来启动任务，如果使用Task.Factory.StarNew，则不用调用Start方法来启动任务。
            Task<string> task2 = System.Threading.Tasks.Task.Factory.StartNew(()=> { return "Func"; });
            Console.WriteLine(task2.Result);
            System.Threading.Tasks.Task task3 = System.Threading.Tasks.Task.Factory.StartNew(() => Console.WriteLine("Action"));

            //Task.Run的跟Task.Factory.StarNew和new Task相差不多，不同的是前两种是放进线程池立即执行，而Task.Run则是等线程池空闲后在执行。
            //Run方法只接受无参的Action和Func委托，另外两个接受一个object类型的参数。
            //备注：只有当你需要一个长期运行的计算机绑定任务的高密度控件时，使用StartNew方法.从.NET Framework4.5开始，Task.Run方法是启动一个计算机绑定的任务推荐的使用方式

            //它们都可以调用Wait方法来阻塞当前线程，还可以通过Task.Result来获取返回值，当然它也会阻塞当前线程。

            //接下来再说说常用的ContinueWith，这个说白了就是在某个任务执行完的延续，类似callback
            //continuewith接受action或func委托，委托的第一个参数都是task类型，可以通过它访问先前的task对象
            var task4 = System.Threading.Tasks.Task.Factory.StartNew<int>(() => { return 1; });
            Console.WriteLine("task4:"+ task4.Result);
            var result = task4.ContinueWith<int>((aa => { return aa.Result + 1; }));
            Console.WriteLine("task4 ContinueWith:" + result.Result);

            Console.Read();
        }

        //这是线程不安全，直接调用外部参数
        static void TestRun(string Name, int Age)
        {
            System.Threading.Tasks.Task.Factory.StartNew(() => Console.WriteLine("name:{0} age:{1}", Name, Age));
        }

        //如果你确定底层封装好了，可以像上面那样写，但建议写成下面这种
        static void TestRun1(string Name, int Age)
        {
            System.Threading.Tasks.Task.Factory.StartNew(obj =>
            {
                var o = (dynamic)obj;//dynamic:在编译时不确定其类型，只有在运行时才确定其类型
                Console.WriteLine("name:{0} age:{1}", o.Name, o.Age);
            }, new { Name, Age });
        }




    }
}
