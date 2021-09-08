#include <vector>
#include <iostream>
using namespace std;

int main()
{
    // 初始化方法：
    vector<int> a;
    vector<int> v2(a);
    vector<int> v3 = a;
    vector<int> v4(3, 5);
    cout << v4.size() << endl;
    vector<int> v5{1, 2, 3, 4, 5, 6};
    vector<int> v6 = {1, 2, 3, 4, 5, 6}; //与上一条等价

    // push_back(a)方法将a压倒最后
    v5.push_back(7);
    cout << v5[6] << endl;
    cout << "----------------------------" << endl;

    vector<int> test{1, 2, 3, 4, 5, 6, 7, 8};
    auto b = test.begin(), c = test.end();
    cout << b[0] << c[-1] << endl;
    // cout < < < < endl;
    return 0;
}