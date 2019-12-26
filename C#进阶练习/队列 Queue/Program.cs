using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace 队列_Queue
{
    class Program
    {
        static void Main(string[] args)
        {
            //queue不是线程安全。线程安全要使用ConcurrentQueue队列。
            //TaskTest.QueueMian();

            //异步队列
            try
            {
                Stopwatch stopWatch = new Stopwatch();
                //TaskTest.QueueMian();
                
                TaskTest2.QueueMian();
                Console.WriteLine("------------------------------------------------------");
                TaskTest2.QueueC();
                Console.ReadLine();
            }
            catch (Exception e)
            {

                throw;
            }

            Console.ReadLine();
        }
    }

    class QueueTest
    {
        public static Queue<string> q = new Queue<string>();//queue不是线程安全
        public static ConcurrentQueue<string> cq = new ConcurrentQueue<string>();//线程安全
        public int getQueueCount() {
            return q.Count;
        }
        public static int getcqCount()
        {
            return cq.Count;
        }
        #region Queue
        /// <summary> 
        /// 队列添加数据（入队）
        /// </summary>
        public void intoData(string qStr)
        {
            string thearid = Thread.CurrentThread.ManagedThreadId.ToString();
            q.Enqueue(qStr);
            Console.WriteLine($"当前线程id:{thearid};  队列添加数据: {qStr}");
        }
        /// <summary>
        /// 队列输出数据（出队）
        /// </summary>
        public string outData()
        {
            string thearid = Thread.CurrentThread.ManagedThreadId.ToString();
            string str = q.Dequeue();
            Console.WriteLine($"当前线程id:{thearid};  队列输出数据: {str}");
            return str;
        }
        #endregion

        #region ConcurrentQueue

        public static void cqIntoData(string qStr)
        {
            string threadId = System.Threading.Thread.CurrentThread.ManagedThreadId.ToString();
            cq.Enqueue(qStr);
            System.Threading.Thread.Sleep(10);
            Console.WriteLine($"队列添加数据: {qStr};当前线程id:{threadId}");
        }
       

       
        public static void cqOutData()
        {
            string threadId = System.Threading.Thread.CurrentThread.ManagedThreadId.ToString();
            foreach (var item in cq)
            {

                Console.WriteLine($"------队列输出数据: {item};当前线程id:{threadId}");
                string d = "";
                cq.TryDequeue(out d);
            }         
        }
        #endregion


    }
    class TaskTest
    {

        #region 队列的操作模拟
        public static void QueueMian()
        {
            QueueA();
            QueueB();
        }
        /// <summary>
        /// 异步插入数据
        /// </summary>
        private static async void QueueA()
        {
            QueueTest queue = new QueueTest();
            var task = Task.Run(() =>
            {
                for (int i = 0; i < 20; i++)
                {
                    queue.intoData("QueueA" + i);
                }
            });
            await task;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("QueueA插入完成,进行输出:");

            while (queue.getQueueCount() > 0)
            {
                queue.outData();
            }
        }

        private static async void QueueB()
        {
            QueueTest queue = new QueueTest();
            var task = Task.Run(() =>
            {
                for (int i = 0; i < 20; i++)
                {
                    queue.intoData("QueueB" + i);
                }
            });
            await task;
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine("QueueB插入完成,进行输出:");

            while (queue.getQueueCount() > 0)
            {
                queue.outData();
            }
        }
        #endregion

    }

  class TaskTest2
    {
        #region 队列的操作模拟
        public static async void QueueMian()
        {
            QueueA();
            QueueB();
        }
        private static async void QueueA()
        {
            var task = Task.Run(() =>
            {
                for (int i = 0; i < 20; i++)
                {
                    QueueTest.cqIntoData("cqA" + i);
                }
            });
            await task;
            Console.WriteLine("QueueA插入完成,进行输出:");
        }

        private static async void QueueB()
        {
            var task = Task.Run(() =>
            {
                for (int i = 0; i < 20; i++)
                {
                    QueueTest.cqIntoData("cqB" + i);
                }
            });
            await task;
            Console.WriteLine("QueueB插入完成,进行输出:");

        }

        public static void QueueC()
        {
            Console.WriteLine("Queue插入完成,进行输出:");
            while (QueueTest.getcqCount() > 0)
            {
                QueueTest.cqOutData();
            }
        }
        #endregion
    }



}
