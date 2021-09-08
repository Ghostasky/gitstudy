// 还有点问题，之后再改
#include <iostream>
using namespace std;

class Str
{

private:
    int a;

public:
    Str()
    {
        this->a = 10;
    }
    Str operator+(Str a);
};
Str Str::operator+(Str a)
{
    return a;
}
int main()
{
    Str a;
    Str b;
    Str c;
    c = a + b;
    cout < < < < endl;
    return 0;
}