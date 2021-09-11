using System;

namespace _07ref参数
{
    class Program
    {
        static int Main(string[] args)
        {
            double fakuan = 4000;
            Fakuan(ref fakuan);
            Console.WriteLine(fakuan);
            return 0;
        }

        public static void Fakuan(ref double fakuan)
        {
            fakuan -= 100;
        }
    }
}
