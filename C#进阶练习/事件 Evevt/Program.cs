using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 事件_Evevt
{
    class Program
    {
        static void Main(string[] args)
        {
            #region 概念
            // 事件是C#中另一高级概念，使用方法和委托相关。奥运会参加百米的田径运动员听到枪声，比赛立即进    行。其 中枪声是事件，而运动员比赛就是这个事件发生后的动作。不参加该项比赛的人对枪声没有反应。

            //从程序员的角度分析，当裁判员枪声响起，发生了一个事件，裁判员通知该事件发生，参加比赛的运动员仔细听枪声是  否发    生。运动员是该事件的订阅者，没有参赛的人不会注意，即没有订阅该事件。

            //C#中使用事件需要的步骤：
            //1、创建一个委托；
            //2、将创建的委托与特定事件关联(.Net类库中的很多事件都是已经定制好的，所以他们也就有相应的一个委托，在编写   关联 C#事件处理程序--也就是当有事件发生时我们要执行的方法的时候我们需要和这个委托有相同的签名)；
            //3、编写C#事件处理程序；
            //4、利用编写的C#事件处理程序生成一个委托实例；
            //5、把这个委托实例添加到产生事件对象的事件列表中去，这个过程又叫订阅事件。

            //            一、定义事件

            //定义事件时，发生者首先要定义委托，然后根据委托定义事件。定义事件的语法如下：

            //< 访问修饰符 > event 委托名 事件名;

            //        定义事件时，一定要有一个委托类型，用这个委托类型来定义处理事件的方法类型。下面定义一个发布者类，并在其内部定义eventRun事件。

            //复制代码
            //复制代码
            //复制代码
            //1 class Judgment
            //2 {
            //3     //定义一个委托
            //4     public delegate void delegateRun();
            //5     //定义一个事件
            //6     public event delegateRun eventRun;
            //7 }
            //    复制代码
            //    复制代码
            //复制代码
            //二、订阅事件

            //定义好事件后，与事件有关的人会订阅事件，只有订阅事件的对象才会收到发生事件的通知，没有订阅该事件的对象则不会收到通知。订阅事件的语法如下：

            //事件名+=new 委托名（方法名）;

            //假如方法名为Run，那么订阅事件eventRun的代码如下：

            //judgment.eventRun+=new Judgment.delegateRun(runsport.Run);

            //judgment为类Judgment的对象，runsport为运动员类RunSports的对象，Run为其中的方法。

            //事件的订阅通过“+=”操作符来实现，可以给事件加一个或多个方法委托。

            //三、引发事件

            //一般都是在满足某个条件下引发事件，裁判员枪声一响，引发运动员奔跑这个事件。在编程中可以用条件诘句，也可以使用方法引发事件。

            //public void Begin()
            //    {
            //        enentRun();
            //    }
            //    这段代码中，通过Begin方法引发事件enentRun。引发事件的语法与调用方法的语法相同，引发该事件时，将调用订阅事件的对象的所有委托。下面代码演示裁判员枪声响起到引发运动员比赛的动作，完整代码：
            #endregion

            RunSports runsport = new RunSports();//实例化事件发布者
             Judgment judgment = new Judgment();//实例化事件订阅者
             //订阅事件：定义好事件后，与事件有关的人会订阅事件，只有订阅事件的对象才会收到发生事件的通知，没有订阅该事件的对象则不会收到通知。订阅事件的语法如下：
             //事件的订阅通过“+=”操作符来实现，可以给事件加一个或多个方法委托。
             judgment.eventRun += new Judgment.delegateRun(runsport.Run);//事件名+=new 委托名（方法名）;
             //引发事件
            judgment.Begin();
            Console.ReadKey();
        }
        class Judgment
         { 
             //定义一个委托
             public delegate void delegateRun();//无参数，无返回值的委托
             //定义一个事件
             public event delegateRun eventRun;//< 访问修饰符 > event 委托名 事件名;
             //引发事件的方法
            public void Begin()
             {
                 eventRun();//被引发的事件
             } 
         }
        class RunSports
     { 
         //定义事件处理方法
         public void Run()
         {
             Console.WriteLine("运动员开始比赛");
         }
     }



    }
}
