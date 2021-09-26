using System;

namespace _09类
{
    class Program
    {
        static void Main(string[] args)
        {
            Person person=new Person();
            person.name = "123";
            person.age = 14;
            person.Func1();

            //不能person.Func2()
            //但是可以这样
            Person.Func2();//静态方法

        }
    }
}
