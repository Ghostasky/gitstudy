using System;

namespace _15装箱和拆箱
{
    class Program
    {
        static void Main(string[] args)
        {
            int a = 10;
            object o = a;

            int aa = (int)o;
            Console.WriteLine(a);
            Console.WriteLine(o);
            Console.WriteLine(aa);
            Console.WriteLine("Hello World!");
        }
    }
}
