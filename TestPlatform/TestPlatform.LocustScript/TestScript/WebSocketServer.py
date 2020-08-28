# -*- coding: utf-8 -*-

import time
import socket
import base64
import hashlib
import threading
import struct
import copy

buffsize = 2048
packagestart = "<package>"
packageend = "</package>"
isonetoone = False

global users
users = set()


def send_msg(sock, msg_bytes):
    """
    WebSocket服务端向客户端发送消息
    :param sock: 客户端连接到服务器端的socket对象,即： sock,address = socket.accept()
    :param msg_bytes: 向客户端发送的字节
    :return:
    """
    token = b"\x81"  # 接收的第一字节，一般都是x81不变
    length = len(msg_bytes)

    if length < 126:
        token += struct.pack("B", length)
    elif length <= 0xFFFF:
        token += struct.pack("!BH", 126, length)
    else:
        token += struct.pack("!BQ", 127, length)

    msg = token + msg_bytes

    # 如果出错就是客户端断开连接
    try:
        sock.send(msg)
    except Exception as e:
        print(e)
        # 删除断开连接的记录
        users.remove(sock)


def link(sock, addr):
    package = ""

    # if isonetoone:
    #     package = "Welcome!"
    # else:
    #     package = "%sWelcome!%s" % (packagestart, packageend)

    # sock.send((package).encode())

    while True:
        try:
            # data = sock.recv(buffsize).decode()
            data = sock.recv(buffsize)
            # print(data)

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

                # package = "%sOK!%s" % (packagestart, packageend)
                # print(package)
                # send_msg(sock, bytes(package, encoding="utf-8"))
                sock.send(data)
            except Exception as e:
                print(str(e))
                print("Error Package: %s" % data)
        except Exception as e:
            print(str(e))
            break

    sock.close()
    print("Connection from %s:%s closed." % addr)


def get_headers(data):
    '''将请求头转换为字典'''
    header_dict = {}

    data = str(data, encoding="utf-8")

    header, body = data.split("\r\n\r\n", 1)
    header_list = header.split("\r\n")
    print("---"*22, body)

    for i in range(0, len(header_list)):
        if i == 0:
            if len(header_list[0].split(" ")) == 3:
                header_dict['method'], header_dict['url'], header_dict['protocol'] = header_list[0].split(" ")
        else:
            k, v = header_list[i].split(":", 1)
            header_dict[k] = v.strip()

    return header_dict


def accept(host, port):
    ADDR = (host, port)
    conn = socket.socket()
    conn.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
    conn.bind(ADDR)
    conn.listen(5)

    print("Wait for connection ...")

    try:
        while True:
            sock, addr = conn.accept()
            print("Accept new connection from %s:%s..." % addr)

            users.add(sock)
            # 获取握手消息，magic string ,sha1加密
            # 发送给客户端
            # 握手消息
            data = sock.recv(8096)
            headers = get_headers(data)
            # 对请求头中的sec-websocket-key进行加密
            response_tpl = "HTTP/1.1 101 Switching Protocols\r\n" \
                    "Upgrade:websocket\r\n" \
                    "Connection: Upgrade\r\n" \
                    "Sec-WebSocket-Accept: %s\r\n" \
                    "WebSocket-Location: ws://%s%s\r\n\r\n"

            magic_string = '258EAFA5-E914-47DA-95CA-C5AB0DC85B11'
            value = headers['Sec-WebSocket-Key'] + magic_string
            ac = base64.b64encode(hashlib.sha1(value.encode('utf-8')).digest())
            response_str = response_tpl % (ac.decode('utf-8'), headers['Host'], headers['url'])
            # 响应【握手】信息
            sock.send(bytes(response_str, encoding='utf-8'),)

            t = threading.Thread(target=link, args=(sock, addr))
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
# python "E:\Documents\Visual Studio Code\TestPython\locust-tcp\locustfiles\WebSocketServer.py"

# python "/home/TPUser/WebSocketServer.py"
