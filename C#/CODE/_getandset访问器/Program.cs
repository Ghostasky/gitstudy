using System;

namespace _11getandset访问器
{
    class Program
    {
        static void Main(string[] args)
        {
            Person aa = new Person();
            aa.age = 14;
            Console.WriteLine("{0}", aa.Age);
            Console.WriteLine("Hello World!");
        }
    }
}
