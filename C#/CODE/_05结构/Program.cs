using System;

namespace _05结构
{
    public struct People
    {
        public int age;
        public char name;
    }
    class Program
    {
        static void Main(string[] args)
        {
            People[] people=new People[10];

            for (int i=0;i<10;i++)
            {
                people[i].age = i + 10;
                people[i].name = (char)(i + 60);
            }

            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine("{0} age:{1},name:{2}", i, people[i].age, people[i].name);
            }
        }
    }
}
