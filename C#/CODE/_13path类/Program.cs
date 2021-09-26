using System;
using System.IO;

namespace _13path类
{
    class Program
    {
        static void Main(string[] args)
        {
            string str = @"D:\background\1.png";
            Console.WriteLine(Path.GetFileName(str));

            Console.WriteLine(Path.GetFileNameWithoutExtension(str));

            Console.WriteLine(Path.GetExtension(str));
            Console.WriteLine(Path.GetDirectoryName(str));
            //Console.WriteLine("Hello World!");
        }
    }
}
