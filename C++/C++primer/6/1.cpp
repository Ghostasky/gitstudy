#include <iostream>
using namespace std;
void fun(int *a)
{
    *a = 20;
}
void fun(double a)
{
    printf("this is b\n");
}
int main()
{
    int a = 10;
    fun(&a);
    printf("%d\n", a);
    return 0;
}