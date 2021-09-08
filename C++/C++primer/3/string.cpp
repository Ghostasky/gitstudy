#include <string>
#include <iostream>

using namespace std;

int main()
{

    string a = "123212313aa";
    string s1 = a;
    string s2(a);
    string s3("qwer");
    string s4(10, 'c');
    string s5;
    cout << a << endl;
    getline(cin, s5);
    cout << s5 << endl;
    for (auto b : s5)
    {
        cout << b << endl;
    }
    return 0;
}

