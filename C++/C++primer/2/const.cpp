#include <iostream>
using namespace std;
int main()
{
    const int a = 10;
    // a = 122;
    // 上述语句会出错，，不能修改

    cout << a;

    return 0;
}