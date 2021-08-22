using System;

namespace _07ref参数
{
    class Program
    {
        static void Main(string[] args)
        {
            double fakuan = 4000;
            Fakuan(ref fakuan);
            Console.WriteLine(fakuan);
        }











        public static void  Fakuan(ref double fakuan)
        {
            fakuan -= 100;
        }
    }
}
