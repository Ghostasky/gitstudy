# 第一章

先来个argparse模块

首先

```python
import argparse
parse = argparse.ArgumentParser()
parse.parse_args()
```

## 位置参数

```python
import argparse

parse = argparse.ArgumentParser()
parse.add_argument("qwe")
args = parse.parse_args()
print(args.qwe)
```

 `add_argument()` 方法，该方法用于指定程序能够接受哪些命令行选项。

稍微改的有用点：

```python
parse.add_argument("qwe", help="qwe can echo string you input")
```

```shell
usage: aa.py [-h] qwe

positional arguments:
  qwe         qwe can echo string you input

optional arguments:
  -h, --help  show this help message and exit
```

还可以：

```python
parse.add_argument("qwe", help="qwe can echo string you input", type=int)
args = parse.parse_args()
print(args.qwe ** 2)
```

输出为参数的平方

## 可选参数

```python
parse.add_argument("--verbosity", help="increase output verbosity")
args = parse.parse_args()
if args.verbosity:
    print("verbosity turn on")
```

这样写--verbosity参数时后面必须跟一个参数，如果不想跟的话可以加一个`action = "store_true"`

### 短选项

`parse.add_argument( "-v", "--verbosity", help="increase output verbosity", action="store_true")`

# 第二章

可以使用getsockname()来获取包含ip和port的二元组

recvfrom(max_byte)：获取内容，返回两个值，data和address，通常使用在udp中

recv(max_byte)：获取内容，返回一个值，data，通常使用在tcp中

sendto(data,(ip,port))：send内容

客户端还可以使用connect：connect((ip.port))



# 第三章

server端的异同

tcp server端：

-   sock
-   bind
-   listen
-   accept
-   recv
-   close

udp server端

-   sock
-   bind
-   recvfrom

















