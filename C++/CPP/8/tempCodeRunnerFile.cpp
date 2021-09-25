// #include <stdio.h>

// int main()
// {
//     /*  Write C code in this online editor and run it. */
//     int a = 0x12345678;
//     printf("%p\n", &a);
//     printf("%p\n", ((int)&a & 0xffffff));

//     return 0;
// }

#include <iostream>

using namespace std;

void swap2(int &a, int &b)
{
    cout << a << " " << b << endl;
    cout << &a << " " << &b << endl;
    int t = a;
    a = b;
    b = t;
}
int main()
{
    int a = 1, b = 2;
    cout << &a << " " << &b << endl;
    swap2(a, b);
    cout << a << " " << b << endl;
    cout << &a << " " << &b << endl;
    return 0;
}