[TOC]

SROP(Sigreturn Oriented Programming) 

其中Sigreturn 是一个系统调用。在类Unix系统发生signal的时候会被间接地调用

# signal机制

signal 机制是类 unix 系统中进程之间相互传递信息的一种方法。一般，我们也称其为软中断信号，或者软中断。比如说，进程之间可以通过系统调用 kill 来发送软中断信号。一般来说，信号机制常见的步骤如下图所示：

![https://ctf-wiki.org/pwn/linux/user-mode/stackoverflow/x86/advanced-rop/figure/ProcessOfSignalHandlering.png](https://ctf-wiki.org/pwn/linux/user-mode/stackoverflow/x86/advanced-rop/figure/ProcessOfSignalHandlering.png)

1.  内核向某个进程发送 signal 机制，该进程会被暂时挂起，进入内核态。
2.  内核会为该进程保存相应的上下文，**主要是将所有寄存器压入栈中，以及压入 signal 信息，以及指向 sigreturn 的系统调用地址**。此时栈的结构如下图所示，我们称 ucontext 以及 siginfo 这一段为 Signal Frame。**需要注意的是，这一部分是在用户进程的地址空间的。**之后会跳转到注册过的 signal handler 中处理相应的 signal。因此，当 signal handler 执行完之后，就会执行 sigreturn 代码。
3.  signal handler 返回后，内核为执行 sigreturn 系统调用，为该进程恢复之前保存的上下文，其中包括将所有压入的寄存器，重新 pop 回对应的寄存器，最后恢复进程的执行。其中，32 位的 sigreturn 的调用号为 77，64 位的系统调用号为 15。

![](https://ctf-wiki.org/pwn/linux/user-mode/stackoverflow/x86/advanced-rop/figure/signal2-stack.png)

# 攻击原理

仔细回顾一下内核在 signal 信号处理的过程中的工作，我们可以发现，内核主要做的工作就是为进程保存上下文，并且恢复上下文。这个主要的变动都在 Signal Frame 中。但是需要注意的是：

-   Signal Frame 被保存在用户的地址空间中，所以用户是可以读写的。
-   由于内核与信号处理程序无关 (kernel agnostic about signal handlers)，它并不会去记录这个 signal 对应的 Signal Frame，所以当执行 sigreturn 系统调用时，此时的 Signal Frame 并不一定是之前内核为用户进程保存的 Signal Frame。

### 获取 shell

首先，我们假设攻击者可以控制用户进程的栈，那么它就可以伪造一个 Signal Frame，如下图所示，这里以 64 位为例子，给出 Signal Frame 更加详细的信息

![](https://ctf-wiki.org/pwn/linux/user-mode/stackoverflow/x86/advanced-rop/figure/srop-example-1.png)

当系统执行完 sigreturn 系统调用之后，会执行一系列的 pop 指令以便于恢复相应寄存器的值，当执行到 rip 时，就会将程序执行流指向 syscall 地址，根据相应寄存器的值，此时，便会得到一个 shell。

### system call chains

需要指出的是，上面的例子中，我们只是单独的获得一个 shell。有时候，我们可能会希望执行一系列的函数。我们只需要做两处修改即可

-   **控制栈指针。**
-   **把原来 rip 指向的`syscall` gadget 换成`syscall; ret` gadget。**

如下图所示 ，这样当每次 syscall 返回的时候，栈指针都会指向下一个 Signal Frame。因此就可以执行一系列的 sigreturn 函数调用。

![](https://ctf-wiki.org/pwn/linux/user-mode/stackoverflow/x86/advanced-rop/figure/srop-example-2.png)

需要注意的是，我们在构造 ROP 攻击的时候，需要满足下面的条件

-   **可以通过栈溢出来控制栈的内容**
-   需要知道相应的地址
    -   **"/bin/sh"**
    -   **Signal Frame**
    -   **syscall**
    -   **sigreturn**
-   需要有够大的空间来塞下整个 sigal frame

此外，关于 sigreturn 以及 syscall;ret 这两个 gadget 在上面并没有提及。提出该攻击的论文作者发现了这些 gadgets 出现的某些地址：

