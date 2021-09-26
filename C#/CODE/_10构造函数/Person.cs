using System;
using System.Collections.Generic;
using System.Text;

namespace _10构造函数
{
    public class Person
    {
        public string name = "";
        int math;
        int chinese;
       public Person(string name,int math,int chinese)
       {
            this.name = name;
            this.math = math;
            this.chinese = chinese;
            Console.WriteLine("init is OK.");
       }
       
        public void Print()
        {
            Console.WriteLine("name is {0}", this.name);
            Console.WriteLine("math is {0}", this.math);
            Console.WriteLine("chinese is {0}", this.chinese);
        }
        ~Person()
        {
            Console.WriteLine("i am end ");
        }
    }
}
