[TOC]



# 0x1.php弱类型比较

## 0x1.1. “\==”与“\===”比较漏洞/switch

"\=="是判断值是否相等，“\===”是判断数值和类型是否相等，不仅如此，“\==”还将不同类型的值转化为相同的再进行比较：

```php
<?php  
var_dump("admin" ==0);//bool(true)

var_dump("1admin" ==1);//bool(true)

var_dump("2admin" ==2);//bool(true)

var_dump("admin1" ==1);//bool(false) 

var_dump("admin1" ==0);//bool(true)

var_dump("0e123456" =="0e4456789");//bool(true)
?>
```

如果字符串没有包含'.'，'e'，'E'，并且在整形范围之内，则会被当做int，否则会float：

```php
<?php  
var_dump(1+"admin1");//int(1) 
var_dump(1+"1admin");//int(2) 
var_dump(1+"2e2");//float(201)
var_dump(1+"-2e2");//float(-99)
var_dump(1+"hh-2e2");//int(1) 
var_dump(1+"1hh-2e2");//int(2) 
?>
```

switch同理。

## 0x1.2. bool类型的true比较

bool类型的true可以和任何字符串弱类型相等。

```php
<?php  
if(true == "GETF"){
	echo "OK";
}
?>
```



## 0x1.3. strcmp()比较漏洞

version>5.3:

```
Note a difference between 5.2 and 5.3 versions  
  
echo (int)strcmp('pending',array());  
will output -1 in PHP 5.2.16 (probably in all versions prior 5.3)  
but will output 0 in PHP 5.3.3  
  
Of course, you never need to use array as a parameter in string comparisions.
```

输入数组的话会返回0，也就是弱相等。

## 0x1.4. sha1加密比较

```php
$_GET['name'] == $_GET['password']
&&
sha1($_GET['name']) === sha1($_GET['password'])
```

这里为什么可以数组绕过呢？因为sha1要求参数不能为数组，传入数组的话返回都为false，从而两边都相等，

## 0x1.5. MD5加密比较

### 类型1

```php
$_GET['name'] != $_GET['password']
&&
MD5($_GET['name']) == MD5($_GET['password'])
```

PHP在处理哈希字符串时，它把每一个以“0E”开头的哈希值都解释为0，所以如果两个不同的密码经过哈希以后，其哈希值都是以“0E”开头的，那么PHP将会认为他们相同，都是0。

**以下值在md5加密后以0E开头：**

-   QNKCDZO
-   240610708
-   s878926199a
-   s155964671a
-   s214587387a
-   s214587387a

**以下值在sha1加密后以0E开头：**

-   sha1(‘aaroZmOk’)
-   sha1(‘aaK1STfY’)
-   sha1(‘aaO8zKZF’)
-   sha1(‘aa3OFF9m’)

运用了\==的原理：“0e1345”\=="45678"

### 类型2

```php
$_POST['param1']!==$_POST['param2'] 
&& 
md5($_POST['param1'])===md5($_POST['param2']))
```

使用了强等，数组绕过。

### 类型3

```
(string)$_POST['param1']!==(string)$_POST['param2'] 
&& 
md5($_POST['param1'])===md5($_POST['param2']))
```

```
Param1=%4d%c9%68%ff%0e%e3%5c%20%95%72%d4%77%7b%72%15%87%d3%6f%a7%b2%1b%dc%56%b7%4a%3d%c0%78%3e%7b%95%18%af%bf%a2%00%a8%28%4b%f3%6e%8e%4b%55%b3%5f%42%75%93%d8%49%67%6d%a0%d1%55%5d%83%60%fb%5f%07%fe%a2
Param2=%4d%c9%68%ff%0e%e3%5c%20%95%72%d4%77%7b%72%15%87%d3%6f%a7%b2%1b%dc%56%b7%4a%3d%c0%78%3e%7b%95%18%af%bf%a2%02%a8%28%4b%f3%6e%8e%4b%55%b3%5f%42%75%93%d8%49%67%6d%a0%d1%d5%5d%83%60%fb%5f%07%fe%a2
```

```
a=%4d%c9%68%ff%0e%e3%5c%20%95%72%d4%77%7b%72%15%87%d3%6f%a7%b2%1b%dc%56%b7%4a%3d%c0%78%3e%7b%95%18%af%bf%a2%00%a8%28%4b%f3%6e%8e%4b%55%b3%5f%42%75%93%d8%49%67%6d%a0%d1%55%5d%83%60%fb%5f%07%fe%a2
&b=%4d%c9%68%ff%0e%e3%5c%20%95%72%d4%77%7b%72%15%87%d3%6f%a7%b2%1b%dc%56%b7%4a%3d%c0%78%3e%7b%95%18%af%bf%a2%02%a8%28%4b%f3%6e%8e%4b%55%b3%5f%42%75%93%d8%49%67%6d%a0%d1%d5%5d%83%60%fb%5f%07%fe%a2
```



# 0x2.escapeshellarg与escapeshellcmd共用漏洞

## 0x2.1. escapeshellarg()

escapeshellarg会将传入的字符用单引号围起来，能使传入的字符当成字符，而不是shell命令了。

比如:

```php
没有用该函数：
<?php
$a=$_GET[a];
eval("echo $a".';');
//a = 11;system(ls)，则会导致命令执行

使用该函数：
<?php
$a=$_GET[a];
$a = escapeshellarg($a)
eval("echo $a".';');
//则不会导致命令执行
```

## 0x2.2escapeshellcmd()

同样防止任意shell命令执行，过滤&;之类的管道符转义掉。

'和"仅在不配对的时候被转义。

## 0x2.3漏洞的出现

单独使用两个其中个任意一个都不会出现问题。

或者先试用escapeshellcmd，在使用escapeshellarg也不会出现问题。

唯有先试用escapeshellarg再使用escapeshellcmd会出现漏洞。

例题：**[BUUCTF 2018]Online Tool**
