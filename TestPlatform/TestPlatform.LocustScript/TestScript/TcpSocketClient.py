# !/usr/bin/env python3
# -*- coding:utf-8 -*-

import time
import datetime
import socket
import random
import threading
import json
import pandas
import gevent
from gevent._semaphore import Semaphore
from locust import HttpLocust, Locust, HttpUser, User, TaskSet
from locust import events, task, between
from locust.event import EventHook
from locust.env import Environment
from locust.stats import stats_printer
from locust.log import setup_logging
from Crypto.Cipher import DES3
import base64
import requests
from urllib import request, parse

import sys
import os
sys.path.append(os.path.dirname(os.path.dirname(__file__)))
# from config import config
# from case import case
# from api import api
# from common import common

setup_logging("INFO", None)

# host = config.host
# port = config.port
# package_start = config.package_start
# package_end = config.package_end
# client_id = config.client_id
# case_id = config.case_id
# min_wait = config.min_wait
# max_wait = config.max_wait
# datetime_format = config.datetime_format
# buff_size = config.buff_size
# is_one_to_one = config.is_one_to_one
# is_hatch_complete_run = config.is_hatch_complete_run
# is_recv_error_close = config.is_recv_error_close
# is_worker_report_save = config.is_worker_report_save
# is_security_data = config.is_security_data
# key = config.key


host = "127.0.0.1"
# host = "{Address}"
port = 12345
# port = {Port}
# client_id
client_id = "client_id"
# client_id = "{SlaveName}"
# case_id
case_id = "ce514456-8da9-432f-8999-1010fa94a83a"
# case_id = "{CaseID}"
# # 数据包头
package_start = "<package>"
# # 数据包尾
package_end = "</package>"
# package_end = "{ResponseSeparator}"
# API地址
case_service_base_address = "http://52.188.14.158:8082/"
# case_service_base_address = "{CaseServiceBaseAddress}"

# 每个用户每次Task之间的等待时间，单位：秒
min_wait = 0.001
max_wait = 0.001
# 时间格式
datetime_format = "%Y-%m-%d %H:%M:%S.%f"
# revc数据大小
buff_size = 2048
# 是否1次发送对应1次接收
is_one_to_one = False
# 是否等待所有用户加载完，执行Task
is_hatch_complete_run = True
# recv报错时，是否要关闭socket
is_recv_error_close = False
# 是否在worker_report时保存数据
is_worker_report_save = True
# 是否Current数据
is_current = True
# 保存数据的时间间隔（秒）
is_save_interval = 10
# 发送的数据包是否加密
is_security_data = True
# 秘钥，密钥的长度必须是16的倍数
key = "tjjtgjzs########"

all_locusts_spawned = Semaphore()
lock = threading.Lock()


# {$additionfunc(0)}
# 按脚本名称随机获取该名称下的行数据，每次取完后，该行就不能再被获取
# 参数：
#   1，json键值对
# 返回值：
#   键值对
def NameOnceJsonData(data):
    # print("NameOnceJsonData")
    import random

    if data is not None and len(data) > 0:
        index = random.randint(0, len(data) - 1)
        row = data[index]
        del data[index]

        return row

    return None


# 执行Tcp的请求发送与响应处理
# 参数：
#   1，服务端地址
#   2，端口
#   3，请求内容
#   4，响应内容截取匹配（正则表达式）
# 返回值：
#   响应内容截取的数据
def TcpRR(address, port, senddata, receivereg):
    # print("TcpRR")
    import socket
    import re

    if address is None or address == "" or port is None or port == "":
        return None

    host = address
    port = port
    ADDR = (host, port)
    bufsize = 2048

    connect = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

    try:
        connect.connect(ADDR)
        connect.send(senddata.encode())

        data = connect.recv(bufsize).decode()

        p = re.compile(receivereg, re.S)
        result = re.findall(p, data)

        if len(result) > 0:
            return result[0]
        else:
            return ""
    except Exception as e:
        print(str(e))
        return None
    finally:
        connect.close()

    return ""


# 根据传入的Tcp连接执行Tcp的请求发送与响应处理
# 参数：
#   1，Tcp连接
#   2，请求内容
#   3，响应内容截取匹配（正则表达式）
# 返回值：
#   响应内容截取的数据
def TcpRRWithConnect(connect, senddata, receivereg):
    # print("TcpRRWithConnect")
    import socket
    import re

    bufsize = 2048

    try:
        connect.send(senddata)

        data = connect.recv(bufsize)

        p = re.compile(receivereg, re.S)
        result = re.findall(p, data)

        if len(result) > 0:
            return result[0]
        else:
            return ""
    except Exception as e:
        print(str(e))
        return None

    return ""


# 随机获取json行数据列表中的一行数据
# 参数：
#   1，json列表
# 返回值：
#   键值对
def GetJsonRowData(data):
    # print("GetJsonRowData")
    import random

    if data is not None and len(data) > 0:
        index = random.randint(0, len(data) - 1)
        row = data[index]

        return row

    return None


# 获取指定名称和类别下的序列号
# 参数：
#   1，名称
#   2，类别
#   3，初始序号
# 返回值：
#   键值对
serial_no_s = [
    {
        "Name": "name",
        "Type": "type",
        "SerialNo": 0
    }
]


def GetNameSerialNo(name, type, start):
    # print("GetJsonRowData")

    for serial_no in serial_no_s:
        if serial_no["Name"] == name and serial_no["Type"] == type:
            serial_no["SerialNo"] = serial_no["SerialNo"] + 1
            return serial_no["SerialNo"]

    serial_no = {
        "Name": name,
        "Type": type,
        "SerialNo": start + 1
    }

    serial_no_s.append(serial_no)
    return serial_no["SerialNo"]


# 数字补0
# 参数：
#   1，数字
#   2，补齐方向，0，左对齐，1，右对齐(int没右补零)
#   3，总长度
# 返回值：
#   补完后的字符串
def NumberFill(number, direct, length):
    # print("NumberFill")
    str_number = str(number)
    result = ""

    if direct == 0:
        result = str_number.zfill(length)
    elif direct == 1:
        if("." in str_number):
            result = str_number + "0" * (length - len(str_number))
        else:
            result = str_number
    else:
        result = str_number

    return result


# 整数范围随机
# 参数：
#   1，最小数
#   2，最大数
# 返回值：
#   范围内的随机整数
def IntRange(min, max):
    # print("IntRange")
    import random

    if min <= max:
        ran = random.randint(min, max)

        return ran
    else:
        return min


# 十进制数范围随机
# 参数：
#   1，最小数
#   2，最大数
# 返回值：
#   范围内的随机十进制数
def DecimalRange(min, max):
    # print("DecimalRange")
    import random

    if min <= max:
        ran = random.uniform(min, max)
        str_min = str(min)
        str_max = str(max)
        dot = "."

        if(dot in str_min):
            index = str_min.index(dot)
            length = len(str_min) - index
            ran = float(round(ran, length))
        elif(dot in str_max):
            index = str_max.index(dot)
            length = len(str_max) - index
            ran = float(round(ran, length))

        return ran
    else:
        return min


# 随机从Json数据中获取指定属性的值
# 参数：
#   1，Json字符串
#   2，属性名称
# 返回值：
#   找到的属性的值
def RanJsonData(data, name):
    # print("RanJsonData")
    import random

    if data is not None and type(data).__name__ == "dict":
        return data[name]
    elif len(data) > 0:
        index = random.randint(0, len(data) - 1)
        row = data[index]

        return row[name]

    return None


# DES加密
# 参数：
#   1，要加密的数据
#   2，密钥
# 返回值：
#   加密后的数据
def DesSecurity(data, key):
    # print("DesSecurity")
    from Crypto.Cipher import DES3
    import base64

    if len(key) % 16 > 0:
        return ""

    key = key
    mode = DES3.MODE_CBC
    iv = b"12345678"
    length = DES3.block_size

    text = str(data)
    text = text + (length - len(text) % length) * chr(length - len(text) % length)
    cryptor = DES3.new(key, mode, iv)
    # self.iv 为 IV 即偏移量
    x = len(text) % 8

    if x != 0:
        text = text + "\0" * (8 - x)  # 不满16，32，64位补0

    ciphertext = cryptor.encrypt(text)
    result = base64.standard_b64encode(ciphertext).decode("utf-8")

    return result


# {$datavardeclareinit(0)}


class EncryptDate():
    def __init__(self, key):
        self.key = key
        self.mode = DES3.MODE_CBC
        self.iv = b"12345678"
        self.length = DES3.block_size

    def pad(self, s):
        return s + (self.length - len(s) % self.length) * chr(self.length - len(s) % self.length)

    # 定义 padding 即 填充 为PKCS7
    def unpad(self, s):
        return s[0:-ord(s[-1])]

    # DES3的加密模式为CBC
    def encrypt(self, text):
        text = self.pad(text)
        cryptor = DES3.new(self.key, self.mode, self.iv)
        # self.iv 为 IV 即偏移量
        x = len(text) % 8

        if x != 0:
            text = text + "\0" * (8 - x)  # 不满16，32，64位补0

        self.ciphertext = cryptor.encrypt(text)
        return base64.standard_b64encode(self.ciphertext).decode("utf-8")

    def decrypt(self, text):
        cryptor = DES3.new(self.key, self.mode, self.iv)
        de_text = base64.standard_b64decode(text)
        plain_text = cryptor.decrypt(de_text)
        st = str(plain_text.decode("utf-8")).rstrip("\0")
        out = self.unpad(st)
        return out
        # 上面注释内容解密如果运行报错，就注释掉试试
        # return plain_text


ed = EncryptDate(key)


class TcpSocketClient(socket.socket):
    _locust_environment = None

    def __init__(self, af_inet, socket_type):
        super(TcpSocketClient, self).__init__(af_inet, socket_type)

    def connect(self, addr):
        start_time = time.time()

        try:
            super(TcpSocketClient, self).connect(addr)
        except Exception as e:
            total_time = int((time.time() - start_time) * 1000)
            self._locust_environment.events.request_failure.fire(
                request_type="tcpsocket", name="connect",
                response_time=total_time, response_length=len(str(e)),
                exception=e)

            return False
        else:
            total_time = int((time.time() - start_time) * 1000)
            self._locust_environment.events.request_success.fire(
                request_type="tcpsocket", name="connect",
                response_time=total_time, response_length=0)

            return True

    def send(self, msg):
        start_time = time.time()

        try:
            super(TcpSocketClient, self).send(msg.encode())
        except Exception as e:
            total_time = int((time.time() - start_time) * 1000)
            self._locust_environment.events.request_failure.fire(
                request_type="tcpsocket", name="send",
                response_time=total_time, response_length=len(str(e)),
                exception=e)

            return False
        else:
            total_time = int((time.time() - start_time) * 1000)
            self._locust_environment.events.request_success.fire(
                request_type="tcpsocket", name="send",
                response_time=total_time, response_length=0)

            return True

    def recv(self, bufsize):
        recv_data = ""
        start_time = time.time()

        try:
            recv_data = super(TcpSocketClient, self).recv(bufsize).decode()
        except Exception as e:
            total_time = int((time.time() - start_time) * 1000)
            self._locust_environment.events.request_failure.fire(
                request_type="tcpsocket", name="recv",
                response_time=total_time, response_length=len(str(e)),
                exception=e)
        else:
            total_time = int((time.time() - start_time) * 1000)
            self._locust_environment.events.request_success.fire(
                request_type="tcpsocket", name="recv",
                response_time=total_time, response_length=0)

        return recv_data


class TcpTestUser(User):
    # min_wait = 100
    # max_wait = 1000
    wait_time = between(min_wait, max_wait)
    # 连接的TCP服务的IP
    host = host
    # 连接的TCP服务的端口
    port = port
    ADDR = (host, port)
    user_id = ""
    user_password = ""
    user_token = ""
    request_body = ""
    senddata = ""
    worker_report_time = datetime.datetime.now()

    # 自定义变量集合
    currconnectkv = {
        "name": ""
    }

    stats_tcps = {
        "stats_tcps": []
    }

    stats_tcps_currents = {
        "stats_tcps_currents": []
    }

    stats_tcps_totals = {
        "stats_tcps_totals": []
    }

    def __init__(self, *args, **kwargs):
        super(TcpTestUser, self).__init__(*args, **kwargs)
        self.client = TcpSocketClient(socket.AF_INET, socket.SOCK_STREAM)
        self.client._locust_environment = self.environment

    def get_package(self):
        # data = str(random.randint(1, 10000))
        # send_time = datetime.datetime.now().strftime(datetime_format)
        # package_json = {
        #     "data": data,
        #     "send_time": send_time
        # }
        # package = json.dumps(package_json)

        # if is_security_data:
        #     package = ed.encrypt(package)

        self.request_body = {"UserName": self.user_id, "UserToken": self.user_token, "a": "a"}
        # self.request_body = {$senddata()}
        self.senddata = self.request_body
        package = self.senddata
        package = json.dumps(package)

        # {$sendinit(8)}
        package = DesSecurity(package, 'abcdefghjhijklmn')

        return package

    def connect(self):
        # while self.client.connect(self.ADDR) is False:
        #     print("Client: connect fail, %s:%s" % self.ADDR)
        #     time.sleep(1)

        # print("Client: connect success, %s:%s" % self.ADDR)

        is_success = self.client.connect(self.ADDR)

        if is_success is False:
            print("Client: connect fail, %s:%s" % self.ADDR)
            return is_success

        print("Client: connect success, %s:%s" % self.ADDR)
        return is_success

    def login(self):
        # Login
        # {$connectinit(8)}
        json_user_account = {"UserName": "zhangsan", "Password": "123456"}
        self.user_id = "user_id_%s" % str(random.randint(1, 10000))
        self.user_password = "user_password_%s" % str(random.randint(1, 10000))
        login_send_data = {"UserName": self.user_id, "Password": self.user_password, "a": "a"}
        self.user_token = TcpRRWithConnect(self.client, json.dumps(login_send_data), ".*")
        self.user_id = self.user_id

        return True

    def send_data(self, package):
        # while self.client.send(package) is False:
        #     print("Client: send fail, %s" % package)
        #     time.sleep(1)

        # # print("Client: send success, %s" % package)

        is_success = self.client.send(package)

        if is_success is False:
            print("Client: send fail, %s" % package)
            return is_success

        # print("Client: send success, %s" % package)
        return is_success

    def recv_data(self, package):
        package[0] = ""
        recv_buff = ""

        while True:
            try:
                data = self.client.recv(buff_size)
                recv_buff += data

                if data == "exit" or not data:
                    # print("Not Data: %s" % data)
                    break

                # print("Data: %s" % data)

                package_recv = ""
                package_recv_data = ""
                is_package_complete = False

                if is_one_to_one or package_end == "":
                    package_recv = recv_buff
                    package_recv_data = package_recv
                    is_package_complete = True
                    print("onetoone")
                else:
                    if package_start in recv_buff and package_end in recv_buff:
                        ms = recv_buff.find(package_start)
                        me = recv_buff.find(package_end) + len(package_end)
                        package_recv = recv_buff[ms:me]
                        package_recv_data = package_recv.replace(
                            package_start, "").replace(package_end, "")
                        is_package_complete = True

                if is_package_complete:
                    try:
                        recv_time = datetime.datetime.now().strftime(
                            datetime_format)
                        # package_json = {
                        #     "data": json.loads(package_recv_data),
                        #     "recv_time": recv_time
                        # }
                        # package_json = {
                        #     "data": package_recv_data,
                        #     "recv_time": recv_time
                        # }
                        package_json = package_recv_data
                        package[0] = json.dumps(package_json)

                        # print("Package: %s" % package[0])
                    except Exception as e:
                        print(str(e))
                        print("Error Package: %s" % package_recv_data)

                    recv_buff = recv_buff.replace(package_recv, "")
                    # print("Recv_Buff: %s" % recv_buff)

                    # return package[0]
                    return True
            except Exception as e:
                print(str(e))
                break

        if is_recv_error_close:
            self.client.close()
            print("Connection closed.")

        return False

    def recv_data_thread(self):
        recv_buff = ""

        while True:
            try:
                data = self.client.recv(buff_size)
                recv_buff += data

                if data == "exit" or not data:
                    # print("Not Data: %s" % data)
                    break

                # print("Data: %s" % data)

                package_recv = ""
                package_recv_data = ""

                if is_one_to_one or package_end == "":
                    package_recv = recv_buff
                    package_recv_data = package_recv
                else:
                    if package_start in recv_buff and package_end in recv_buff:
                        ms = recv_buff.find(package_start)
                        me = recv_buff.find(package_end) + len(package_end)
                        package_recv = recv_buff[ms:me]
                        package_recv_data = package_recv.replace(
                            package_start, "").replace(package_end, "")

                try:
                    recv_time = datetime.datetime.now().strftime(
                        datetime_format)
                    # package_json = {
                    #     "data": json.loads(package_recv_data),
                    #     "recv_time": recv_time
                    # }
                    # package_json = {
                    #     "data": package_recv_data,
                    #     "recv_time": recv_time
                    # }
                    package_json = package_recv_data
                    package = json.dumps(package_json)

                    # OK
                    # print("Package: %s" % package)
                except Exception as e:
                    print(str(e))
                    print("Error Package: %s" % package_recv_data)

                recv_buff = recv_buff.replace(package_recv, "")
                # print("Recv_Buff: %s" % recv_buff)
            except Exception as e:
                print(str(e))
                break

        if is_recv_error_close:
            self.client.close()
            print("Connection closed.")

    def reset_data(self):
        # print("reset_data")

        # 如果是单点模式，取定时清理stats_tcps，以免爆内存
        if self.environment.parsed_options.worker is False:
            if TcpTestUser.is_worker_report_time() and len(TcpTestUser.stats_tcps["stats_tcps"]) > 0:
                lock.acquire()

                # temp_stats_tcps_currents = TcpTestUser.stats_tcps_currents["stats_tcps_currents"]
                TcpTestUser.create_stats_tcps_currents()
                # TcpTestUser.stats_tcps_currents["stats_tcps_currents"] = temp_stats_tcps_currents + TcpTestUser.stats_tcps_currents["stats_tcps_currents"]
                # TcpTestUser.create_stats_tcps_currents_worker_report()

                temp_stats_tcps_totals = TcpTestUser.stats_tcps_totals["stats_tcps_totals"]
                TcpTestUser.create_stats_tcps_totals()
                TcpTestUser.stats_tcps_totals["stats_tcps_totals"] = temp_stats_tcps_totals + TcpTestUser.stats_tcps_totals["stats_tcps_totals"]
                TcpTestUser.create_stats_tcps_totals_worker_report()

                if is_worker_report_save:
                    if len(TcpTestUser.stats_tcps["stats_tcps"]) > 0 and len(TcpTestUser.stats_tcps_totals["stats_tcps_totals"]) > 0:
                        # 在多节点分布式运行时，判断此为master, 汇总数据并提交API
                        TcpTestUser.save_data(TcpTestUser.stats_tcps, TcpTestUser.stats_tcps_currents, TcpTestUser.stats_tcps_totals)                

                        TcpTestUser.reset_stats_tcps()
                        # TcpTestUser.reset_stats_tcps_currents()
                        # TcpTestUser.reset_stats_tcps_totals()
                lock.release()

    def append_stats_tcps(self, user_id, client_id, case_id, name, start_time, end_time, run_time, is_success, message, error_message):
        stats_tcp = {}
        stats_tcp["user_id"] = user_id
        stats_tcp["client_id"] = client_id
        stats_tcp["case_id"] = case_id
        stats_tcp["name"] = name
        stats_tcp["start_time"] = start_time
        stats_tcp["end_time"] = end_time
        stats_tcp["run_time"] = run_time
        stats_tcp["is_success"] = is_success
        stats_tcp["message"] = message
        stats_tcp["error_message"] = error_message
        TcpTestUser.stats_tcps["stats_tcps"].append(stats_tcp)

    def append_stats_tcps_currents(client_id, case_id, name, start_time, end_time, run_time, max_time, min_time, avg_time, med_time, count_num, success_num, fail_num, count_s, success_s, fail_s, max_count_s, min_count_s, max_success_s, min_success_s, max_fail_s, min_fail_s):
        stats_tcps_current = {}
        stats_tcps_current["client_id"] = client_id
        stats_tcps_current["case_id"] = case_id
        stats_tcps_current["name"] = name
        stats_tcps_current["start_time"] = start_time
        stats_tcps_current["end_time"] = end_time
        stats_tcps_current["run_time"] = run_time
        stats_tcps_current["max_time"] = max_time
        stats_tcps_current["min_time"] = min_time
        stats_tcps_current["avg_time"] = avg_time
        stats_tcps_current["med_time"] = med_time
        stats_tcps_current["count_num"] = count_num
        stats_tcps_current["success_num"] = success_num
        stats_tcps_current["fail_num"] = fail_num
        stats_tcps_current["count_s"] = count_s
        stats_tcps_current["success_s"] = success_s
        stats_tcps_current["fail_s"] = fail_s
        stats_tcps_current["max_count_s"] = max_count_s
        stats_tcps_current["min_count_s"] = min_count_s
        stats_tcps_current["max_success_s"] = max_success_s
        stats_tcps_current["min_success_s"] = min_success_s
        stats_tcps_current["max_fail_s"] = max_fail_s
        stats_tcps_current["min_fail_s"] = min_fail_s
        TcpTestUser.stats_tcps_currents["stats_tcps_currents"].append(stats_tcps_current)

    def append_stats_tcps_totals(client_id, case_id, name, start_time, end_time, run_time, max_time, min_time, avg_time, med_time, count_num, success_num, fail_num, count_s, success_s, fail_s, max_count_s, min_count_s, max_success_s, min_success_s, max_fail_s, min_fail_s):
        stats_tcps_total = {}
        stats_tcps_total["client_id"] = client_id
        stats_tcps_total["case_id"] = case_id
        stats_tcps_total["name"] = name
        stats_tcps_total["start_time"] = start_time
        stats_tcps_total["end_time"] = end_time
        stats_tcps_total["run_time"] = run_time
        stats_tcps_total["max_time"] = max_time
        stats_tcps_total["min_time"] = min_time
        stats_tcps_total["avg_time"] = avg_time
        stats_tcps_total["med_time"] = med_time
        stats_tcps_total["count_num"] = count_num
        stats_tcps_total["success_num"] = success_num
        stats_tcps_total["fail_num"] = fail_num
        stats_tcps_total["count_s"] = count_s
        stats_tcps_total["success_s"] = success_s
        stats_tcps_total["fail_s"] = fail_s        
        stats_tcps_total["max_count_s"] = max_count_s
        stats_tcps_total["min_count_s"] = min_count_s
        stats_tcps_total["max_success_s"] = max_success_s
        stats_tcps_total["min_success_s"] = min_success_s
        stats_tcps_total["max_fail_s"] = max_fail_s
        stats_tcps_total["min_fail_s"] = min_fail_s

        TcpTestUser.stats_tcps_totals["stats_tcps_totals"].append(stats_tcps_total)

    def reset_stats_tcps():
        TcpTestUser.stats_tcps = {
            "stats_tcps": []
        }

    def reset_stats_tcps_currents():
        TcpTestUser.stats_tcps_currents = {
            "stats_tcps_currents": []
        }

    def reset_stats_tcps_totals():
        TcpTestUser.stats_tcps_totals = {
            "stats_tcps_totals": []
        }

    def is_worker_report_time():
        worker_report_time_last_seconds = (datetime.datetime.now() - TcpTestUser.worker_report_time).total_seconds()

        if worker_report_time_last_seconds <= is_save_interval:
            return False

        TcpTestUser.worker_report_time = datetime.datetime.now()

        return True

    def create_stats_tcps_currents():
        if len(TcpTestUser.stats_tcps["stats_tcps"]) == 0:
            return False

        TcpTestUser.reset_stats_tcps_currents()

        pandas_stats_tcps = pandas.read_json(json.dumps(TcpTestUser.stats_tcps["stats_tcps"]), "records")
        # print(pandas_stats_tcps)

        if len(pandas_stats_tcps) == 0:
            return False

        names = ["connect", "login", "send",  "recv"]

        for name in names:
            pandas_stats_tcps_name = pandas_stats_tcps[pandas_stats_tcps["name"]==name]
            pandas_stats_tcps_name_true = pandas_stats_tcps_name[pandas_stats_tcps_name["is_success"]==True]
            pandas_stats_tcps_name_false = pandas_stats_tcps_name[pandas_stats_tcps_name["is_success"]==False]

            if pandas_stats_tcps_name["name"].count() > 0:
                run_time = pandas_stats_tcps_name["end_time"].max() - pandas_stats_tcps_name["start_time"].min()
                run_time = run_time.total_seconds()

                if run_time == 0:
                    run_time = 1

                TcpTestUser.append_stats_tcps_currents(
                    client_id,
                    case_id,
                    name,
                    pandas_stats_tcps_name["start_time"].min().strftime(datetime_format),
                    pandas_stats_tcps_name["end_time"].max().strftime(datetime_format),
                    float(round(run_time, 6)),
                    float(round(pandas_stats_tcps_name["run_time"].max(), 6)),
                    float(round(pandas_stats_tcps_name["run_time"].min(), 6)),
                    float(round(pandas_stats_tcps_name["run_time"].mean(), 6)),
                    float(round(pandas_stats_tcps_name["run_time"].median(), 6)),
                    float(pandas_stats_tcps_name["is_success"].count()),
                    float(pandas_stats_tcps_name_true["is_success"].count()),
                    float(pandas_stats_tcps_name_false["is_success"].count()),
                    float(round(pandas_stats_tcps_name["is_success"].count() / run_time, 6)),
                    float(round(pandas_stats_tcps_name_true["is_success"].count() / run_time, 6)),
                    float(round(pandas_stats_tcps_name_false["is_success"].count() / run_time, 6)),
                    float(round(pandas_stats_tcps_name["is_success"].count() / run_time, 6)),
                    float(round(pandas_stats_tcps_name["is_success"].count() / run_time, 6)),
                    float(round(pandas_stats_tcps_name_true["is_success"].count() / run_time, 6)),
                    float(round(pandas_stats_tcps_name_true["is_success"].count() / run_time, 6)),
                    float(round(pandas_stats_tcps_name_false["is_success"].count() / run_time, 6)),
                    float(round(pandas_stats_tcps_name_false["is_success"].count() / run_time, 6)))       

        # pandas_stats_tcps_currents = pandas.read_json(json.dumps(TcpTestUser.stats_tcps_currents["stats_tcps_currents"]), "records")
        # print(pandas_stats_tcps_currents)

        return True

    def create_stats_tcps_currents_worker_report():
        if is_current is False:
            return True

        if len(TcpTestUser.stats_tcps["stats_tcps"]) == 0:
            return False

        stats_tcps_currents = TcpTestUser.stats_tcps_currents
        TcpTestUser.reset_stats_tcps_currents()        

        # pandas_stats_tcps_currents = pandas.read_json(json.dumps(TcpTestUser.stats_tcps["stats_tcps"]), "records")
        pandas_stats_tcps_currents = pandas.read_json(json.dumps(stats_tcps_currents["stats_tcps_currents"]), "records")
        # print(pandas_stats_tcps_currents)

        if len(pandas_stats_tcps_currents) == 0:
            return False

        names = ["connect", "login", "send",  "recv"]

        for name in names:
            pandas_stats_tcps_currents_name = pandas_stats_tcps_currents[pandas_stats_tcps_currents["name"]==name]

            if pandas_stats_tcps_currents_name["name"].count() > 0:
                run_time = pandas_stats_tcps_currents_name["end_time"].max() - pandas_stats_tcps_currents_name["start_time"].min()
                run_time = run_time.total_seconds()

                if run_time == 0:
                    run_time = 1

                TcpTestUser.append_stats_tcps_currents(
                    client_id,
                    case_id,
                    name,
                    pandas_stats_tcps_currents_name["start_time"].min().strftime(datetime_format),
                    pandas_stats_tcps_currents_name["end_time"].max().strftime(datetime_format),
                    float(round(run_time, 6)),
                    float(round(pandas_stats_tcps_currents_name["max_time"].max(), 6)),
                    float(round(pandas_stats_tcps_currents_name["min_time"].min(), 6)),
                    float(round(pandas_stats_tcps_currents_name["avg_time"].mean(), 6)),
                    float(round(pandas_stats_tcps_currents_name["med_time"].median(), 6)),
                    float(pandas_stats_tcps_currents_name["count_num"].sum()),
                    float(pandas_stats_tcps_currents_name["success_num"].sum()),
                    float(pandas_stats_tcps_currents_name["fail_num"].sum()),
                    float(round(pandas_stats_tcps_currents_name["count_num"].sum() / run_time, 6)),
                    float(round(pandas_stats_tcps_currents_name["success_num"].sum() / run_time, 6)),
                    float(round(pandas_stats_tcps_currents_name["fail_num"].sum() / run_time, 6)),
                    float(round(pandas_stats_tcps_currents_name["count_s"].max(), 6)),
                    float(round(pandas_stats_tcps_currents_name["count_s"].min(), 6)),
                    float(round(pandas_stats_tcps_currents_name["success_s"].max(), 6)),
                    float(round(pandas_stats_tcps_currents_name["success_s"].min(), 6)),
                    float(round(pandas_stats_tcps_currents_name["fail_s"].max(), 6)),
                    float(round(pandas_stats_tcps_currents_name["fail_s"].max(), 6)))          

        pandas_stats_tcps_currents = pandas.read_json(json.dumps(TcpTestUser.stats_tcps_currents["stats_tcps_currents"]), "records")
        # print(pandas_stats_tcps_currents)

        return True

    def create_stats_tcps_totals():
        if len(TcpTestUser.stats_tcps["stats_tcps"]) == 0:
            return False

        TcpTestUser.reset_stats_tcps_totals()

        pandas_stats_tcps = pandas.read_json(json.dumps(TcpTestUser.stats_tcps["stats_tcps"]), "records")
        # print(pandas_stats_tcps)

        if len(pandas_stats_tcps) == 0:
            return False        

        names = ["connect", "login", "send",  "recv"]

        for name in names:
            pandas_stats_tcps_name = pandas_stats_tcps[pandas_stats_tcps["name"]==name]
            pandas_stats_tcps_name_true = pandas_stats_tcps_name[pandas_stats_tcps_name["is_success"]==True]
            pandas_stats_tcps_name_false = pandas_stats_tcps_name[pandas_stats_tcps_name["is_success"]==False]

            if pandas_stats_tcps_name["name"].count() > 0:
                run_time = pandas_stats_tcps_name["end_time"].max() - pandas_stats_tcps_name["start_time"].min()
                run_time = run_time.total_seconds()

                if run_time == 0:
                    run_time = 1

                TcpTestUser.append_stats_tcps_totals(
                    client_id,
                    case_id,
                    name,
                    pandas_stats_tcps_name["start_time"].min().strftime(datetime_format),
                    pandas_stats_tcps_name["end_time"].max().strftime(datetime_format),
                    float(round(run_time, 6)),
                    float(round(pandas_stats_tcps_name["run_time"].max(), 6)),
                    float(round(pandas_stats_tcps_name["run_time"].min(), 6)),
                    float(round(pandas_stats_tcps_name["run_time"].mean(), 6)),
                    float(round(pandas_stats_tcps_name["run_time"].median(), 6)),
                    float(pandas_stats_tcps_name["is_success"].count()),
                    float(pandas_stats_tcps_name_true["is_success"].count()),
                    float(pandas_stats_tcps_name_false["is_success"].count()),
                    float(round(pandas_stats_tcps_name["is_success"].count() / run_time, 6)),
                    float(round(pandas_stats_tcps_name_true["is_success"].count() / run_time, 6)),
                    float(round(pandas_stats_tcps_name_false["is_success"].count() / run_time, 6)),
                    float(round(pandas_stats_tcps_name["is_success"].count() / run_time, 6)),
                    float(round(pandas_stats_tcps_name["is_success"].count() / run_time, 6)),
                    float(round(pandas_stats_tcps_name_true["is_success"].count() / run_time, 6)),
                    float(round(pandas_stats_tcps_name_true["is_success"].count() / run_time, 6)),
                    float(round(pandas_stats_tcps_name_false["is_success"].count() / run_time, 6)),
                    float(round(pandas_stats_tcps_name_false["is_success"].count() / run_time, 6)))            

        # pandas_stats_tcps_totals = pandas.read_json(json.dumps(TcpTestUser.stats_tcps_totals["stats_tcps_totals"]), "records")
        # print(pandas_stats_tcps_totals)
        return True

    def create_stats_tcps_totals_worker_report():
        if len(TcpTestUser.stats_tcps_totals["stats_tcps_totals"]) == 0:
            return False

        stats_tcps_totals = TcpTestUser.stats_tcps_totals
        TcpTestUser.reset_stats_tcps_totals()        

        pandas_stats_tcps_totals = pandas.read_json(json.dumps(stats_tcps_totals["stats_tcps_totals"]), "records")
        # print(pandas_stats_tcps_totals)

        if len(pandas_stats_tcps_totals) == 0:
            return False    

        names = ["connect", "login", "send",  "recv"]

        for name in names:
            pandas_stats_tcps_totals_name = pandas_stats_tcps_totals[pandas_stats_tcps_totals["name"]==name]

            if pandas_stats_tcps_totals_name["name"].count() > 0:
                run_time = pandas_stats_tcps_totals_name["end_time"].max() - pandas_stats_tcps_totals_name["start_time"].min()
                run_time = run_time.total_seconds()

                if run_time == 0:
                    run_time = 1

                TcpTestUser.append_stats_tcps_totals(
                    client_id,
                    case_id,
                    name,
                    pandas_stats_tcps_totals_name["start_time"].min().strftime(datetime_format),
                    pandas_stats_tcps_totals_name["end_time"].max().strftime(datetime_format),
                    float(round(run_time, 6)),
                    float(round(pandas_stats_tcps_totals_name["max_time"].max(), 6)),
                    float(round(pandas_stats_tcps_totals_name["min_time"].min(), 6)),
                    float(round(pandas_stats_tcps_totals_name["avg_time"].mean(), 6)),
                    float(round(pandas_stats_tcps_totals_name["med_time"].median(), 6)),
                    float(pandas_stats_tcps_totals_name["count_num"].sum()),
                    float(pandas_stats_tcps_totals_name["success_num"].sum()),
                    float(pandas_stats_tcps_totals_name["fail_num"].sum()),
                    float(round(pandas_stats_tcps_totals_name["count_num"].sum() / run_time, 6)),
                    float(round(pandas_stats_tcps_totals_name["success_num"].sum() / run_time, 6)),
                    float(round(pandas_stats_tcps_totals_name["fail_num"].sum() / run_time, 6)),
                    float(round(pandas_stats_tcps_totals_name["count_s"].max(), 6)),
                    float(round(pandas_stats_tcps_totals_name["count_s"].min(), 6)),
                    float(round(pandas_stats_tcps_totals_name["success_s"].max(), 6)),
                    float(round(pandas_stats_tcps_totals_name["success_s"].min(), 6)),
                    float(round(pandas_stats_tcps_totals_name["fail_s"].max(), 6)),
                    float(round(pandas_stats_tcps_totals_name["fail_s"].max(), 6)))

        # pandas_stats_tcps_totals = pandas.read_json(json.dumps(TcpTestUser.stats_tcps_totals["stats_tcps_totals"]), "records")
        # print(pandas_stats_tcps_totals)
        return True

    def save_data(stats_tcps, stats_tcps_currents, stats_tcps_totals):
        # print("save_data")

        pandas_stats_tcps = pandas.read_json(json.dumps(stats_tcps["stats_tcps"]), "records")
        print(pandas_stats_tcps)

        pandas_stats_tcps_currents = pandas.read_json(json.dumps(stats_tcps_currents["stats_tcps_currents"]), "records")
        print(pandas_stats_tcps_currents)

        pandas_stats_tcps_totals = pandas.read_json(json.dumps(stats_tcps_totals["stats_tcps_totals"]), "records")
        print(pandas_stats_tcps_totals)

        TcpTestUser.add_master_data(stats_tcps, stats_tcps_currents, stats_tcps_totals)

        # stats_tcps = {
        #     "stats_tcps": [
        #         {
        #             "user_id": "user_id",
        #             "client_id": "client_id",
        #             "case_id": "case_id",
        #             "name": "connect",
        #             "start_time": "2020-06-06 00:00:00.000000",
        #             "end_time": "2020-06-06 00:00:00.000000",
        #             "run_time": 0.0,
        #             "is_success": True,
        #             "message": "",
        #             "error_message": ""
        #         }
        #     ]
        # }

        # stats_tcps_currents = {
        #     "stats_tcps_currents": [
        #         {
        #             "client_id": "client_id",
        #             "case_id": "case_id",
        #             "name": "connect",
        #             "start_time": "2020-06-06 00:00:00.000000",
        #             "end_time": "2020-06-06 00:00:00.000000",
        #             "run_time": 0.0,
        #             "max_time": 0.0,
        #             "min_time": 0.0,
        #             "avg_time": 0.0,
        #             "med_time": 0.0,
        #             "count_num": 0.0,
        #             "success_num": 0,
        #             "fail_num": 0,
        #             "count/s": 0,
        #             "success/s": 0,
        #             "fail/s": 0,
        #             "max_count/s": 0,
        #             "min_count/s": 0,
        #             "max_success/s": 0,
        #             "min_success/s": 0,
        #             "max_fail/s": 0,
        #             "min_fail/s": 0
        #         },
        #         {
        #             "client_id": "client_id",
        #             "case_id": "case_id",
        #             "name": "send",
        #             "start_time": "2020-06-06 00:00:00.000000",
        #             "end_time": "2020-06-06 00:00:00.000000",
        #             "run_time": 0.0,
        #             "max_time": 0.0,
        #             "min_time": 0.0,
        #             "avg_time": 0.0,
        #             "med_time": 0.0,
        #             "count_num": 0.0,
        #             "success_num": 0,
        #             "fail_num": 0,
        #             "count/s": 0,
        #             "success/s": 0,
        #             "fail/s": 0,
        #             "max_count/s": 0,
        #             "min_count/s": 0,
        #             "max_success/s": 0,
        #             "min_success/s": 0,
        #             "max_fail/s": 0,
        #             "min_fail/s": 0
        #         },
        #         {
        #             "client_id": "client_id",
        #             "case_id": "case_id",
        #             "name": "recv",
        #             "start_time": "2020-06-06 00:00:00.000000",
        #             "end_time": "2020-06-06 00:00:00.000000",
        #             "run_time": 0.0,
        #             "max_time": 0.0,
        #             "min_time": 0.0,
        #             "avg_time": 0.0,
        #             "med_time": 0.0,
        #             "count_num": 0.0,
        #             "success_num": 0,
        #             "fail_num": 0,
        #             "count/s": 0,
        #             "success/s": 0,
        #             "fail/s": 0,
        #             "max_count/s": 0,
        #             "min_count/s": 0,
        #             "max_success/s": 0,
        #             "min_success/s": 0,
        #             "max_fail/s": 0,
        #             "min_fail/s": 0
        #         }
        #     ]
        # }

        # stats_tcps_totals = {
        #     "stats_tcps_totals": [
        #         {
        #             "client_id": "client_id",
        #             "case_id": "case_id",
        #             "name": "connect",
        #             "start_time": "2020-06-06 00:00:00.000000",
        #             "end_time": "2020-06-06 00:00:00.000000",
        #             "run_time": 0.0,
        #             "max_time": 0.0,
        #             "min_time": 0.0,
        #             "avg_time": 0.0,
        #             "med_time": 0.0,
        #             "count_num": 0.0,
        #             "success_num": 0,
        #             "fail_num": 0,
        #             "count/s": 0,
        #             "success/s": 0,
        #             "fail/s": 0,
        #             "max_count/s": 0,
        #             "min_count/s": 0,
        #             "max_success/s": 0,
        #             "min_success/s": 0,
        #             "max_fail/s": 0,
        #             "min_fail/s": 0
        #         },
        #         {
        #             "client_id": "client_id",
        #             "case_id": "case_id",
        #             "name": "send",
        #             "start_time": "2020-06-06 00:00:00.000000",
        #             "end_time": "2020-06-06 00:00:00.000000",
        #             "run_time": 0.0,
        #             "max_time": 0.0,
        #             "min_time": 0.0,
        #             "avg_time": 0.0,
        #             "med_time": 0.0,
        #             "count_num": 0.0,
        #             "success_num": 0,
        #             "fail_num": 0,
        #             "count/s": 0,
        #             "success/s": 0,
        #             "fail/s": 0,
        #             "max_count/s": 0,
        #             "min_count/s": 0,
        #             "max_success/s": 0,
        #             "min_success/s": 0,
        #             "max_fail/s": 0,
        #             "min_fail/s": 0
        #         },
        #         {
        #             "client_id": "client_id",
        #             "case_id": "case_id",
        #             "name": "recv",
        #             "start_time": "2020-06-06 00:00:00.000000",
        #             "end_time": "2020-06-06 00:00:00.000000",
        #             "run_time": 0.0,
        #             "max_time": 0.0,
        #             "min_time": 0.0,
        #             "avg_time": 0.0,
        #             "med_time": 0.0,
        #             "count_num": 0.0,
        #             "success_num": 0,
        #             "fail_num": 0,
        #             "count/s": 0,
        #             "success/s": 0,
        #             "fail/s": 0,
        #             "max_count/s": 0,
        #             "min_count/s": 0,
        #             "max_success/s": 0,
        #             "min_success/s": 0,
        #             "max_fail/s": 0,
        #             "min_fail/s": 0
        #         }
        #     ]
        # }

        # task_data = {
        #     "client_id": "",
        #     "data": {
        #         "stats_tcps": [
        #             {
        #                 "user_id": "user_id",
        #                 "client_id": "client_id",
        #                 "case_id": "case_id",
        #                 "name": "connect",
        #                 "start_time": "2020-06-06 00:00:00.000000",
        #                 "end_time": "2020-06-06 00:00:00.000000",
        #                 "run_time": 0.0,
        #                 "is_success": True,
        #                 "message": "",
        #                 "error_message": ""
        #             },
        #             {
        #                 "user_id": "user_id",
        #                 "client_id": "client_id",
        #                 "case_id": "case_id",
        #                 "name": "send",
        #                 "start_time": "2020-06-06 00:00:00.000000",
        #                 "end_time": "2020-06-06 00:00:00.000000",
        #                 "run_time": 0.0,
        #                 "is_success": True,
        #                 "message": "",
        #                 "error_message": ""
        #             },
        #             {
        #                 "user_id": "user_id",
        #                 "client_id": "client_id",
        #                 "case_id": "case_id",
        #                 "name": "recv",
        #                 "start_time": "2020-06-06 00:00:00.000000",
        #                 "end_time": "2020-06-06 00:00:00.000000",
        #                 "run_time": 0.0,
        #                 "is_success": True,
        #                 "message": "",
        #                 "error_message": ""
        #             }
        #         ],
        #         "stats_tcps_totals": [
        #             {
        #                 "client_id": "client_id",
        #                 "case_id": "case_id",
        #                 "name": "connect",
        #                 "start_time": "2020-06-06 00:00:00.000000",
        #                 "end_time": "2020-06-06 00:00:00.000000",
        #                 "run_time": 0.0,
        #                 "max_time": 0.0,
        #                 "min_time": 0.0,
        #                 "avg_time": 0.0,
        #                 "med_time": 0.0,
        #                 "count_num": 0.0,
        #                 "success_num": 0,
        #                 "fail_num": 0,
        #                 "count/s": 0,
        #                 "success/s": 0,
        #                 "fail/s": 0,
        #                 "max_count/s": 0,
        #                 "min_count/s": 0,
        #                 "max_success/s": 0,
        #                 "min_success/s": 0,
        #                 "max_fail/s": 0,
        #                 "min_fail/s": 0
        #             },
        #             {
        #                 "client_id": "client_id",
        #                 "case_id": "case_id",
        #                 "name": "send",
        #                 "start_time": "2020-06-06 00:00:00.000000",
        #                 "end_time": "2020-06-06 00:00:00.000000",
        #                 "run_time": 0.0,
        #                 "max_time": 0.0,
        #                 "min_time": 0.0,
        #                 "avg_time": 0.0,
        #                 "med_time": 0.0,
        #                 "count_num": 0.0,
        #                 "success_num": 0,
        #                 "fail_num": 0,
        #                 "count/s": 0,
        #                 "success/s": 0,
        #                 "fail/s": 0,
        #                 "max_count/s": 0,
        #                 "min_count/s": 0,
        #                 "max_success/s": 0,
        #                 "min_success/s": 0,
        #                 "max_fail/s": 0,
        #                 "min_fail/s": 0
        #             },
        #             {
        #                 "client_id": "client_id",
        #                 "case_id": "case_id",
        #                 "name": "recv",
        #                 "start_time": "2020-06-06 00:00:00.000000",
        #                 "end_time": "2020-06-06 00:00:00.000000",
        #                 "run_time": 0.0,
        #                 "max_time": 0.0,
        #                 "min_time": 0.0,
        #                 "avg_time": 0.0,
        #                 "med_time": 0.0,
        #                 "count_num": 0.0,
        #                 "success_num": 0,
        #                 "fail_num": 0,
        #                 "count/s": 0,
        #                 "success/s": 0,
        #                 "fail/s": 0,
        #                 "max_count/s": 0,
        #                 "min_count/s": 0,
        #                 "max_success/s": 0,
        #                 "min_success/s": 0,
        #                 "max_fail/s": 0,
        #                 "min_fail/s": 0
        #             }
        #         ]
        #     }
        # }

    def add_master_data(stats_tcps, stats_tcps_currents, stats_tcps_totals):
        # print("add_master_data")

        if len(stats_tcps_totals["stats_tcps_totals"]) == 0:
            return

        master_data = {}
        master_data["CaseID"] = ""
        master_data["ConnectCount"] = "0"
        master_data["ConnectFailCount"] = "0"
        master_data["ReqCount"] = "0"
        master_data["ReqFailCount"] = "0"
        master_data["MaxDuration"] = "0.0"
        master_data["MinDurartion"] = "0.0"
        master_data["AvgDuration"] = "0.0"

        for row in stats_tcps_currents["stats_tcps_currents"]:
            if row["name"] == "connect":
                a = 1
            elif row["name"] == "send":
                master_data["ReqCount"] = str(row["count_num"])
                master_data["ReqFailCount"] = str(row["fail_num"])
                master_data["MaxDuration"] = str(row["max_time"])
                master_data["MinDurartion"] = str(row["min_time"])
                master_data["AvgDuration"] = str(row["avg_time"])
            elif row["name"] == "recv":
                a = 1

        for row in stats_tcps_totals["stats_tcps_totals"]:
            if row["name"] == "connect":
                master_data["CaseID"] = row["case_id"]
                master_data["ConnectCount"] = str(row["count_num"])
                master_data["ConnectFailCount"] = str(row["fail_num"])
            elif row["name"] == "send":
                a = 1
            elif row["name"] == "recv":
                a = 1

        if master_data["CaseID"] == "":
            return

        # print(master_data)
        TcpTestUser.post_api("api/monitor/addmasterdata", master_data)

    def add_worker_data(stats_tcps, stats_tcps_currents, stats_tcps_totals):
        # print("add_worker_data")

        if len(stats_tcps_currents["stats_tcps_currents"]) == 0:
            return

        worker_data_data = {}
        worker_data_data["CaseID"] = ""
        worker_data_data["SlaveID"] = ""
        worker_data_data["QPS"] = "0.0"
        worker_data_data["Time"] = datetime.datetime.utcnow().strftime("%Y%m%d%H%M%S")

        for row in stats_tcps_currents["stats_tcps_currents"]:
            if row["name"] == "connect":
                a = 1
            elif row["name"] == "send":
                worker_data_data["CaseID"] = row["case_id"]
                worker_data_data["SlaveID"] = row["client_id"]
                worker_data_data["QPS"] = str(row["count_s"])
                worker_data_data["Time"] = datetime.datetime.utcnow().strftime("%Y%m%d%H%M%S")
            elif row["name"] == "recv":
                a = 1

        worker_data = []
        worker_data.append(worker_data_data)

        if len(worker_data) == 0:
            return
        elif worker_data[0]["CaseID"] == "":
            return

        # print(worker_data)
        TcpTestUser.post_api("api/monitor/addslavedata", worker_data)

    def add_history_data(stats_tcps, stats_tcps_currents, stats_tcps_totals):
        # print("add_history_data")

        if len(stats_tcps_totals["stats_tcps_totals"]) == 0:
            return

        history_data = {}
        history_data["CaseID"] = ""
        history_data["ConnectCount"] = 0
        history_data["ConnectFailCount"] = 0
        history_data["ReqCount"] = 0
        history_data["ReqFailCount"] = 0
        history_data["MaxQPS"] = 0.0
        history_data["MinQPS"] = 0.0
        history_data["AvgQPS"] = 0.0
        history_data["MaxDuration"] = 0.0
        history_data["MinDurartion"] = 0.0
        history_data["AvgDuration"] = 0.0

        for row in stats_tcps_totals["stats_tcps_totals"]:
            if row["name"] == "connect":
                history_data["CaseID"] = row["case_id"]
                history_data["ConnectCount"] = int(row["count_num"])
                history_data["ConnectFailCount"] = int(row["fail_num"])
            elif row["name"] == "send":
                history_data["ReqCount"] = int(row["count_num"])
                history_data["ReqFailCount"] = int(row["fail_num"])
                history_data["MaxDuration"] = row["max_time"]
                history_data["MinDurartion"] = row["min_time"]
                history_data["AvgDuration"] = row["avg_time"]
                history_data["MaxQPS"] = row["max_count_s"]
                history_data["MinQPS"] = row["min_count_s"]
                history_data["AvgQPS"] = row["count_s"]
            elif row["name"] == "recv":
                a = 1

        if history_data["CaseID"] == "":
            return

        # print(history_data)
        TcpTestUser.post_api("api/report/addhistory", history_data)

    def post_api(path, data):
        # print("post_api")
        # print(data)

        try:
            github_url = "%s%s" % (case_service_base_address, path)
            headers = {
                "Content-Type": "application/json"
            }
            result = requests.post(github_url, json=data, headers=headers)
            # print("Url: %s" % github_url)
            # print("Result: %s" % result.text)
        except Exception as e:
            print(str(e))
            return

    def quitting():
        if len(TcpTestUser.stats_tcps_totals["stats_tcps_totals"]) == 0:
            # 在单节点非分布式运行时，判断此为master，master在quitting前，需汇总数据
            print("master")
            TcpTestUser.create_stats_tcps_totals()
        else:
            # 在多节点分布式运行时，判断此为worker，work在quitting前，不用处理任何数据
            print("worker")

        if len(TcpTestUser.stats_tcps["stats_tcps"]) == 0:
            # 在多节点分布式运行时，判断此为worker，work在quitting前，会清空stats_tcps数据
            print("worker")
        else:
            # 在单节点非分布式运行时，判断此为master，master在quitting前，需提交API
            print("master")
            TcpTestUser.create_stats_tcps_currents_worker_report()
            TcpTestUser.create_stats_tcps_totals_worker_report()
            TcpTestUser.save_data(TcpTestUser.stats_tcps, TcpTestUser.stats_tcps_currents, TcpTestUser.stats_tcps_totals)

        TcpTestUser.add_history_data(TcpTestUser.stats_tcps, TcpTestUser.stats_tcps_currents, TcpTestUser.stats_tcps_totals)        
        TcpTestUser.reset_stats_tcps()
        TcpTestUser.reset_stats_tcps_currents()
        TcpTestUser.reset_stats_tcps_totals()

    @events.hatch_complete.add_listener
    def on_hatch_complete(**kwargs):
        time.sleep(1)
        # print("on_hatch_complete: %s" % kwargs["user_count"])        

        if is_hatch_complete_run:
            # print(all_locusts_spawned.ready())

            if not all_locusts_spawned.ready():
                all_locusts_spawned.release()

            # print(all_locusts_spawned.ready())

    @events.test_start.add_listener
    def on_test_start(**kwargs):
        # print("on_test_start")        

        if is_hatch_complete_run:
            # print(all_locusts_spawned.ready())

            if all_locusts_spawned.ready():
                all_locusts_spawned.acquire()

            # print(all_locusts_spawned.ready())

    @events.test_stop.add_listener
    def on_test_stop(**kwargs):
        # print("on_test_stop")

        if is_hatch_complete_run:
            # print(all_locusts_spawned.ready())

            if not all_locusts_spawned.ready():
                all_locusts_spawned.release()

            # print(all_locusts_spawned.ready())

    @events.worker_report.add_listener
    def on_worker_report(**kwargs):
        print("on_worker_report")

        # 在多节点分布式运行时，判断此为master, 汇总worker来的数据
        lock.acquire()
        print(kwargs)

        TcpTestUser.stats_tcps["stats_tcps"] = TcpTestUser.stats_tcps["stats_tcps"] + kwargs["data"]["stats_tcps"]
        TcpTestUser.stats_tcps_currents["stats_tcps_currents"] = TcpTestUser.stats_tcps_currents["stats_tcps_currents"] + kwargs["data"]["stats_tcps_currents"]
        TcpTestUser.stats_tcps_totals["stats_tcps_totals"] = TcpTestUser.stats_tcps_totals["stats_tcps_totals"] + kwargs["data"]["stats_tcps_totals"]
        
        if TcpTestUser.is_worker_report_time():
            TcpTestUser.create_stats_tcps_currents_worker_report()
            TcpTestUser.create_stats_tcps_totals_worker_report()

            if is_worker_report_save:
                if len(TcpTestUser.stats_tcps["stats_tcps"]) > 0 and len(TcpTestUser.stats_tcps_totals["stats_tcps_totals"]) > 0:
                    # 在多节点分布式运行时，判断此为master, 汇总数据并提交API
                    TcpTestUser.save_data(TcpTestUser.stats_tcps, TcpTestUser.stats_tcps_currents, TcpTestUser.stats_tcps_totals)

                    TcpTestUser.reset_stats_tcps()
                    TcpTestUser.reset_stats_tcps_currents()
                    # TcpTestUser.reset_stats_tcps_totals()
        
        lock.release()

    @events.report_to_master.add_listener
    def on_report_to_master(**kwargs):
        # print("on_report_to_master")

        # 在多节点分布式运行时，判断此为worker, 汇总数据后提交master，并清空worker数据
        lock.acquire()
        kwargs["data"]["stats_tcps"] = TcpTestUser.stats_tcps["stats_tcps"]
        TcpTestUser.create_stats_tcps_currents()
        kwargs["data"]["stats_tcps_currents"] = TcpTestUser.stats_tcps_currents["stats_tcps_currents"]
        TcpTestUser.create_stats_tcps_totals()
        kwargs["data"]["stats_tcps_totals"] = TcpTestUser.stats_tcps_totals["stats_tcps_totals"]

        TcpTestUser.add_worker_data(TcpTestUser.stats_tcps, TcpTestUser.stats_tcps_currents, TcpTestUser.stats_tcps_totals)

        TcpTestUser.reset_stats_tcps()
        TcpTestUser.reset_stats_tcps_currents()
        TcpTestUser.reset_stats_tcps_totals()
        lock.release()

    @events.quitting.add_listener
    def on_quitting(**kwargs):
        # print("on_quitting")
        TcpTestUser.quitting()

    def on_start(self):        
        # print("on_start")

        if is_hatch_complete_run:
            # print(all_locusts_spawned.ready())
            lock.acquire()

            if all_locusts_spawned.ready():
                all_locusts_spawned.acquire()

            lock.release()
            # print(all_locusts_spawned.ready())

        connect_run_time = 0.0
        connect_start_time = datetime.datetime.now()
        is_success = self.connect()
        connect_end_time = datetime.datetime.now()
        connect_run_time = connect_end_time - connect_start_time

        self.append_stats_tcps(
            self.user_id,
            client_id,
            case_id,
            "connect",
            connect_start_time.strftime(datetime_format),
            connect_end_time.strftime(datetime_format),
            round(connect_run_time.total_seconds()),
            is_success,
            "",
            "")

        login_run_time = 0.0
        login_start_time = datetime.datetime.now()
        is_success = self.login()
        login_end_time = datetime.datetime.now()
        login_run_time = login_end_time - login_start_time

        self.append_stats_tcps(
            self.user_id,
            client_id,
            case_id,
            "login",
            login_start_time.strftime(datetime_format),
            login_end_time.strftime(datetime_format),
            round(login_run_time.total_seconds()),
            is_success,
            "",
            "")

        # t = threading.Thread(target=self.recv_data_thread)
        # t.setDaemon(True)
        # t.start()

        if is_hatch_complete_run:
            all_locusts_spawned.wait()

    def on_stop(self):
        # print("on_stop")

        if is_hatch_complete_run:
            # print(all_locusts_spawned.ready())
            lock.acquire()

            if not all_locusts_spawned.ready():
                all_locusts_spawned.release()

            lock.release()
            # print(all_locusts_spawned.ready())

        self.client.close()

    @task(1)
    def send(self):
        send_run_time = 0.0
        recv_run_time = 0.0

        package = self.get_package()
        send_start_time = datetime.datetime.now()
        send_is_success = self.send_data(package)
        send_end_time = datetime.datetime.now()
        send_run_time = send_end_time - send_start_time
        
        self.append_stats_tcps(
            self.user_id,
            client_id,
            case_id,
            "send",
            send_start_time.strftime(datetime_format),
            send_end_time.strftime(datetime_format),
            round(send_run_time.total_seconds(), 6),
            send_is_success,
            package,
            "")        

        package_recv = [""]
        recv_start_time = datetime.datetime.now()
        recv_is_success = self.recv_data(package_recv)
        recv_end_time = datetime.datetime.now()
        recv_run_time = recv_end_time - recv_start_time

        self.append_stats_tcps(
            self.user_id,
            client_id,
            case_id,
            "recv",
            recv_start_time.strftime(datetime_format),
            recv_end_time.strftime(datetime_format),
            round(recv_run_time.total_seconds(), 6),
            recv_is_success,
            package_recv[0],
            "")

        self.reset_data()


# if __name__ == "__main__":
#     env = Environment(user_classes=[TcpTestUser])
#     env.create_local_runner()

#     # start a WebUI instance
#     env.create_web_ui("127.0.0.1", 8089)

#     # start a greenlet that periodically outputs the current stats
#     gevent.spawn(stats_printer(env.stats))

#     # start the test
#     env.runner.start(1, hatch_rate=1)

#     # in 60 seconds stop the runner
#     gevent.spawn_later(60, lambda: env.runner.quit())

#     # wait for the greenlets
#     env.runner.greenlet.join()

#     # stop the web server for good measures
#     env.web_ui.stop()

if __name__ == "__main__":
    import os

    os.system(
        "locust -f .\\locust-tcp\\locustfiles\\TcpSocketClient.py")

# locust -f "E:\Documents\Visual Studio Code\TestPython\locust-tcp\locustfiles\TcpSocketClient.py"
# locust -f "E:\Documents\Visual Studio Code\TestPython\locust-tcp\locustfiles\TcpSocketClient.py" --master --master-bind-host 127.0.0.1 --master-bind-port 5557 --expect-workers 1
# locust -f "E:\Documents\Visual Studio Code\TestPython\locust-tcp\locustfiles\TcpSocketClient.py" --worker --master-host 127.0.0.1 --master-port 5557
# locust -f "E:\Documents\Visual Studio Code\TestPython\locust-tcp\locustfiles\TcpSocketClient.py" --headless -u 100 -r 10 -t 1m 
# locust -f "E:\Documents\Visual Studio Code\TestPython\locust-tcp\locustfiles\TcpSocketClient.py" --master --headless --expect-workers 1 -u 100 -r 10 -t 1m  
# locust -f "E:\Documents\Visual Studio Code\TestPython\locust-tcp\locustfiles\TcpSocketClient.py" --worker --master-host 127.0.0.1 --master-port 5557
# locust -f "E:\Documents\Visual Studio Code\TestPython\locust-tcp\locustfiles\TcpSocketClient.py" --logfile "E:\Documents\Visual Studio Code\TestPython\locust-tcp\log\log.log"
# locust -f "E:\Documents\Visual Studio Code\TestPython\locust-tcp\locustfiles\TcpSocketClient.py" --logfile "E:\Documents\Visual Studio Code\TestPython\locust-tcp\log\log.log" --master --headless -u 100 -r 10 -t 1m
# locust -f "E:\Documents\Visual Studio Code\TestPython\locust-tcp\locustfiles\TcpSocketClient.py" --worker --master-host=127.0.0.1 --master-port=5557

# locust -f "/usr/testfile/script.py" -u 100 -r 10 -t 1m --headless
