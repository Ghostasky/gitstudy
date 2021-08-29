
import socket
import argparse

MAX_BYTES = 65535

parse = argparse.ArgumentParser()
parse.add_argument("-c", "--client", help="client", action="store_true")
parse.add_argument("-s", "--serve", help="serve", action="store_true")
parse.add_argument("-p", "--port", help="port", type=int)
parse.add_argument("-m", "--message", help="something you want send")
args = parse.parse_args()
