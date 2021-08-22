using System;

namespace code_test//命名空间
{
    class Retangle
    {
        double length;
        double witdth;
        public void Init()
        {
            length = 10;
            witdth = 20;
        }
        public void Print()
        {
            Console.WriteLine("length:{0}", length);
            Console.WriteLine("width:{0}", witdth);
            Console.WriteLine("area:{0}", Mianji());
        }
        public double Mianji()
        {
            return length * witdth;
        }
    }

    class Program
    {

        static void Main(String[] args)
        {
            /*Retangle r = new Retangle();
            r.Init();
            r.Print();*/
            Console.WriteLine("byte:{0}", sizeof(byte));
            Console.WriteLine("byte:{0}", sizeof(int));
            Console.WriteLine("byte:{0}", sizeof(double));
        }
    }
}



