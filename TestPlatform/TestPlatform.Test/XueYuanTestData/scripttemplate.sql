SELECT * FROM tpmain.scripttemplate;

INSERT INTO tpmain.scripttemplate
VALUES('9adb8033-f28a-43a1-b396-0f36307b213b', 'LocustTcp', '', now(), now(), '1');

UPDATE tpmain.scripttemplate
SET content = '# !/usr/bin/env python3
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

import sys
import os
sys.path.append(os.path.dirname(os.path.dirname(__file__)))
# from config import config
# from case import case
# from api import api
# from common import common

setup_logging("INFO", None)

# host = "127.0.0.1"
host = "{Address}"
# port = 12345
port = {Port}
# client_id
# client_id = "client_id"
client_id = "{SlaveName}"
# case_id
# case_id = "case_id"
case_id = "{CaseName}"
# # 数据包头
package_start = "<package>"
# # 数据包尾
# package_end = "</package>"
package_end = "{ResponseSeparator}"

# 每个用户每次Task之间的等待时间，单位：秒
min_wait = 1
max_wait = 1
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


{$additionfunc()}


{$datavardeclareinit()}


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
    def __init__(self, af_inet, socket_type):
        super(TcpSocketClient, self).__init__(af_inet, socket_type)

    def connect(self, addr):
        start_time = time.time()

        try:
            super(TcpSocketClient, self).connect(addr)
        except Exception as e:
            total_time = int((time.time() - start_time) * 1000)
            events.request_failure.fire(
                request_type="tcpsocket", name="connect",
                response_time=total_time, response_length=len(str(e)),
                exception=e)

            return False
        else:
            total_time = int((time.time() - start_time) * 1000)
            events.request_success.fire(
                request_type="tcpsocket", name="connect",
                response_time=total_time, response_length=0)

            return True

    def send(self, msg):
        start_time = time.time()

        try:
            super(TcpSocketClient, self).send(msg.encode())
        except Exception as e:
            total_time = int((time.time() - start_time) * 1000)
            events.request_failure.fire(
                request_type="tcpsocket", name="send",
                response_time=total_time, response_length=len(str(e)),
                exception=e)

            return False
        else:
            total_time = int((time.time() - start_time) * 1000)
            events.request_success.fire(
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
            events.request_failure.fire(
                request_type="tcpsocket", name="recv",
                response_time=total_time, response_length=len(str(e)),
                exception=e)
        else:
            total_time = int((time.time() - start_time) * 1000)
            events.request_success.fire(
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
    user_token = ""
    worker_report_time = datetime.datetime.now()

    stats_tcps = {
        "stats_tcps": []
    }

    stats_tcps_current = {
        "stats_tcps_current": []
    }

    stats_tcps_totals = {
        "stats_tcps_totals": []
    }

    def __init__(self, *args, **kwargs):
        super(TcpTestUser, self).__init__(*args, **kwargs)
        self.client = TcpSocketClient(socket.AF_INET, socket.SOCK_STREAM)

    def get_package(self):
        {$sendinit()}

        # package = "123122123"
        package = {$senddata()}

        if is_security_data:
            package = ed.encrypt(package)

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

        self.client.close()
        print("Connection closed.")

    def reset_data(self):
        # print("reset_data")

        # 如果是单点模式，取定时清理stats_tcps，以免爆内存
        if self.environment.parsed_options.worker is False:
            if len(TcpTestUser.stats_tcps["stats_tcps"]) > 50000:
                lock.acquire()

                if TcpTestUser.is_worker_report_time():
                    TcpTestUser.create_stats_tcps_current_worker_report()

                    temp_stats_tcps_totals = TcpTestUser.stats_tcps_totals["stats_tcps_totals"]
                    TcpTestUser.create_stats_tcps_totals()
                    TcpTestUser.stats_tcps_totals["stats_tcps_totals"] = temp_stats_tcps_totals + TcpTestUser.stats_tcps_totals["stats_tcps_totals"]
                    TcpTestUser.create_stats_tcps_totals_worker_report()

                    if is_worker_report_save:
                        if len(TcpTestUser.stats_tcps["stats_tcps"]) > 0 and len(TcpTestUser.stats_tcps_totals["stats_tcps_totals"]) > 0:
                            # 在多节点分布式运行时，判断此为master, 汇总数据并提交API
                            TcpTestUser.save_data(TcpTestUser.stats_tcps, TcpTestUser.stats_tcps_current, TcpTestUser.stats_tcps_totals)                

                            TcpTestUser.reset_stats_tcps()
                            # TcpTestUser.reset_stats_tcps_current()
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

    def append_stats_tcps_current(client_id, case_id, name, start_time, end_time, run_time, max_time, min_time, avg_time, med_time, count_num, success_num, fail_num, count_s, success_s, fail_s):
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
        TcpTestUser.stats_tcps_current["stats_tcps_current"].append(stats_tcps_total)

    def append_stats_tcps_totals(client_id, case_id, name, start_time, end_time, run_time, max_time, min_time, avg_time, med_time, count_num, success_num, fail_num, count_s, success_s, fail_s):
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
        TcpTestUser.stats_tcps_totals["stats_tcps_totals"].append(stats_tcps_total)

    def reset_stats_tcps():
        TcpTestUser.stats_tcps = {
            "stats_tcps": []
        }

    def reset_stats_tcps_current():
        TcpTestUser.stats_tcps_current = {
            "stats_tcps_current": []
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

    def create_stats_tcps_current():
        if len(TcpTestUser.stats_tcps["stats_tcps"]) == 0:
            return False

        TcpTestUser.reset_stats_tcps_current()

        pandas_stats_tcps = pandas.read_json(json.dumps(TcpTestUser.stats_tcps["stats_tcps"]), "records")
        # print(pandas_stats_tcps)

        if len(pandas_stats_tcps) == 0:
            return False

        names = ["connect", "send",  "recv"]

        for name in names:
            pandas_stats_tcps_name = pandas_stats_tcps[pandas_stats_tcps["name"]==name]
            pandas_stats_tcps_name_true = pandas_stats_tcps_name[pandas_stats_tcps_name["is_success"]==True]
            pandas_stats_tcps_name_false = pandas_stats_tcps_name[pandas_stats_tcps_name["is_success"]==False]

            if pandas_stats_tcps_name["name"].count() > 0:
                run_time = pandas_stats_tcps_name["end_time"].max() - pandas_stats_tcps_name["start_time"].min()
                run_time = run_time.total_seconds()

                if run_time == 0:
                    run_time = 1

                TcpTestUser.append_stats_tcps_current(
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
                    float(pandas_stats_tcps_name["is_success"].count() / run_time),
                    float(pandas_stats_tcps_name_true["is_success"].count() / run_time),
                    float(pandas_stats_tcps_name_false["is_success"].count() / run_time))            

        # pandas_stats_tcps_current = pandas.read_json(json.dumps(TcpTestUser.stats_tcps_current["stats_tcps_current"]), "records")
        # print(pandas_stats_tcps_current)
        return True

    def create_stats_tcps_current_worker_report():
        if is_current is False:
            return True

        if len(TcpTestUser.stats_tcps["stats_tcps"]) == 0:
            return False

        stats_tcps_current = TcpTestUser.stats_tcps_current
        TcpTestUser.reset_stats_tcps_current()        

        # start_time = (TcpTestUser.worker_report_time + datetime.timedelta(seconds=0))
        # end_time = (start_time + datetime.timedelta(seconds=1))

        pandas_stats_tcps = pandas.read_json(json.dumps(TcpTestUser.stats_tcps["stats_tcps"]), "records")
        # pandas_stats_tcps = pandas_stats_tcps[(pandas_stats_tcps["start_time"]>=start_time) & (pandas_stats_tcps["start_time"]<=end_time)]
        # print(pandas_stats_tcps)

        if len(pandas_stats_tcps) == 0:
            return False

        names = ["connect", "send",  "recv"]

        for name in names:
            pandas_stats_tcps_name = pandas_stats_tcps[pandas_stats_tcps["name"]==name]
            pandas_stats_tcps_name_true = pandas_stats_tcps_name[pandas_stats_tcps_name["is_success"]==True]
            pandas_stats_tcps_name_false = pandas_stats_tcps_name[pandas_stats_tcps_name["is_success"]==False]

            if pandas_stats_tcps_name["name"].count() > 0:
                run_time = pandas_stats_tcps_name["end_time"].max() - pandas_stats_tcps_name["start_time"].min()
                run_time = run_time.total_seconds()

                if run_time == 0:
                    run_time = 1

                TcpTestUser.append_stats_tcps_current(
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
                    float(pandas_stats_tcps_name["is_success"].count() / run_time),
                    float(pandas_stats_tcps_name_true["is_success"].count() / run_time),
                    float(pandas_stats_tcps_name_false["is_success"].count() / run_time))            

        # pandas_stats_tcps_current = pandas.read_json(json.dumps(TcpTestUser.stats_tcps_current["stats_tcps_current"]), "records")
        # print(pandas_stats_tcps_current)

        return True

    def create_stats_tcps_totals():
        if len(TcpTestUser.stats_tcps["stats_tcps"]) == 0:
            return False

        TcpTestUser.reset_stats_tcps_totals()

        pandas_stats_tcps = pandas.read_json(json.dumps(TcpTestUser.stats_tcps["stats_tcps"]), "records")
        # print(pandas_stats_tcps)

        if len(pandas_stats_tcps) == 0:
            return False        

        names = ["connect", "send",  "recv"]

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
                    float(pandas_stats_tcps_name["is_success"].count() / run_time),
                    float(pandas_stats_tcps_name_true["is_success"].count() / run_time),
                    float(pandas_stats_tcps_name_false["is_success"].count() / run_time))            

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

        names = ["connect", "send",  "recv"]

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
                    float(pandas_stats_tcps_totals_name["count_num"].sum() / run_time),
                    float(pandas_stats_tcps_totals_name["success_num"].sum() / run_time),
                    float(pandas_stats_tcps_totals_name["fail_num"].sum() / run_time))        

        # pandas_stats_tcps_totals = pandas.read_json(json.dumps(TcpTestUser.stats_tcps_totals["stats_tcps_totals"]), "records")
        # print(pandas_stats_tcps_totals)
        return True

    def save_data(stats_tcps, stats_tcps_current, stats_tcps_totals):
        # print("save_data")

        pandas_stats_tcps = pandas.read_json(json.dumps(stats_tcps["stats_tcps"]), "records")
        print(pandas_stats_tcps)

        pandas_stats_tcps_current = pandas.read_json(json.dumps(stats_tcps_current["stats_tcps_current"]), "records")
        print(pandas_stats_tcps_current)

        pandas_stats_tcps_totals = pandas.read_json(json.dumps(stats_tcps_totals["stats_tcps_totals"]), "records")
        print(pandas_stats_tcps_totals)

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

        # stats_tcps_current = {
        #     "stats_tcps_current": [
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
        #             "fail/s": 0
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
        #             "fail/s": 0
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
        #             "fail/s": 0
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
        #             "fail/s": 0
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
        #             "fail/s": 0
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
        #             "fail/s": 0
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
        #                 "fail/s": 0
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
        #                 "fail/s": 0
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
        #                 "fail/s": 0
        #             }
        #         ]
        #     }
        # }

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
            # my_api = api.api()
            # my_api.save_data(TcpTestUser.stats_tcps, TcpTestUser.stats_tcps_totals)
            TcpTestUser.create_stats_tcps_current_worker_report()
            TcpTestUser.create_stats_tcps_totals_worker_report()
            TcpTestUser.save_data(TcpTestUser.stats_tcps, TcpTestUser.stats_tcps_current, TcpTestUser.stats_tcps_totals)
            TcpTestUser.reset_stats_tcps()
            TcpTestUser.reset_stats_tcps_current()
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
        # print("on_worker_report")

        # 在多节点分布式运行时，判断此为master, 汇总worker来的数据
        lock.acquire()
        TcpTestUser.stats_tcps["stats_tcps"] = TcpTestUser.stats_tcps["stats_tcps"] + kwargs["data"]["stats_tcps"]
        TcpTestUser.stats_tcps_totals["stats_tcps_totals"] = TcpTestUser.stats_tcps_totals["stats_tcps_totals"] + kwargs["data"]["stats_tcps_totals"]
        
        if TcpTestUser.is_worker_report_time():
            TcpTestUser.create_stats_tcps_current_worker_report()
            TcpTestUser.create_stats_tcps_totals_worker_report()

            if is_worker_report_save:
                if len(TcpTestUser.stats_tcps["stats_tcps"]) > 0 and len(TcpTestUser.stats_tcps_totals["stats_tcps_totals"]) > 0:
                    # 在多节点分布式运行时，判断此为master, 汇总数据并提交API
                    TcpTestUser.save_data(TcpTestUser.stats_tcps, TcpTestUser.stats_tcps_current, TcpTestUser.stats_tcps_totals)

                    TcpTestUser.reset_stats_tcps()
                    # TcpTestUser.reset_stats_tcps_current()
                    # TcpTestUser.reset_stats_tcps_totals()
        
        lock.release()

    @events.report_to_master.add_listener
    def on_report_to_master(**kwargs):
        # print("on_report_to_master")

        # 在多节点分布式运行时，判断此为worker, 汇总数据后提交master，并清空worker数据
        lock.acquire()
        kwargs["data"]["stats_tcps"] = TcpTestUser.stats_tcps["stats_tcps"]
        # TcpTestUser.create_stats_tcps_current()
        kwargs["data"]["stats_tcps_current"] = TcpTestUser.stats_tcps_current["stats_tcps_current"]
        TcpTestUser.create_stats_tcps_totals()
        kwargs["data"]["stats_tcps_totals"] = TcpTestUser.stats_tcps_totals["stats_tcps_totals"]
        TcpTestUser.reset_stats_tcps()
        TcpTestUser.reset_stats_tcps_current()
        TcpTestUser.reset_stats_tcps_totals()
        lock.release()

    @events.quitting.add_listener
    def on_quitting(**kwargs):
        # print("on_quitting")
        TcpTestUser.quitting()

    def on_start(self):
        {$connectinit()}

        # print("on_start")

        if is_hatch_complete_run:
            # print(all_locusts_spawned.ready())
            lock.acquire()

            if all_locusts_spawned.ready():
                all_locusts_spawned.acquire()

            lock.release()
            # print(all_locusts_spawned.ready())

        self.user_id = "user_id_%s" % str(random.randint(1, 10000))
        self.user_token = "user_token_%s" % str(random.randint(1, 10000))
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
'
WHERE id = '9adb8033-f28a-43a1-b396-0f36307b213b';






