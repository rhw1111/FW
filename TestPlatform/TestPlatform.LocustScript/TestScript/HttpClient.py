# !/usr/bin/env python3
# -*- coding:utf-8 -*-

import time
import datetime
import socket
import random
import threading
import json
import uuid
import traceback
import gevent
from gevent._semaphore import Semaphore
import locust.stats
from locust import HttpLocust, Locust, HttpUser, User, TaskSet
from locust import events, task, between
from locust.event import EventHook
from locust.env import Environment
from locust.stats import stats_printer
from locust.log import setup_logging
from locust.contrib.fasthttp import FastHttpUser
import requests
from urllib import request, parse

import sys
import os
sys.path.append(os.path.dirname(os.path.dirname(__file__)))
# from config import config
# from case import case
# from api import api
# from common import common


# -----------------------------------------------------------
# {$additionfunc(0)}
# 按脚本名称随机获取该名称下的行数据，每次取完后，该行就不能再被获取
# 参数：
#   1，json键值对
# 返回值：
#   键值对
def NameOnceJsonData(data):
    # print("NameOnceJsonData")

    if data and type(data) == dict:
        return data
    if data and len(data) > 0:
        index = 0
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
#   5，name，用于Locust统计日志
#   6，self，实例
# 返回值：
#   响应内容截取的数据
def TcpRR(address, port, senddata, receivereg, name=None, self=None):
    # print("TcpRR")
    import socket
    import re
    import traceback
    import time

    if address is None or address == "" or port is None or port == "":
        return ""

    if senddata is None or senddata == "":
        return ""

    if type(senddata) != str:
        senddata = str(senddata)

    host = address
    port = port
    ADDR = (host, port)
    buffsize = 10240
    start_time = time.time()        

    connect = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

    try:
        connect.connect(ADDR)
        Print("Connect Success, %s:%s" % (address, port))

        connect.send(senddata.encode())
        Print("Send Success")
        Print("SendData, %s" % senddata)

        if sync_type:
            Print("Recv Waitting...")
            data = connect.recv(buffsize).decode()
            Print("RecvData, %s" % data)

            if name and self:
                total_time = int((time.time() - start_time) * second_unit)
                self.environment.events.request_success.fire(
                    request_type=request_type, name=name,
                    response_time=total_time, response_length=0)

            p = re.compile(receivereg, re.S)
            result = re.findall(p, data)

            if len(result) > 0:
                return result[0]
            else:
                return ""
        else:
            if name and self:
                total_time = int((time.time() - start_time) * second_unit)
                self.environment.events.request_success.fire(
                    request_type=request_type, name=name,
                    response_time=total_time, response_length=0)

            return "OK"
    except Exception as e:
        if name and self:
            total_time = int((time.time() - start_time) * second_unit)
            self.environment.events.request_failure.fire(
                request_type=request_type, name=name,
                response_time=total_time, response_length=len(str(e)),
                exception=e)

        print("[%s] [%s]: Error, %s." % (datetime.datetime.now().strftime(datetime_format), client_id, str(e)))
        print("[%s] [%s]: Error, %s." % (datetime.datetime.now().strftime(datetime_format), client_id, traceback.format_exc()))

        return ""
    finally:
        # Print("Connect Closeing...")
        connect.close()
        # Print("Connect Close Success")


# 根据传入的Tcp连接执行Tcp的请求发送与响应处理
# 参数：
#   1，Tcp连接
#   2，请求内容
#   3，响应内容截取匹配（正则表达式）
#   4，name，用于Locust统计日志
#   5，self，实例
# 返回值：
#   响应内容截取的数据
def TcpRRWithConnect(connect, senddata, receivereg, name=None, self=None):
    # print("TcpRRWithConnect")
    import socket
    import re
    import traceback
    import time

    if senddata is None or senddata == "":
        return ""

    if type(senddata) != str:
        senddata = str(senddata)

    buffsize = 10240
    start_time = time.time()    

    try:
        connect.send(senddata)
        Print("Send Success")
        Print("SendData, %s" % senddata)

        if sync_type:
            Print("Recv Waitting...")
            data = connect.recv(buffsize)
            Print("RecvData, %s" % data)

            if name and self:
                total_time = int((time.time() - start_time) * second_unit)
                self.environment.events.request_success.fire(
                    request_type=request_type, name=name,
                    response_time=total_time, response_length=0)

            p = re.compile(receivereg, re.S)
            result = re.findall(p, data)

            if len(result) > 0:
                return result[0]
            else:
                return ""
        else:
            if name and self:
                total_time = int((time.time() - start_time) * second_unit)
                self.environment.events.request_success.fire(
                    request_type=request_type, name=name,
                    response_time=total_time, response_length=0)

            return "OK"
    except Exception as e:
        if name and self:
            total_time = int((time.time() - start_time) * second_unit)
            self.environment.events.request_failure.fire(
                request_type=request_type, name=name,
                response_time=total_time, response_length=len(str(e)),
                exception=e)

        print("[%s] [%s]: Error, %s." % (datetime.datetime.now().strftime(datetime_format), client_id, str(e)))
        print("[%s] [%s]: Error, %s." % (datetime.datetime.now().strftime(datetime_format), client_id, traceback.format_exc()))

        return ""


# 随机获取json行数据列表中的一行数据
# 参数：
#   1，json列表
# 返回值：
#   键值对
def GetJsonRowData(data):
    # print("GetJsonRowData")
    import random

    if data and type(data) == dict:
        return data
    if data and len(data) > 0:
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
    # print("GetNameSerialNo")

    for serial_no in serial_no_s:
        if serial_no["Name"] == name and serial_no["Type"] == type:
            serial_no["SerialNo"] = serial_no["SerialNo"] + 1
            return serial_no["SerialNo"]

    serial_no = {
        "Name": name,
        "Type": type,
        "SerialNo": start
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

    if data and type(data) == dict:
        return data[name]
    elif data and len(data) > 0:
        index = random.randint(0, len(data) - 1)
        row = data[index]

        return row[name]

    return None


from Crypto.Cipher import DES3
import base64


# DES加密
# 参数：
#   1，要加密的数据
#   2，密钥
# 返回值：
#   加密后的数据
def DesSecurity(data, key):
    # print("DesSecurity")
    # from Crypto.Cipher import DES3
    # import base64

    # if len(key) % 16 > 0:
    #     return ""

    # key = key
    # mode = DES3.MODE_CBC
    # iv = b"12345678"
    # length = DES3.block_size

    # text = str(data)
    # text = text + (length - len(text) % length) * chr(length - len(text) % length)
    # cryptor = DES3.new(key, mode, iv)
    # # self.iv 为 IV 即偏移量
    # x = len(text) % 8

    # if x != 0:
    #     text = text + "\0" * (8 - x)  # 不满16，32，64位补0

    # ciphertext = cryptor.encrypt(text)
    # result = base64.standard_b64encode(ciphertext).decode("utf-8")

    # return result

    ed.setKey(key)
    return ed.encrypt(data)


class EncryptDate():
    def __init__(self, key):
        self.key = key
        self.mode = DES3.MODE_CBC
        self.iv = b"12345678"
        self.length = DES3.block_size

    def setKey(self, key):
        self.key = key

    def pad(self, s):
        return s + (self.length - len(s) % self.length) * chr(self.length - len(s) % self.length)

    # 定义 padding 即 填充 为PKCS7
    def unpad(self, s):
        return s[0:-ord(s[-1])]

    # DES3的加密模式为CBC
    def encrypt(self, text):
        if type(text) != str:
            text = str(text)

        text = self.pad(text)
        cryptor = DES3.new(self.key, self.mode, self.iv)
        # self.iv 为 IV 即偏移量
        x = len(text) % 8

        if x != 0:
            text = text + "\0" * (8 - x)  # 不满16，32，64位补0

        self.ciphertext = cryptor.encrypt(text)
        return base64.standard_b64encode(self.ciphertext).decode("utf-8")

    def decrypt(self, text):
        if type(text) != str:
            text = str(text)

        cryptor = DES3.new(self.key, self.mode, self.iv)
        de_text = base64.standard_b64decode(text)
        plain_text = cryptor.decrypt(de_text)
        st = str(plain_text.decode("utf-8")).rstrip("\0")
        out = self.unpad(st)
        return out
        # 上面注释内容解密如果运行报错，就注释掉试试
        # return plain_text


# 秘钥，密钥的长度必须是16的倍数
key = "abcdefghijklmnop"
ed = EncryptDate(key)


# 过滤数据
# 参数：
#   1，Json字符串
#   2，属性名称
#   3，属性值
# 返回值：
#   过滤后数据
def FilterJsonData(data, name, value):
    # print("FilterJsonData")

    if data and type(data) == dict:
        if data[name] == value:
            return [data]
        else:
            return []
    elif data and len(data) > 0:
        filterJsonData = []

        for row in data:
            if row[name] == value:
                filterJsonData.append(row)

        return filterJsonData

    return None


# 计算校验和
# 参数：
#   1，从"8=........"开始一直到"10="之前的部分，不包含10=
# 返回值：
#   校验和
def CalcCheckSum(msg):
    # print("CalcCheckSum")
    sum = 0

    if msg:
        for letter in msg:
            sum += ord(letter)

    return sum & 255


# 顺序取值
# 参数：
#   1，Json字符串
#   2，0=顺序取值，1=唯一取值，2=随机取值
#   3，0=结束测试，1=循环使用，2=循环停留最后一个
# 返回值：
#   返回Json数据
def GetJsonData(data, gettype, endtype):
    # print("GetJsonData")
    import random

    if data and type(data) == dict:
        return data
    elif data and len(data) > 0:
        if gettype == 0 or gettype == 1:
            if endtype == 0:
                index = 0
                row = data[index]
                del data[index]

                return row
            elif endtype == 1:
                index = 0
                row = data[index]
                del data[index]
                data.append(row)

                return row
            elif endtype == 2:
                index = 0
                row = data[index]

                if len(data) > 1:
                    del data[index]

                return row
        elif gettype == 2:
            index = random.randint(0, len(data) - 1)
            row = data[index]

            return row
    elif not data and len(data) == 0:
        if endtype == 0:
            HttpTestUser.environment.runner.quit()
            return None

    return None


# 数据分割
# 参数：
#   1，Json字符串
#   2，份数
# 返回值：
#   返回Json数据
def SplitJsonData(data, piece):
    # print("SplitJsonData")

    if data and type(data) == dict:
        return data
    elif piece <= 0:
        return None
    elif data and len(data) > 0:
        if len(data) < piece:
            result = data
            del(data)

            return result
        else:
            length = len(data)
            count = length // piece
            result = []

            for row in data:
                if not row.get("_SplitJsonData_"):
                    row["_SplitJsonData_"] = True
                    result.append(row)

                if(len(result) >= count):
                    break

            return result

    return None


# 打印内容
# 参数：
#   1，是否打印日志
#   2，打印内容
#   3，客户端ID
def Print(content):
    if is_print_log:
        print("[%s] [%s] [PrintLog]: %s." % (datetime.datetime.now().strftime(datetime_format), client_id, content))


# 记录Locust日志
def FireEventRequest(self, is_success, name, start_time, e):
    import time

    if not self or not name or not start_time:
        return

    total_time = int((time.time() - start_time) * second_unit)

    if is_success:
        self.environment.events.request_success.fire(
            request_type=request_type, name=name,
            response_time=total_time, response_length=0)
    else:
        self.environment.events.request_failure.fire(
            request_type=request_type, name=name,
            response_time=total_time, response_length=len(str(e)),
            exception=e)


# 格式化
# 参数：
#   1，DateTime类型的时间
#   2，时间格式，%Y-%m-%d %H:%M:%S
def DateTimeFormate(dt, formate):
    import datetime

    if not dt or not formate:
        return None

    if type(dt) == datetime.datetime:
        return dt.strftime(formate)


# 时间加减
# 参数：
#   1，dt：DateTime类型的时间
#   2，years，months，weeks，days，hours，minutes，seconds，microseconds，milliseconds
#   3，num：增加的数量，如：1，2，3，-1，-2，-3
def DateTimeAdd(dt, years=0, months=0, weeks=0, days=0, hours=0, minutes=0, seconds=0, microseconds=0, milliseconds=0):
    import datetime
    from dateutil.relativedelta import relativedelta

    if not dt:
        return None

    if type(dt) == datetime.datetime:
        if years:
            return dt + relativedelta(years=years)
        elif months:
            return dt + relativedelta(months=months)
        elif weeks:
            return dt + datetime.timedelta(weeks=weeks)
        elif days:
            return dt + datetime.timedelta(days=days)
        elif hours:
            return dt + datetime.timedelta(hours=hours)
        elif minutes:
            return dt + datetime.timedelta(minutes=minutes)
        elif seconds:
            return dt + datetime.timedelta(seconds=seconds)
        elif microseconds:
            return dt + datetime.timedelta(microseconds=microseconds)
        elif milliseconds:
            return dt + datetime.timedelta(milliseconds=milliseconds)


# 根据传入的Http连接执行Http的请求发送与响应处理
# 参数：
#   1，Http连接
#   2，Url
#   3，响应内容截取匹配（正则表达式）
# 返回值：
#   响应内容截取的数据
def HttpGetWithConnect(connect, url, headers, receivereg, name=None, self=None):
    # print("HttpGetWithConnect")
    import re
    import traceback
    import time

    if not url:
        return ""
    
    start_time = time.time()

    try:
        if not headers:
            headers = {"User-Agent": "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/69.0.3497.100 Safari/537.36"}

        response = connect.get(url, headers=headers)

        if response.status_code == 200:
            if name and self:
                total_time = int((time.time() - start_time) * second_unit)
                self.environment.events.request_success.fire(
                    request_type=request_type, name=name,
                    response_time=total_time, response_length=0)
            
            Print("Http Get Success, Url, %s%s, StatusCode, %s, Text, %s" % (connect.base_url, url, response.status_code, response.text))

            result = response.text

            p = re.compile(receivereg, re.S)
            result = re.findall(p, result)

            if len(result) > 0:
                return result[0]
            else:
                return ""
        else:
            if name and self:
                total_time = int((time.time() - start_time) * second_unit)
                self.environment.events.request_failure.fire(
                    request_type=request_type, name=name,
                    response_time=total_time, response_length=0,
                    exception=None)

            print("[%s] [%s]: Http Get Fail, Url, %s%s, StatusCode, %s, Text, %s." % (datetime.datetime.now().strftime(datetime_format), client_id, connect.base_url, url, response.status_code, response.text))

            return ""
    except Exception as e:
        if name and self:
            total_time = int((time.time() - start_time) * second_unit)
            self.environment.events.request_failure.fire(
                request_type=request_type, name=name,
                response_time=total_time, response_length=len(str(e)),
                exception=e)
        
        print("[%s] [%s]: Error, %s." % (datetime.datetime.now().strftime(datetime_format), client_id, str(e)))
        print("[%s] [%s]: Error, %s." % (datetime.datetime.now().strftime(datetime_format), client_id, traceback.format_exc()))

        return ""


# 根据传入的Http连接执行Http的请求发送与响应处理
# 参数：
#   1，Http连接
#   2，Url
#   3，响应内容截取匹配（正则表达式）
# 返回值：
#   响应内容截取的数据
def HttpPostWithConnect(connect, url, headers, senddata, receivereg, name=None, self=None):
    # print("HttpPostWithConnect")
    import re
    import traceback
    import time

    if not url:
        return ""

    start_time = time.time()

    try:
        if not headers:
            headers = {"User-Agent": "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/69.0.3497.100 Safari/537.36"}

        if type(senddata) == dict:
            headers = {"Content-Type": "application/json"}
            response = connect.post(url, headers=headers, json=senddata)
        else:
            response = connect.post(url, headers=headers, data=senddata)

        if response.status_code == 200:
            if name and self:
                total_time = int((time.time() - start_time) * second_unit)
                self.environment.events.request_success.fire(
                    request_type=request_type, name=name,
                    response_time=total_time, response_length=0)
            
            Print("Http Post Success, Url, %s%s, StatusCode, %s, Text, %s" % (connect.base_url, url, response.status_code, response.text))

            result = response.text

            p = re.compile(receivereg, re.S)
            result = re.findall(p, result)

            if len(result) > 0:
                return result[0]
            else:
                return ""
        else:
            if name and self:
                total_time = int((time.time() - start_time) * second_unit)
                self.environment.events.request_failure.fire(
                    request_type=request_type, name=name,
                    response_time=total_time, response_length=0,
                    exception=None)
            
            print("[%s] [%s]: Http Post Fail, Url, %s%s, StatusCode, %s, Text, %s." % (datetime.datetime.now().strftime(datetime_format), client_id, connect.base_url, url, response.status_code, response.text))

            return ""
    except Exception as e:
        if name and self:
            total_time = int((time.time() - start_time) * second_unit)
            self.environment.events.request_failure.fire(
                request_type=request_type, name=name,
                response_time=total_time, response_length=len(str(e)),
                exception=e)
        
        print("[%s] [%s]: Error, %s." % (datetime.datetime.now().strftime(datetime_format), client_id, str(e)))
        print("[%s] [%s]: Error, %s." % (datetime.datetime.now().strftime(datetime_format), client_id, traceback.format_exc()))

        return ""
# -----------------------------------------------------------


setup_logging("INFO", None)

# is_print_log = {IsPrintLog}
is_print_log = False
# host = "{Address}"
host = "http://52.188.14.158:8082"
# port = {Port}
# port = NameOnceJsonData(port_list)
port = 8082
# address
address = None
# client_id
# client_id = "{SlaveName}"
client_id = "client_id"
# case_id
# case_id = "{CaseID}"
case_id = "ce514456-8da9-432f-8999-1010fa94a83a"
# # 数据包头
package_start = ""
# # 数据包尾
# package_end = "{ResponseSeparator}"
package_end = ""
# API地址
# case_service_base_address = "{CaseServiceBaseAddress}"
case_service_base_address = "http://52.188.14.158:8082/"
# 同步类型
# sync_type = {SyncType}
sync_type = True

# 每个用户每次Task之间的等待时间，单位：秒
min_wait = 0.001
max_wait = 0.001
# 时间格式
datetime_format = "%Y-%m-%d %H:%M:%S.%f"
# revc数据大小
buff_size = 10240
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
is_save_interval = 1
# 发送的数据包是否加密
is_security_data = False
# 是否取Master的QPS
is_qps_master = True
# 时间单位，秒=1，毫秒=1000，微妙=1000000，纳秒=1000000000
second_unit = 1000000
# FireEventRequest request_type
request_type = "http"

# default is 2 seconds
all_locusts_spawned = Semaphore()
lock = threading.Lock()
# locust.runners.WORKER_REPORT_INTERVAL = is_save_interval


# -----------------------------------------------------------
# {$datavardeclareinit(0)}
user_account_list = [
    {
        "SlaveName": "client_id",
        "UserName": "zhangsan",
        "Password": "123456"
    },
    {
        "SlaveName": "client_id",
        "UserName": "lisi",
        "Password": "123456"
    },
    {
        "SlaveName": "Master",
        "UserName": "zhangsan",
        "Password": "123456"
    },
    {
        "SlaveName": "slave1-0",
        "UserName": "zhangsan",
        "Password": "123456"
    },
    {
        "SlaveName": "slave1-1",
        "UserName": "lisi",
        "Password": "123456"
    },
    {
        "SlaveName": "slave2-0",
        "UserName": "wangwu",
        "Password": "123456"
    },
    {
        "SlaveName": "slave2-1",
        "UserName": "zhaoliu",
        "Password": "123456"
    }
]

json_user_account_list = FilterJsonData(user_account_list, "SlaveName", client_id)

user_parameter_list = [
    {
        "UserName": "zhangsan",
        "Parameter": "zhangsanzhangsanzhangsanzhangsan",
        "Parameter2": "zhangsanzhangsanzhangsanzhangsan"
    },
    {
        "UserName": "zhangsan",
        "Parameter": "1111111111",
        "Parameter2": "111111111"
    },
    {
        "UserName": "zhangsan",
        "Parameter": "2222222222222",
        "Parameter2": "222222222222"
    },
    {
        "UserName": "zhangsan",
        "Parameter": "3333333333",
        "Parameter2": "3333333333"
    },
    {
        "UserName": "zhangsan",
        "Parameter": "4444444444444",
        "Parameter2": "44444444444444"
    },
    {
        "UserName": "lisi",
        "Parameter": "lisilisilisilisilisilisi",
        "Parameter2": "lisilisilisilisilisilisi"
    }
]

host_list = [
    "http://52.188.14.158:8082",
    "http://52.188.14.158:8082"
]

host = GetJsonRowData(host_list)

# -----------------------------------------------------------


class HttpTestUser(FastHttpUser):
    wait_time = between(min_wait, max_wait)
    # 连接的Http服务的地址
    host = host
    # 连接的Http服务的端口
    port = port
    address = None
    request_body = ""
    worker_report_time = datetime.datetime.now()
    environment = None
    is_login = False
    is_need_login = True

    user_id = ""
    user_name = ""
    user_password = ""
    user_token = ""
    senddata = ""
    recvdata = ""
    is_success = False

    # 自定义变量集合
    currconnectkv = {
        "user_id": {
            "name": "value"
        }
    }

    currconnectkv_user_id = {}

    def __init__(self, *args, **kwargs):
        # print("__init__")
        super(HttpTestUser, self).__init__(*args, **kwargs)
        HttpTestUser.environment = self.environment

    def login(self):
        self.user_id = uuid.uuid1()
        self.currconnectkv[self.user_id] = {}
        self.currconnectkv_user_id = self.currconnectkv[self.user_id]
        self.user_name = ""
        self.user_password = ""
        self.user_token = ""
        self.senddata = ""
        self.recvdata = ""
        self.is_success = False

        try:
            #--------------------------------------------------        
            # {$connectinit(12)}
            # pass
            # json_user_account = NameOnceJsonData(json_user_account_list)
            json_user_account = GetJsonRowData(json_user_account_list)
            self.currconnectkv_user_id["user_name"] = (json_user_account["UserName"] if json_user_account else None)
            self.currconnectkv_user_id["user_password"] = (json_user_account["Password"] if json_user_account else None)
            json_user_parameter_list = FilterJsonData(user_parameter_list, "UserName", self.currconnectkv_user_id["user_name"])
            json_user_parameter_list = SplitJsonData(user_parameter_list, 1)
            self.currconnectkv_user_id["user_parameter"] = GetJsonRowData(json_user_parameter_list)
            parameter = (self.currconnectkv_user_id["user_parameter"]["Parameter"] if self.currconnectkv_user_id["user_parameter"] else None)
            self.recvdata = HttpGetWithConnect(self.client, "/weatherforecast", "", ".*")
            self.currconnectkv_user_id["user_token"] = self.recvdata
            self.user_name = self.currconnectkv_user_id["user_name"]
            self.user_password = self.currconnectkv_user_id["user_password"]
            self.user_token = self.currconnectkv_user_id["user_token"]
            self.is_success = len(self.recvdata) > 0 and self.user_name
            #--------------------------------------------------
        except Exception as e:
            print("[%s] [%s] [%s]: Error, %s." % (datetime.datetime.now().strftime(datetime_format), client_id, self.user_name, str(e)))
            print("[%s] [%s] [%s]: Error, %s." % (datetime.datetime.now().strftime(datetime_format), client_id, self.user_name, traceback.format_exc()))

        if self.is_success:
            self.is_success = True
        # elif self.recvdata and self.user_name:
        #     self.is_success = True
        else:
            self.is_success = False

        if self.is_success:
            print("[%s] [%s] [%s]: Login Success." % (datetime.datetime.now().strftime(datetime_format), client_id, self.user_name))
            
            if self.senddata:
                print("[%s] [%s] [%s]: SendData, %s." % (datetime.datetime.now().strftime(datetime_format), client_id, self.user_name, self.senddata))

            if self.recvdata:
                print("[%s] [%s] [%s]: RecvData, %s." % (datetime.datetime.now().strftime(datetime_format), client_id, self.user_name, self.recvdata))
        elif not self.is_success:
            print("[%s] [%s] [%s]: Login Fail." % (datetime.datetime.now().strftime(datetime_format), client_id, self.user_name))
            print("[%s] [%s] [%s]: SendData, %s." % (datetime.datetime.now().strftime(datetime_format), client_id, self.user_name, self.senddata))
            print("[%s] [%s] [%s]: RecvData, %s." % (datetime.datetime.now().strftime(datetime_format), client_id, self.user_name, self.recvdata))

        return self.is_success

    def get_package(self):
        try:
            #--------------------------------------------------
            # self.request_body = {$requestbody()}
            # pass
            # self.request_body = "{\"UserName\": \"%s\", \"UserToken\": \"%s\", \"a\": \"a\"}" % (self.user_id, self.user_token)
            self.request_body = {"UserName": self.user_name, "UserToken": self.user_token, "a": "a"}
            #--------------------------------------------------
        except Exception as e:
            print("[%s] [%s] [%s]: Error, %s." % (datetime.datetime.now().strftime(datetime_format), client_id, self.user_name, str(e)))
            print("[%s] [%s] [%s]: Error, %s." % (datetime.datetime.now().strftime(datetime_format), client_id, self.user_name, traceback.format_exc()))

        self.senddata = self.request_body
        # package = DesSecurity(self.senddata, "abcdefghjhijklmn")
        # package = ed.encrypt(self.senddata)

        return package

    def send_data(self):
        self.senddata = ""
        self.recvdata = ""
        self.is_success = False

        try:
            #--------------------------------------------------
            # {$sendinit(12)}
            # pass

            # master_data = {}
            # master_data["CaseID"] = case_id
            # master_data["ConnectCount"] = str("0")
            # master_data["ConnectFailCount"] = str("0")
            # master_data["ReqCount"] = str("0")
            # master_data["ReqFailCount"] = str("0")
            # master_data["MaxDuration"] = str("0")
            # master_data["MinDurartion"] = str("0")
            # master_data["AvgDuration"] = str("0")

            # request_body = master_data
            # package = request_body
            # self.senddata = package
            # self.recvdata = HttpPostWithConnect(self.client, "/api/monitor/addmasterdata", "", self.senddata, ".*")
            self.recvdata = HttpGetWithConnect(self.client, "/weatherforecast", "", ".*", "name", self)
            self.is_success = len(self.recvdata) > 0
            # print("recvdata: %s" % self.recvdata)
            #--------------------------------------------------
        except Exception as e:
            print("[%s] [%s] [%s]: Error, %s." % (datetime.datetime.now().strftime(datetime_format), client_id, self.user_name, str(e)))
            print("[%s] [%s] [%s]: Error, %s." % (datetime.datetime.now().strftime(datetime_format), client_id, self.user_name, traceback.format_exc()))

        if self.is_success:
            self.is_success = True
        # elif self.recvdata == 0:
        #     self.is_success = True
        else:
            self.is_success = False

        if self.is_success and is_print_log:
            print("[%s] [%s] [%s]: Send Success." % (datetime.datetime.now().strftime(datetime_format), client_id, self.user_name))
            
            if self.senddata:
                print("[%s] [%s] [%s]: SendData, %s." % (datetime.datetime.now().strftime(datetime_format), client_id, self.user_name, self.senddata))

            if self.recvdata:
                print("[%s] [%s] [%s]: RecvData, %s." % (datetime.datetime.now().strftime(datetime_format), client_id, self.user_name, self.recvdata))
        elif not self.is_success:
            print("[%s] [%s] [%s]: Send Fail." % (datetime.datetime.now().strftime(datetime_format), client_id, self.user_name))
            print("[%s] [%s] [%s]: SendData, %s." % (datetime.datetime.now().strftime(datetime_format), client_id, self.user_name, self.senddata))
            print("[%s] [%s] [%s]: RecvData, %s." % (datetime.datetime.now().strftime(datetime_format), client_id, self.user_name, self.recvdata))

        return self.is_success

    def stop_data(self):
        self.senddata = ""
        self.recvdata = ""
        self.is_success = False

        try:
            #--------------------------------------------------
            # {$stopinit(12)}
            # pass

            master_data = {}
            master_data["CaseID"] = case_id
            master_data["ConnectCount"] = str("0")
            master_data["ConnectFailCount"] = str("0")
            master_data["ReqCount"] = str("0")
            master_data["ReqFailCount"] = str("0")
            master_data["MaxDuration"] = str("0")
            master_data["MinDurartion"] = str("0")
            master_data["AvgDuration"] = str("0")

            request_body = master_data
            package = request_body
            self.senddata = package
            headers = {"Content-Type": "application/json"}
            self.recvdata = HttpPostWithConnect(self.client, "/api/monitor/addmasterdata", headers, self.senddata, ".*")
            self.is_success = len(self.recvdata) == 0
            #--------------------------------------------------
        except Exception as e:
            print("[%s] [%s] [%s]: Error, %s." % (datetime.datetime.now().strftime(datetime_format), client_id, self.user_name, str(e)))
            print("[%s] [%s] [%s]: Error, %s." % (datetime.datetime.now().strftime(datetime_format), client_id, self.user_name, traceback.format_exc()))

        if self.is_success:
            self.is_success = True
        # elif self.recvdata == 0:
        #     self.is_success = True
        else:
            self.is_success = False

        if self.is_success:
            print("[%s] [%s] [%s]: Stop Success." % (datetime.datetime.now().strftime(datetime_format), client_id, self.user_name))
            
            if self.senddata:
                print("[%s] [%s] [%s]: SendData, %s." % (datetime.datetime.now().strftime(datetime_format), client_id, self.user_name, self.senddata))

            if self.recvdata:
                print("[%s] [%s] [%s]: RecvData, %s." % (datetime.datetime.now().strftime(datetime_format), client_id, self.user_name, self.recvdata))
        elif not self.is_success:
            print("[%s] [%s] [%s]: Stop Fail." % (datetime.datetime.now().strftime(datetime_format), client_id, self.user_name))
            print("[%s] [%s] [%s]: SendData, %s." % (datetime.datetime.now().strftime(datetime_format), client_id, self.user_name, self.senddata))
            print("[%s] [%s] [%s]: RecvData, %s." % (datetime.datetime.now().strftime(datetime_format), client_id, self.user_name, self.recvdata))

        return self.is_success

    MaxQPS = 0.0
    MinQPS = 0.0
    AvgQPS = 0.0

    def setQPS():
        # print("setQPS")

        QPS = 0

        try:
            if ("send_data", request_type) in HttpTestUser.environment.runner.stats.entries:
                stats = HttpTestUser.environment.runner.stats
                stats_send = stats.entries[("send_data", request_type)]

                if stats_send:
                    QPS = stats_send.current_rps

        except Exception as e:
            print("[%s] [%s]: Error, %s." % (datetime.datetime.now().strftime(datetime_format), client_id, str(e)))
            print("[%s] [%s]: Error, %s." % (datetime.datetime.now().strftime(datetime_format), client_id, traceback.format_exc()))

        if HttpTestUser.MaxQPS == 0.0:
            HttpTestUser.MaxQPS = QPS

        if HttpTestUser.MinQPS == 0.0:
            HttpTestUser.MinQPS = QPS

        if HttpTestUser.AvgQPS == 0.0:
            HttpTestUser.AvgQPS = QPS

        if HttpTestUser.MaxQPS < QPS:
            HttpTestUser.MaxQPS = QPS

        if HttpTestUser.MinQPS > QPS:
            HttpTestUser.MinQPS = QPS

        if HttpTestUser.AvgQPS > 0.0:
            HttpTestUser.AvgQPS = (HttpTestUser.AvgQPS + QPS) / 2

        # print("MaxQPS: %.2f, MinQPS: %.2f, AvgQPS: %.2f" % (HttpTestUser.MaxQPS, HttpTestUser.MinQPS, HttpTestUser.AvgQPS))

    def save_data():
        # print("save_data")

        if HttpTestUser.environment and HttpTestUser.environment.runner and HttpTestUser.environment.runner.state == locust.runners.STATE_RUNNING:
            if not is_qps_master:
                HttpTestUser.add_worker_data()

    def save_data_thread():
        # print("save_data_thread")

        while HttpTestUser.environment is None or HttpTestUser.environment.runner is None or HttpTestUser.environment.runner.state != locust.runners.STATE_RUNNING:
            time.sleep(1)

        print("[%s] [%s]: Locust Runner State: %s." % (datetime.datetime.now().strftime(datetime_format), client_id, HttpTestUser.environment.runner.state))

        if HttpTestUser.environment.parsed_options.master is False and HttpTestUser.environment.parsed_options.worker is False:
            # 单节点
            while HttpTestUser.environment.runner.state == locust.runners.STATE_RUNNING:
                HttpTestUser.add_master_data()
                HttpTestUser.add_worker_data()
                HttpTestUser.setQPS()
                time.sleep(is_save_interval)
        elif HttpTestUser.environment.parsed_options.master is True and HttpTestUser.environment.parsed_options.worker is False:
            # Master
            while HttpTestUser.environment.runner.state == locust.runners.STATE_RUNNING:
                HttpTestUser.add_master_data()

                if is_qps_master:
                    HttpTestUser.add_worker_data()

                HttpTestUser.setQPS()
                time.sleep(is_save_interval)
        elif HttpTestUser.environment.parsed_options.master is False and HttpTestUser.environment.parsed_options.worker is True:
            # Work
            while HttpTestUser.environment.runner.state == locust.runners.STATE_RUNNING:
                if not is_qps_master:
                    HttpTestUser.add_worker_data()

                time.sleep(is_save_interval)

        print("[%s] [%s]: Locust Runner State: %s." % (datetime.datetime.now().strftime(datetime_format), client_id, HttpTestUser.environment.runner.state))        

    def add_master_data():
        # print("add_master_data")

        try:
            if ("send_data", request_type) in HttpTestUser.environment.runner.stats.entries:
                stats = HttpTestUser.environment.runner.stats
                stats_send = stats.entries[("send_data", request_type)]

                if stats_send:
                    master_data = {}
                    master_data["CaseID"] = case_id
                    master_data["ConnectCount"] = str(HttpTestUser.environment.parsed_options.num_users if HttpTestUser.environment.parsed_options.num_users else 0)
                    master_data["ConnectFailCount"] = str(0)
                    master_data["ReqCount"] = str(stats_send.num_requests)
                    master_data["ReqFailCount"] = str(stats_send.num_failures)
                    master_data["MaxDuration"] = str(stats_send.max_response_time)
                    master_data["MinDurartion"] = str(stats_send.min_response_time)
                    master_data["AvgDuration"] = str(stats_send.avg_response_time)

                    Print("add_master_data, %s" % master_data)
                    HttpTestUser.post_api("api/monitor/addmasterdata", master_data)
            elif ("connect", request_type) in TcpTestUser.environment.runner.stats.entries:
                stats = TcpTestUser.environment.runner.stats
                stats_connect = stats.entries[("connect", request_type)]

                if stats_connect:
                    master_data = {}
                    master_data["CaseID"] = case_id
                    master_data["ConnectCount"] = str(stats_connect.num_requests)
                    master_data["ConnectFailCount"] = str(stats_connect.num_failures)
                    master_data["ReqCount"] = str(stats_connect.num_requests)
                    master_data["ReqFailCount"] = str(stats_connect.num_failures)
                    master_data["MaxDuration"] = str(stats_connect.max_response_time)
                    master_data["MinDurartion"] = str(stats_connect.min_response_time)
                    master_data["AvgDuration"] = str(stats_connect.avg_response_time)

                    Print("add_master_data, %s" % master_data)
                    TcpTestUser.post_api("api/monitor/addmasterdata", master_data)                    
        except Exception as e:
            print("[%s] [%s]: Error, %s." % (datetime.datetime.now().strftime(datetime_format), client_id, str(e)))
            print("[%s] [%s]: Error, %s." % (datetime.datetime.now().strftime(datetime_format), client_id, traceback.format_exc()))

    def add_worker_data():
        # print("add_worker_data")

        try:
            if ("send_data", request_type) in HttpTestUser.environment.runner.stats.entries:
                stats = HttpTestUser.environment.runner.stats
                stats_send = stats.entries[("send_data", request_type)]

                if stats_send:
                    worker_data_data = {}
                    worker_data_data["CaseID"] = case_id
                    worker_data_data["SlaveID"] = client_id
                    worker_data_data["QPS"] = str(stats_send.current_rps)
                    worker_data_data["Time"] = datetime.datetime.utcnow().strftime("%Y%m%d%H%M%S")

                    worker_data = []
                    worker_data.append(worker_data_data)

                    Print("add_worker_data, %s" % worker_data)

                    if stats_send.current_rps > 0.0:
                        HttpTestUser.post_api("api/monitor/addslavedata", worker_data)
        except Exception as e:
            print("[%s] [%s]: Error, %s." % (datetime.datetime.now().strftime(datetime_format), client_id, str(e)))
            print("[%s] [%s]: Error, %s." % (datetime.datetime.now().strftime(datetime_format), client_id, traceback.format_exc()))

    def add_history_data():
        print("add_history_data")

        try:
            if ("send_data", request_type) in HttpTestUser.environment.runner.stats.entries:
                stats = HttpTestUser.environment.runner.stats
                stats_send = stats.entries[("send_data", request_type)]

                if stats_send:
                    history_data = {}
                    history_data["CaseID"] = case_id
                    history_data["ConnectCount"] = (HttpTestUser.environment.parsed_options.num_users if HttpTestUser.environment.parsed_options.num_users else 0)
                    history_data["ConnectFailCount"] = 0
                    history_data["ReqCount"] = stats_send.num_requests
                    history_data["ReqFailCount"] = stats_send.num_failures
                    history_data["MaxQPS"] = HttpTestUser.MaxQPS
                    history_data["MinQPS"] = HttpTestUser.MinQPS
                    history_data["AvgQPS"] = HttpTestUser.AvgQPS
                    history_data["MaxDuration"] = stats_send.max_response_time
                    history_data["MinDurartion"] = stats_send.min_response_time
                    history_data["AvgDuration"] = stats_send.avg_response_time

                    Print("add_history_data, %s" % history_data)
                    HttpTestUser.post_api("api/report/addhistory", history_data)
            else:
                history_data = {}
                history_data["CaseID"] = case_id
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

                Print("add_history_data, %s" % history_data)
                HttpTestUser.post_api("api/report/addhistory", history_data)
        except Exception as e:
            print("[%s] [%s]: Error, %s." % (datetime.datetime.now().strftime(datetime_format), client_id, str(e)))
            print("[%s] [%s]: Error, %s." % (datetime.datetime.now().strftime(datetime_format), client_id, traceback.format_exc()))

            history_data = {}
            history_data["CaseID"] = case_id
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

            Print("add_history_data, %s" % history_data)
            HttpTestUser.post_api("api/report/addhistory", history_data)

    def post_api(path, data):
        # print("post_api")
        # print(data)

        try:
            url = "%s%s" % (case_service_base_address, path)
            headers = {
                "Content-Type": "application/json"
            }
            response = requests.post(url, json=data, headers=headers)
            # print("Url: %s" % url)
            # print("response: %s" % response.text)

            if response.status_code == 200:
                result = response.text

                Print("Http Post Success, Url, %s, StatusCode, %s, Reason, %s, Text, %s" % (url, response.status_code, response.reason, response.text))
            else:
                print("[%s] [%s]: Http Post Fail, Url, %s, StatusCode, %s, Reason, %s, Text, %s." % (datetime.datetime.now().strftime(datetime_format), client_id, url, response.status_code, response.reason, response.text))

                return ""

        except Exception as e:
            print("[%s] [%s]: Error, %s." % (datetime.datetime.now().strftime(datetime_format), client_id, str(e)))
            print("[%s] [%s]: Error, %s." % (datetime.datetime.now().strftime(datetime_format), client_id, traceback.format_exc()))

            return ""

    def quitting():
        # print("quitting")

        if HttpTestUser.environment and HttpTestUser.environment.parsed_options:
            if HttpTestUser.environment.parsed_options.master is False and HttpTestUser.environment.parsed_options.worker is False:
                # 单节点
                HttpTestUser.add_history_data()
            elif HttpTestUser.environment.parsed_options.master is True and HttpTestUser.environment.parsed_options.worker is False:
                # Master
                HttpTestUser.add_history_data()
            elif HttpTestUser.environment.parsed_options.master is False and HttpTestUser.environment.parsed_options.worker is True:
                # Work
                pass     

    @events.hatch_complete.add_listener
    def on_hatch_complete(**kwargs):
        time.sleep(1)
        # print("on_hatch_complete: %s" % kwargs["user_count"])        

        if is_hatch_complete_run:
            if not all_locusts_spawned.ready():
                all_locusts_spawned.release()

    @events.worker_report.add_listener
    def on_worker_report(**kwargs):
        # print("on_worker_report")
        pass 

    @events.report_to_master.add_listener
    def on_report_to_master(**kwargs):
        # print("on_report_to_master")
        HttpTestUser.save_data()

    @events.test_start.add_listener
    def on_test_start(**kwargs):
        # print("on_test_start")

        if is_hatch_complete_run:
            if all_locusts_spawned.ready():
                all_locusts_spawned.acquire()

        HttpTestUser.environment = kwargs["environment"]

        # 统计汇报线程
        t = threading.Thread(target=HttpTestUser.save_data_thread)
        t.setDaemon(True)
        t.start()

    @events.test_stop.add_listener
    def on_test_stop(**kwargs):
        # print("on_test_stop")

        if is_hatch_complete_run:
            if not all_locusts_spawned.ready():
                all_locusts_spawned.release()

    @events.quitting.add_listener
    def on_quitting(**kwargs):
        # print("on_quitting")
        HttpTestUser.quitting()

    def on_start(self):
        # print("on_start")

        if is_hatch_complete_run:
            lock.acquire()

            if all_locusts_spawned.ready():
                all_locusts_spawned.acquire()

            lock.release()

        self.is_login = self.login()        
        # is_success = self.send_data()
        # is_success = self.stop_data()

        if is_hatch_complete_run:
            all_locusts_spawned.wait()

    def on_stop(self):
        # print("on_stop")

        if is_hatch_complete_run:
            lock.acquire()

            if not all_locusts_spawned.ready():
                all_locusts_spawned.release()

            lock.release()

        is_success = self.stop_data()
        self.is_login = False
        self.is_need_login = True

        self.currconnectkv[self.user_id] = {}
        self.currconnectkv_user_id = {}
        self.user_id = ""
        self.user_name = ""
        self.user_password = ""
        self.user_token = ""
        self.senddata = ""
        self.recvdata = ""
        self.is_success = False

    @task(1)
    def send(self):
        if self.is_login:
            start_time = time.time()

            try:
                is_success = self.send_data()
            except Exception as e:
                print("[%s] [%s]: Error, %s." % (datetime.datetime.now().strftime(datetime_format), client_id, str(e)))
                print("[%s] [%s]: Error, %s." % (datetime.datetime.now().strftime(datetime_format), client_id, traceback.format_exc()))
                total_time = int((time.time() - start_time) * second_unit)
                self.environment.events.request_failure.fire(
                    request_type=request_type, name="send_data",
                    response_time=total_time, response_length=len(str(e)),
                    exception=e)
            else:
                total_time = int((time.time() - start_time) * second_unit)
                self.environment.events.request_success.fire(
                    request_type=request_type, name="send_data",
                    response_time=total_time, response_length=0)
        elif self.is_need_login:
            self.is_login = self.login()
            self.is_need_login = False


# if __name__ == "__main__":
#     env = Environment(user_classes=[HttpTestUser])
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
        "locust -f .\\locust-tcp\\locustfiles\\HttpClient.py")

# locust -f "E:\Documents\Visual Studio Code\TestPython\locust-tcp\locustfiles\HttpClient.py"
# locust -f "E:\Documents\Visual Studio Code\TestPython\locust-tcp\locustfiles\HttpClient.py" --master --master-bind-host 127.0.0.1 --master-bind-port 5557 --expect-workers 1
# locust -f "E:\Documents\Visual Studio Code\TestPython\locust-tcp\locustfiles\HttpClient.py" --worker --master-host 127.0.0.1 --master-port 5557
# locust -f "E:\Documents\Visual Studio Code\TestPython\locust-tcp\locustfiles\HttpClient.py" --headless -u 100 -r 10 -t 1m 
# locust -f "E:\Documents\Visual Studio Code\TestPython\locust-tcp\locustfiles\HttpClient.py" --master --headless --expect-workers 1 -u 100 -r 10 -t 1m  
# locust -f "E:\Documents\Visual Studio Code\TestPython\locust-tcp\locustfiles\HttpClient.py" --worker --master-host 127.0.0.1 --master-port 5557
# locust -f "E:\Documents\Visual Studio Code\TestPython\locust-tcp\locustfiles\HttpClient.py" --logfile "E:\Documents\Visual Studio Code\TestPython\locust-tcp\log\log.log"
# locust -f "E:\Documents\Visual Studio Code\TestPython\locust-tcp\locustfiles\HttpClient.py" --logfile "E:\Documents\Visual Studio Code\TestPython\locust-tcp\log\log.log" --master --headless -u 100 -r 10 -t 1m
# locust -f "E:\Documents\Visual Studio Code\TestPython\locust-tcp\locustfiles\HttpClient.py" --worker --master-host=127.0.0.1 --master-port=5557
