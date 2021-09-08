#include <iostream>
using namespace std;

class Test
{
public:
    int a = 1, b = 1, c = 1;
    Test(int a, int b, int c)
    {
        this->a = a, this->b = b, this->c = c;
    }
    Test()
    {
        this->a = 30, this->b = 30, this->c = 30;
    }
    void Print(int q, int w, int e);
    Test aaa(int q, int w, int e);
};
Test Test::aaa(int q, int w, int e)
{
    return *this;
}
void Test::Print(int a, int b, int c)
{
    printf("%d %d %d\n", this->a, this->b, this->c);
    printf("%d %d %d\n", a, b, c);
}
int main()
{
    Test b(2, 2, 2);
    b.aaa(1, 2, 3);
    // Test c;
    // // b.a = 20;

    // b.Print(3, 3, 3);
    // c.Print(4, 4, 4);
    return 0;
}