# -*- coding:utf-8 –*-

import socket
from pprint import pprint
# a = socket.getaddrinfo('baidu.com', 'www')

print(socket.gethostname())  # 得到主机名
print(socket.gethostbyname('www.baidu.com'))  # 主机名与ip交换
print(socket.gethostbyaddr('36.152.44.95'))
print(socket.getprotobyname('UDP'))
print(socket.getservbyname('www'))
print(socket.getservbyport(80))
print(socket.gethostbyname(socket.getfqdn()))
