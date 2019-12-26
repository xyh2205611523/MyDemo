using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

namespace ThreadDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            string strResult = "";//控制台输入的值
                                  //线程练习

            if (false)
            {
                #region  单个线程
                Thread nowTh = Thread.CurrentThread;//获取当前线程信息
                nowTh.Name = "主线程";
                Console.WriteLine($"当前线程为：{nowTh.Name.ToString()}");
                Thread.Sleep(3000); // 停止主线程一段时间



                //通过Thread创建线程      
                // ThreadStart threadStart = new ThreadStart(GetInfo);//获取线程要执行的方法,这步可不要
                //Thread th = new Thread(threadStart);
                Thread th = new Thread(GetInfo);//创建线程,这个与上面两个效果一样
                th.Name = "线程01";//获取或设置线程名称
                Console.WriteLine("是否执行（Y/N）");
                strResult = Console.ReadLine();

                if (strResult.Equals("Y") || strResult.Equals("y"))
                {
                    th.Start(th.Name);//执行线程，*Start()方法可传递参数，但只能传递一个参数并且只能是object类型的         
                    Console.WriteLine($"当前线程为：{Thread.CurrentThread.Name.ToString()}");
                }
                else
                {
                    Console.WriteLine("错误");
                }

                Console.WriteLine("是否执行线程02（Y/N）");

                Thread th1 = new Thread(GetInfo);
                th1.Name = "线程02";//获取或设置线程名称
                strResult = Console.ReadLine();

                if (strResult.Equals("Y") || strResult.Equals("y"))
                {
                    th1.Start(th1.Name);
                    Console.WriteLine($"当前线程为：{Thread.CurrentThread.Name.ToString()}");
                }
                #endregion

            }


            #region 线程池  参考文档https://www.cnblogs.com/qixuejia/p/7802092.html，https://www.cnblogs.com/scmail81/archive/2018/08/19/9503266.html
            //            需要注意的是：
            //线程池中的所有线程都是后台线程。如果进程的所有前台线程都结束了，所有的后台线程也都会停止。
            //不能将入池的线程改为前台线程。
            //不能给入池的线程设置优先级或名称。
            //入池的线程只能用于时间较短的任务。如果线程要一直运行（比如Word的拼写检查线程），就应该使用Thread类创建一个线程。
            //线程池初始化时是没有线程的，线程池里的线程的初始化与其他线程一样，但是在完成任务以后，该线程不会自行销毁，而是以挂起的状态返回到线程池。直到应用程序再次向线程池发出请求时，线程池里挂起的线程就会再度激活执行任务。
            //            CLR线程池分为工作者线程(workerThreads)与I / O线程(completionPortThreads)两种:
            //            工作者线程是主要用作管理CLR内部对象的运作，通常用于计算密集的任务。
            //I / O(Input / Output)线程主要用于与外部系统交互信息，如输入输出，CPU仅需在任务开始的时候，将任务的参数传递给设备，然后启动硬件设备即可。等任务完成的时候，CPU收到一个通知，一般来说是一个硬件的中断信号，此时CPU继续后继的处理工作。在处理过程中，CPU是不必完全参与处理过程的，如果正在运行的线程不交出CPU的控制权，那么线程也只能处于等待状态，即使操作系统将当前的CPU调度给其他线程，此时线程所占用的空间还是被占用，而并没有CPU处理这个线程，可能出现线程资源浪费的问题。如果这是一个网络服务程序，每一个网络连接都使用一个线程管理，可能出现大量线程都在等待网络通信，随着网络连接的不断增加，处于等待状态的线程将会很消耗尽所有的内存资源。可以考虑使用线程池解决这个问题。


            if (false)
            {
                // 参数:
                // workerThreads:
                // 线程池中辅助线程的最大数目。
                // completionPortThreads:
                // 线程池中异步 I/O 线程的最大数目。
                // 返回结果:如果更改成功，则为 true；否则为 false。
                //ThreadPool.SetMinThreads(1,1);//设置最小线程数
                //ThreadPool.SetMaxThreads(5,5);//设置最大线程数

                //for (int i = 0; i < 15; i++)
                //{
                //    ThreadPool.QueueUserWorkItem(ThreadPoolWork,i);//将方法添加进线程池，参数可选

                //}
                ThreadPool.QueueUserWorkItem(new WaitCallback(CountProcess));
                ThreadPool.QueueUserWorkItem(new WaitCallback(GetEnvironmentVariables));
                Thread.Sleep(5000);
                Console.WriteLine("OK");
            }


            //利用ThreadPool调用工作线程和IO线程的范例

            // 设置线程池中处于活动的线程的最大数目
            // 设置线程池中工作者线程数量为1000，I/O线程数量为1000
            ThreadPool.SetMaxThreads(1000, 1000);
            Console.WriteLine("主线程:对异步方法排队");
            PrintMessage("主线程开始");
            // 把工作项添加到队列中，此时线程池会用工作者线程去执行回调方法            
            ThreadPool.QueueUserWorkItem(asyncMethod);
            asyncWriteFile();



            #endregion

            Console.Read();
        }

        /// <summary>
        /// 线程调用的方法
        /// </summary>
        public static void GetInfo(object name)
        {
            //注意点
            //*执行完方法线程默认自动销毁，如果在无限循环中则要手动销毁
            //*C#中的线程分为前台线程和后台线程，线程创建时不做设置默认是前台线程。即线程属性IsBackground=false。
            //*这两者的区别就是：应用程序必须运行完所有的前台线程才可以退出；而对于后台线程，应用程序则可以不考虑其是否已经运行完毕而直接退出，所有的后台线程在应用程序退出时都会自动结束。一般后台线程用于处理时间较短的任务，如在一个Web服务器中可以利用后台线程来处理客户端发过来的请求信息。而前台线程一般用于处理需要长时间等待的任务，如在Web服务器中的监听客户端请求的程序。
            //线程是寄托在进程上的，进程都结束了，线程也就不复存在了！
        //只要有一个前台线程未退出，进程就不会终止！即说的就是程序不会关闭！（即在资源管理器中可以看到进程未结束。）





            Console.WriteLine("");
            Console.WriteLine("--------------------------");
            Console.WriteLine("调用了方法,线程名称为："+ name.ToString());
            Console.WriteLine($"当前线程为：{Thread.CurrentThread.Name.ToString()}");
            Console.WriteLine($"执行状态为：{Thread.CurrentThread.IsAlive}");//获取一个值，该值指示当前线程的执行状态。
            Console.WriteLine($"是否为后台线程：{Thread.CurrentThread.IsBackground}");//获取或设置一个值，该值指示某个线程是否为后台线程。
            Console.WriteLine($"执行状态为：{Thread.CurrentThread.IsThreadPoolThread}");//获取一个值，该值指示线程是否属于托管线程池。
            Console.WriteLine($"托管线程的唯一标识符：{Thread.CurrentThread.ManagedThreadId}");//获取当前托管线程的唯一标识符。
            Console.WriteLine($"调度优先级：{Thread.CurrentThread.Priority}");//获取或设置一个值，该值指示线程的调度优先级。
            Console.WriteLine($"当前线程状态：{Thread.CurrentThread.ThreadState}");//获取一个值，该值包含当前线程的状态。

            //Thread.CurrentThread.Abort();//终止当前线程

        }

        /// <summary>
        /// 线程池调用方法
        /// </summary>
       public static void ThreadPoolWork(object param)
        {
            Console.WriteLine(DateTime.Now+$" :这是第{param.ToString()}个线程");

        }

        /// <summary>  
        /// 统计当前正在运行的系统进程信息  
        /// </summary>  
        /// <param name="state"></param>  
        private static void CountProcess(object state)
        {
            Process[] process = Process.GetProcesses();//获取本地计算机上每个进程资源的组件。
            foreach (Process p in process)
            {
                try
                {
                    Console.WriteLine("进程信息:Id:{0},ProcessName:{1},StartTime:{2}", p.Id, p.ProcessName, p.StartTime);
                }
                catch (Win32Exception e)
                {
                    Console.WriteLine("ProcessName:{0}", p.ProcessName);
                }
                finally
                {
                }
            }
            Console.WriteLine("获取进程信息完毕。");
        }

        /// <summary>  
        /// 获取当前机器系统变量设置  
        /// </summary>  
        /// <param name="state"></param>  
        private static void GetEnvironmentVariables(object state)
        {
            IDictionary list = System.Environment.GetEnvironmentVariables();//从当前进程检索所有环境变量名及其值。
            foreach (DictionaryEntry item in list)
            {
                Console.WriteLine("系统变量信息:key={0},value={1}", item.Key, item.Value);
            }
            Console.WriteLine("获取系统变量信息完毕。");
        }

        #region I/O线程调用方法

        // 打印线程池信息
        private static void PrintMessage(String data)
        {
            int workthreadnumber;
            int iothreadnumber;

            // 获得线程池中可用的线程，把获得的可用工作者线程数量赋给workthreadnumber变量
            // 获得的可用I/O线程数量给iothreadnumber变量
            ThreadPool.GetAvailableThreads(out workthreadnumber, out iothreadnumber);

            Console.WriteLine("{0}\n Id is {1}\n CurrentThread is background :{2}\n WorkerThreadNumber is:{3}\n IOThreadNumbers is: {4}\n",
                data,
                Thread.CurrentThread.ManagedThreadId,//获取当前托管线程的唯一标识符。
                Thread.CurrentThread.IsBackground.ToString(),//获取或设置一个值，该值指示线程是否为后台线程。
                workthreadnumber.ToString(),
                iothreadnumber.ToString());
        }

        // 方法必须匹配WaitCallback委托
        private static void asyncMethod(object state)
        {
            Thread.Sleep(1000);
            PrintMessage("异步线程方法");
            Console.WriteLine("异步线程已经工作");
        }

        //异步读取文件模块
        private static void asyncReadFile()
        {
            byte[] byteData = new byte[1024];
            FileStream stream = new FileStream(@"E:\ThreadPoolIO123.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite, 1024, true);
            //把FileStream对象，byte[]对象，长度等有关数据绑定到FileDate对象中，以附带属性方式送到回调函数
            Hashtable ht = new Hashtable();
            ht.Add("Length", (int)stream.Length);
            ht.Add("Stream", stream);
            ht.Add("ByteData", byteData);

            //启动异步读取,倒数第二个参数是指定回调函数，倒数第一个参数是传入回调函数中的参数
            stream.BeginRead(byteData, 0, (int)ht["Length"], new AsyncCallback(Completed), ht);
            PrintMessage("异步读取文件方法");
        }

        //实际参数就是回调函数
        static void Completed(IAsyncResult result)
        {
            Thread.Sleep(2000);
            PrintMessage("异步读取文件完成方法");
            //参数result实际上就是Hashtable对象，以FileStream.EndRead完成异步读取
            Hashtable ht = (Hashtable)result.AsyncState;
            FileStream stream = (FileStream)ht["Stream"];
            int length = stream.EndRead(result);
            stream.Close();
            string str = Encoding.UTF8.GetString(ht["ByteData"] as byte[]);
            Console.WriteLine(str);
            stream.Close();
        }

        //异步写入模块
        private static void asyncWriteFile()
        {
            //文件名 文件创建方式 文件权限 文件进程共享 缓冲区大小为1024 是否启动异步I/O线程为true
            FileStream stream = new FileStream(@"E:\ThreadPoolIO123.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite, 1024, true);
            //这里要注意，如果写入的字符串很小，则.Net会使用辅助线程写，因为这样比较快
            byte[] bytes = Encoding.UTF8.GetBytes("你在他乡还好吗？");
            //异步写入开始，倒数第二个参数指定回调函数，最后一个参数将自身传到回调函数里，用于结束异步线程
            stream.BeginWrite(bytes, 0, (int)bytes.Length, new AsyncCallback(Callback), stream);
            PrintMessage("异步写入文件方法");
        }

        static void Callback(IAsyncResult result)
        {
            //显示线程池现状
            Thread.Sleep(2000);
            PrintMessage("异步写入文件回调方法");
            //通过result.AsyncState再强制转换为FileStream就能够获取FileStream对象，用于结束异步写入
            FileStream stream = (FileStream)result.AsyncState;
            stream.EndWrite(result);
            stream.Flush();
            stream.Close();
            asyncReadFile();
        }

        #endregion


    }
}
