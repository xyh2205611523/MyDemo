using System;
using System.IO;

namespace IO流
{
    class Program
    {
        static void Main(string[] args)
        {
            //参考   https://www.cnblogs.com/liyangLife/p/4797583.html
            //            (1)文件系统类的介绍
            //    文件操作类大都在System.IO命名空间里。FileSystemInfo类是任何文件系统类的基类；FileInfo与File表示文件系统中的文件；DirectoryInfo与Directory表示文件系统中的文件夹；Path表示文件系统中的路径；DriveInfo提供对有关驱动器的信息的访问。注意，XXXInfo与XXX类的区别是：XXX是静态类，XXXInfo类可以实例化。
            //    还有个较为特殊的类System.MarshalByRefObject允许在支持远程处理的应用程序中跨应用程序域边界访问对象。 
            //(2)FileInfo与File类

            // FileAction();//文件方法

            //(3)DirectoryInfo与Directory类
            DirectoryInfo di = new DirectoryInfo(@"E:\学习笔记\C#平台\test");//创建文件夹
            di.Create();//创建一个目录。
            Console.WriteLine("父文件夹："+di.Parent.FullName);//获取目录或文件的完整路径。
            FileSystemInfo[] files = di.Parent.GetFileSystemInfos();//输出父目录下的所有文件与文件夹
            foreach (FileSystemInfo item in files)
            {
                Console.WriteLine(item.Name);
            }
            Directory.Delete(di.FullName);//删除文件夹
            Console.WriteLine("完成！");



            Console.Read();
        }


public static void FileAction()
        {
            FileInfo file = new FileInfo(@"E:\IO流练习.txt");//创建文件
            Console.WriteLine("创建时间：" + file.CreationTime);
            Console.WriteLine("路径：" + file.DirectoryName);
            StreamWriter sw = file.AppendText();//打开追加流
            sw.Write("测试，这是追加的数据");//追加数据
            sw.Dispose();
            Console.WriteLine("完成！");
        }

    }
}
