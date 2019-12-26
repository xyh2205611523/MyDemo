using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 泛型
{
    class Program
    {
        static void Main(string[] args)
        {
            #region 概述
            //在1.0版的时候，还没有泛型这个概念，那么怎么办呢。相信很多人会想到了OOP三大特性之一 的继承，我们知道，C#语言中，object是所有类型的基类         
            //1、object类型是一切类型的父类。           
            //2、通过继承，子类拥有父类的一切属性和行为，任何父类出现的地方，都可以用子类来代替。
            //但是上面object类型的方法又会带来另外一个问题：装箱和拆箱，会损耗程序的性能。
            //微软在C#2.0的时候推出了泛型，可以很好的解决上面的问题。

            //在泛型类型或方法定义中，类型参数是在其实例化泛型类型的一个变量时，客户端指定的特定类型的占位符。 泛型类(GenericList<T>)无法按原样使用，因为它不是真正的类型；它更像是类型的蓝图。 若要使用 GenericList<T>，客户端代码必须通过指定尖括号内的类型参数来声明并实例化构造类型。 此特定类的类型参数可以是编译器可识别的任何类型。 可创建任意数量的构造类型实例，其中每个使用不同的类型参数。


            //泛型是延迟声明的：即定义的时候没有指定具体的参数类型，把参数类型的声明推迟到了调用的时候才指定参数类型。 延迟思想在程序架构设计的时候很受欢迎。例如：分布式缓存队列、EF的延迟加载等等。泛型究竟是如何工作的呢？

            //控制台程序最终会编译成一个exe程序，exe被点击的时候，会经过JIT(即时编译器)的编译，最终生成二进制代码，才能被计算机执行。泛型加入到语法以后，VS自带的编译器又做了升级，升级之后编译时遇到泛型，会做特殊的处理：生成占位符。再次经过JIT编译的时候，会把上面编译生成的占位符替换成具体的数据类型。请看下面一个例子：
            //1 Console.WriteLine(typeof(List<>));
            //2 Console.WriteLine(typeof(Dictionary<,>));
            //泛型在编译之后会生成占位符。
            //注意：占位符需要在英文输入法状态下才能输入，只需要按一次波浪线(数字1左边的键位)的键位即可，不需要按Shift键。

            //https://www.cnblogs.com/FoR_Oscar/p/11189337.html 文章地址
            #endregion

            //泛型的协变与逆变
            Animal a = new Animal();// 直接声明Animal类
            Cat c = new Cat();//直接声明Cat类
            // 声明子类对象指向父类,如果声明一个父类对象指向子类对象，会调用父类的方法。
            Animal animal2 = new Cat();
           
            // 声明Animal类的集合
            List<Animal> listAnimal = new List<Animal>();
            // 声明Cat类的集合
            List<Cat> listCat = new List<Cat>();
            // List<Animal> list = new List<Cat>();//这样声明是错误的：因为List<Cat>和List<Animal>之间没有父子关系。
            //这时就可以用到协变和逆变了。
            // 协变
            IEnumerable<Animal> List1 = new List<Animal>();
            IEnumerable<Animal> List2 = new List<Cat>();
            //F12查看定义：IEnumerable
            //可以看到，在泛型接口的T前面有一个out关键字修饰，而且T只能是返回值类型，不能作为参数类型，这就是协变。使用了协变以后，左边声明的是基类，右边可以声明基类或者基类的子类。

            //协变除了可以用在接口上面，也可以用在委托上面：
            Func<Animal> func = new Func<Cat>(() => { return null;});
            // 使用自定义协变
            ICustomerListOut<Animal> customerList1 = new CustomerListOut<Animal>();
            customerList1.Get();
            ICustomerListOut<Animal> customerList2 = new CustomerListOut<Cat>();
            customerList2.Get();
            // 使用自定义逆变
           ICustomerListIn<Cat> customerListCat1 = new CustomerListIn<Cat>();
            customerListCat1.Show(c);
           ICustomerListIn<Cat> customerListCat2 = new CustomerListIn<Animal>();
            customerListCat2.Show(c);
            //协变+逆变
            IMyList<Cat, Animal> myList1 = new MyList<Cat, Animal>();
            IMyList<Cat, Animal> myList2 = new MyList<Cat, Cat>();//协变
            IMyList<Cat, Animal> myList3 = new MyList<Animal, Animal>();//逆变
            IMyList<Cat, Animal> myList4 = new MyList<Animal, Cat>();//逆变+协变

            //            公式：

            //协变：IFoo < 父类 > = IFoo < 子类 >；

            //逆变：IBar < 子类 > = IBar < 父类 >；

            //            1.1 协变
            //协变指能够使用比原始指定的派生类型的派生程度更小(不太具体的)的类型。如 string 到 object 的转换。多见于类型参数用作方法的返回值。

            //1.2 逆变
            //逆变指能够使用比原始指定的派生类型的派生程度更大（更具体的）的类型。如 object 到 string 的转换。多见于类型参数用作方法的输入值。

            //泛型类型参数支持协变和逆变，可在分配和使用泛型类型方面提供更大的灵活性。

            //2.怎么理解
            //假如有一个 sub 子类和 parent 父类，我们可以很轻易地将 sub 转换成 parent，这是类型安全的，反之则不行。其实很好理解，子类必然具有相同或更多的属性和方法，所以转换成属性和方法可能更少的父类，只需要去除自身的部分属性和方法即可，这对编译器而言是可行的。反之，你怎么要求编译器为父类增加更多的成员呢。数组也继承了这一特性，对于一个string[] 类型而言
            //            很多人可能不不能很好地理解这些来自于物理和数学的名词。我们无需去了解他们的数学定义，但是至少应该能分清协变和反变。实际上这个词来源于类型和类型之间的绑定。我们从数组开始理解。数组其实就是一种和具体类型之间发生绑定的类型。数组类型Int32[] 就对应于Int32这个原本的类型。任何类型T都有其对应的数组类型T[]。那么我们的问题就来了，如果两个类型T和U之间存在一种安全的隐式转换，那么对应的数组类型T[] 和U[]之间是否也存在这种转换呢？这就牵扯到了将原本类型上存在的类型转换映射到他们的数组类型上的能力，这种能力就称为“可变性（Variance）”。在.NET世界中，唯一允许可变性的类型转换就是由继承关系带来的“子类引用->父类引用”转换。举个例子，就是String类型继承自Object类型，所以任何String的引用都可以安全地转换为Object引用。我们发现String[] 数组类型的引用也继承了这种转换能力，它可以转换成Object[] 数组类型的引用，      ***   数组这种与原始类型转换方向相同的可变性就称作协变（covariant）。

            //由于数组不支持反变性，我们无法用数组的例子来解释反变性，所以我们现在就来看看泛型接口和泛型委托的可变性。假设有这样两个类型：TSub是TParent的子类，显然TSub型引用是可以安全转换为TParent型引用的。如果一个泛型接口IFoo < T >，IFoo<TSub> 可以转换为IFoo<TParent>的话，我们称这个过程为协变，而且说这个泛型接口支持对T的协变。而如果一个泛型接口IBar<T>，IBar<TParent> 可以转换为T< TSub > 的话，我们称这个过程为反变（contravariant），而且说这个接口支持对T的反变。     ***   因此很好理解，如果一个可变性和子类到父类转换的方向一样，就称作协变；而如果和子类到父类的转换方向相反，就叫反变性。你记住了吗？
            //https://www.cnblogs.com/Ninputer/archive/2008/11/22/generic_covariant.html

        }
    }

    class Animal
    {
        public int id { get; set; }
    }
    class Cat:Animal
    {
        public string name { get; set; }
    }

    //除了使用.NET框架定义好的以为，我们还可以自定义协变，例如：
    /// <summary>
    /// out 协变 只能是返回结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICustomerListOut<out T>
    {
        T Get();
    }
    public class CustomerListOut<T> : ICustomerListOut<T>
    {
        public T Get()
        {
            return default(T);
        }
    }

    //在泛型接口的T前面有一个In关键字修饰，而且T只能方法参数，不能作为返回值类型，这就是逆变。请看下面的自定义逆变：
    /// <summary>
    /// 逆变 只能是方法参数
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICustomerListIn<in T>
    {
        void Show(T t);
    }
    public class CustomerListIn<T> : ICustomerListIn<T>
    {
        public void Show(T t)
        {
        }
    }

    //协变和逆变也可以同时使用，看看下面的例子：
    /// <summary>
    /// inT 逆变
    /// outT 协变
    /// </summary>
    /// <typeparam name="inT"></typeparam>
    /// <typeparam name="outT"></typeparam>
    public interface IMyList<in inT, out outT>
    {
        void Show(inT t);
        outT Get();
        outT Do(inT t);
    }

    public class MyList<T1, T2> : IMyList<T1, T2>
    {

        public void Show(T1 t)
        {
            Console.WriteLine(t.GetType().Name);
        }

        public T2 Get()
        {
            Console.WriteLine(typeof(T2).Name);
            return default(T2);
        }

        public T2 Do(T1 t)
        {
            Console.WriteLine(t.GetType().Name);
            Console.WriteLine(typeof(T2).Name);
            return default(T2);
        }
    }

    class testModel<T>
    {
        List<T> ts = new List<T>();

        public T MyProperty { get; set; }

        public void test<T>()
        {

        }

    }

}
