# !/usr/bin/env python3
# -*- coding:utf-8 -*-

import time
import threading
import json
from socket import socket, AF_INET, SOCK_STREAM

buffsize = 2048
packagestart = "<package>"
packageend = "</package>"
isonetoone = False


def tcplink(sock, addr):
    package = ""

    # if isonetoone:
    #     package = "Welcome!"
    # else:
    #     package = "%sWelcome!%s" % (packagestart, packageend)

    # sock.send((package).encode())

    while True:
        try:
            data = sock.recv(buffsize).decode()

            if data == "exit" or not data:
                # print("Not Data: %s" % data)
                break

            try:
                # packagejson = {
                #     "data": json.loads(data),
                #     "clientaddr": "%s:%s" % (addr[0], addr[1])
                # }
                # packagejson = {
                #     "data": data,
                #     "clientaddr": "%s:%s" % (addr[0], addr[1])
                # }
                # package = ""

                # if isonetoone:
                #     package = "%s" % json.dumps(packagejson)
                # else:
                #     package = "%s%s%s" % (
                #         packagestart, json.dumps(packagejson), packageend)

                # sock.send(package.encode())

                package = "%sOK!%s" % (packagestart, packageend)
                sock.send(package.encode())                
            except Exception as e:
                print(str(e))
                print("Error Package: %s" % data)
        except Exception as e:
            print(str(e))
            break

    sock.close()
    print("Connection from %s:%s closed." % addr)


def accept(host, port):
    ADDR = (host, port)
    tctime = socket(AF_INET, SOCK_STREAM)
    tctime.bind(ADDR)
    tctime.listen(3)

    print("Wait for connection ...")

    try:
        while True:
            sock, addr = tctime.accept()
            print("Accept new connection from %s:%s..." % addr)

            t = threading.Thread(target=tcplink, args=(sock, addr))
            t.setDaemon(True)
            t.start()
    except KeyboardInterrupt:
        print("accept")
        pass


def main():
    host = ""
    port = 12345

    t = threading.Thread(target=accept, args=(host, port))
    t.setDaemon(True)
    t.start()

    try:
        while True:
            time.sleep(1)
    except KeyboardInterrupt:
        pass


if __name__ == "__main__":
    main()
# python "E:\Documents\Visual Studio Code\TestPython\locust-tcp\locustfiles\TcpSocketServer.py"

# python "/home/TPUser/TcpSocketServer.py"
