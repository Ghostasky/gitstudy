# -*- coding:utf-8 –*-
import socket
import dns.resolver
import argparse

parse = argparse.ArgumentParser()
parse.add_argument('-u', '--url', help="url you want search")

args = parse.parse_args()

if args.url:
    a = ['A', 'AAAA', 'CNAME', 'MX', 'NS']
    for i in a:
        answer = dns.resolver.query(args.url, i, raise_on_no_answer=False)
        # if answer.rrset is not None:
        print(answer.rrset)
