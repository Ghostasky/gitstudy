#include <stdio.h>

int main()
{
    /*  Write C code in this online editor and run it. */
    int a = 0x12345678;
    printf("%p\n", &a);
    printf("%p\n", ((int)&a & 0xffffff));

    return 0;
}