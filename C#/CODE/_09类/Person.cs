using System;
using System.Collections.Generic;
using System.Text;

namespace _09类
{
    class Person
    {
        public string name="1";
        public int age;


        public void Func1()
        {
            Console.WriteLine("this is person's fun");
            //非静态方法，又叫实例成员
        }
        public static void Func2()
        {

            Console.WriteLine("1111111");
            //静态方法
        }


    }
}
