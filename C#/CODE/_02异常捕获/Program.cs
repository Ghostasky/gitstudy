using System;

namespace _02异常捕获
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
             * 语法上没有错误，在程序运行的过程中，由于某些原因程序出现了错误，不能正常运行
             * 可以使用try-catch
             */


            try
            {
                //可能会出现异常的代码
            }
            catch
            {
                //出现异常后要执行的代码
            }
        }
    }
}
