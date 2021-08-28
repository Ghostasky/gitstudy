import argparse
import socket

MAX_BYTES = 65535
parse = argparse.ArgumentParser()
parse.add_argument("-c", "--client", help="client", action="store_true")
parse.add_argument("-s", "--serve", help="serve", action="store_true")
parse.add_argument("-p", "--port", help="port", type=int)
parse.add_argument("-m", "--message", help="something you want send")
args = parse.parse_args()

if args.client:
    sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    sock.connect(("localhost", args.port))
    print("Client : "+str(sock.getsockname()))
    sock.send(args.message)
    data = sock.recv(MAX_BYTES)
    print(data)
    sock.close()
    print("sock closed!")

if args.serve:
    sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    sock.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
    sock.bind(("localhost", args.port))
    sock.listen(0)
    print("Serve listening at :"+str(sock.getsockname()))
    while True:
        sc, sockname = sock.accept()
        print("we accept from " + str(sockname))
        print("  Socket name:" + str(sc.getsockname()))
        print("  Socket peer:" + str(sc.getpeername()))
        message = sc.recv(MAX_BYTES)
        print("  recved message is :" + message)
        text = "server recved!"
        sc.send(text)
        sc.close()
        print('  socket closed')
