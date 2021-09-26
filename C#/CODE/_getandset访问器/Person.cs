using System;
using System.Collections.Generic;
using System.Text;

namespace _11getandset访问器
{
    class Person
    {
        string name;
        public int age;

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }
        public int Age
        {
            get
            {
                return age;
            }
        }

    }
}
