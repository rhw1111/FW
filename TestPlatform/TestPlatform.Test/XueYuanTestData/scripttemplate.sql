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
import gevent
from gevent._semaphore import Semaphore
import locust.stats
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


host = "{Address}"
port = {Port}
# client_id
client_id = "{SlaveName}"
# case_id
case_id = "{CaseID}"
# # 数据包头
package_start = "<package>"
# # 数据包尾
package_end = "{ResponseSeparator}"
# API地址
case_service_base_address = "{CaseServiceBaseAddress}"

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
is_save_interval = 1
# 发送的数据包是否加密
is_security_data = True
# 是否接收数据
is_recv = False
# 秘钥，密钥的长度必须是16的倍数
key = "abcdefghijklmnop"
# 时间单位，秒=1，毫秒=1000，微妙=1000000，纳秒=1000000000
second_unit = 1000000000

# default is 2 seconds
all_locusts_spawned = Semaphore()
lock = threading.Lock()


{$additionfunc(0)}


{$datavardeclareinit(0)}


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
            total_time = int((time.time() - start_time) * second_unit)
            self._locust_environment.events.request_failure.fire(
                request_type="tcpsocket", name="connect",
                response_time=total_time, response_length=len(str(e)),
                exception=e)

            return False
        else:
            total_time = int((time.time() - start_time) * second_unit)
            self._locust_environment.events.request_success.fire(
                request_type="tcpsocket", name="connect",
                response_time=total_time, response_length=0)

            return True

    def send(self, msg):
        start_time = time.time()

        try:
            super(TcpSocketClient, self).send(msg.encode())
        except Exception as e:
            total_time = int((time.time() - start_time) * second_unit)
            self._locust_environment.events.request_failure.fire(
                request_type="tcpsocket", name="send",
                response_time=total_time, response_length=len(str(e)),
                exception=e)

            return False
        else:
            total_time = int((time.time() - start_time) * second_unit)
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
            total_time = int((time.time() - start_time) * second_unit)
            self._locust_environment.events.request_failure.fire(
                request_type="tcpsocket", name="recv",
                response_time=total_time, response_length=len(str(e)),
                exception=e)
        else:
            total_time = int((time.time() - start_time) * second_unit)
            self._locust_environment.events.request_success.fire(
                request_type="tcpsocket", name="recv",
                response_time=total_time, response_length=0)

        return recv_data


class TcpTestUser(User):
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
    is_login = False
    environment = None

    # 自定义变量集合
    currconnectkv = {
        "name": ""
    }

    def __init__(self, *args, **kwargs):
        # print("__init__")
        super(TcpTestUser, self).__init__(*args, **kwargs)
        self.client = TcpSocketClient(socket.AF_INET, socket.SOCK_STREAM)
        self.client._locust_environment = self.environment
        TcpTestUser.environment = self.environment

    def get_package(self):
        self.request_body = {$senddata()}
        self.senddata = self.request_body
        package = self.senddata

        if type(package) != str:
            package = str(self.senddata)

        {$sendinit(8)}

        return package

    def connect(self):
        is_success = self.client.connect(self.ADDR)

        if is_success is False:
            print("Client: connect fail, %s:%s" % self.ADDR)
            return is_success

        print("Client: connect success, %s:%s" % self.ADDR)
        return is_success

    def login(self):
        # {$connectinit(8)}

        if self.user_token is None or self.user_token == "":
            is_success = False
        else:
            is_success = True

        if is_success is False:
            print("Client: login fail, %s: %s" % (self.user_id, self.user_password))
            return is_success

        print("Client: login success, %s: %s" % (self.user_id, self.user_password))
        return is_success

    def send_data(self, package):
        is_success = self.client.send(package)

        if is_success is False:
            print("Client: send fail, %s" % package)
            return is_success

        # print("Client: send success, %s" % package)
        return is_success

    def recv_data_thread_nothing(self):
        while True:
            try:
                data = self.client.recv(buff_size)

                if data == "exit" or not data:
                    # print("Not Data: %s" % data)
                    break

                # print("Data: %s" % data)

            except Exception as e:
                print(str(e))
                break

        if is_recv_error_close:
            self.client.close()
            print("Connection closed.")

    MaxQPS = 0.0
    MinQPS = 0.0
    AvgQPS = 0.0

    def setQPS(QPS):
        # print("setQPS")

        if TcpTestUser.MaxQPS == 0.0:
            TcpTestUser.MaxQPS = QPS

        if TcpTestUser.MinQPS == 0.0:
            TcpTestUser.MinQPS = QPS

        if TcpTestUser.AvgQPS == 0.0:
            TcpTestUser.AvgQPS = QPS

        if TcpTestUser.MaxQPS < QPS:
            TcpTestUser.MaxQPS = QPS

        if TcpTestUser.MinQPS > QPS:
            TcpTestUser.MinQPS = QPS

        if TcpTestUser.AvgQPS > 0.0:
            TcpTestUser.AvgQPS = (TcpTestUser.AvgQPS + QPS) / 2

        # print("MaxQPS: %.2f, MinQPS: %.2f, AvgQPS: %.2f" % (TcpTestUser.MaxQPS, TcpTestUser.MinQPS, TcpTestUser.AvgQPS))

    def save_data_thread():
        # print("save_data_thread")

        while TcpTestUser.environment is None or TcpTestUser.environment.runner.state != locust.runners.STATE_RUNNING:
            time.sleep(1)

        print("Locust Runner State: %s" % TcpTestUser.environment.runner.state)

        if TcpTestUser.environment.parsed_options.worker is False:
            # Master
            while TcpTestUser.environment.runner.state == locust.runners.STATE_RUNNING:
                TcpTestUser.add_master_data()
                TcpTestUser.add_worker_data()
                time.sleep(is_save_interval)

            print("Locust Runner State: %s" % TcpTestUser.environment.runner.state)
        else:
            # Work
            pass        

    def add_master_data():
        # print("add_master_data")
        stats = TcpTestUser.environment.runner.stats
        stats_connect = stats.entries[("connect", "tcpsocket")]
        stats_send = stats.entries[("send", "tcpsocket")]

        if stats_connect is not None and stats_send is not None:
            master_data = {}
            master_data["CaseID"] = case_id
            master_data["ConnectCount"] = str(stats_connect.num_requests)
            master_data["ConnectFailCount"] = str(stats_connect.num_failures)
            master_data["ReqCount"] = str(stats_send.num_requests)
            master_data["ReqFailCount"] = str(stats_send.num_failures)
            master_data["MaxDuration"] = str(stats_send.max_response_time)
            master_data["MinDurartion"] = str(stats_send.min_response_time)
            master_data["AvgDuration"] = str(stats_send.avg_response_time)

            # print(master_data)
            TcpTestUser.post_api("api/monitor/addmasterdata", master_data)

    def add_worker_data():
        # print("add_worker_data")
        stats = TcpTestUser.environment.runner.stats
        stats_send = stats.entries[("send", "tcpsocket")]

        if stats_send is not None:
            worker_data_data = {}
            worker_data_data["CaseID"] = case_id
            worker_data_data["SlaveID"] = client_id
            worker_data_data["QPS"] = str(stats_send.current_rps)
            worker_data_data["Time"] = datetime.datetime.utcnow().strftime("%Y%m%d%H%M%S")

            worker_data = []
            worker_data.append(worker_data_data)

            # print(worker_data)
            TcpTestUser.post_api("api/monitor/addslavedata", worker_data)

            TcpTestUser.setQPS(stats_send.current_rps)

    def add_history_data():
        # print("add_history_data")
        stats = TcpTestUser.environment.runner.stats
        stats_connect = stats.entries[("connect", "tcpsocket")]
        stats_send = stats.entries[("send", "tcpsocket")]

        if stats_connect is not None and stats_send is not None:
            history_data = {}
            history_data["CaseID"] = case_id
            history_data["ConnectCount"] = stats_connect.num_requests
            history_data["ConnectFailCount"] = stats_connect.num_failures
            history_data["ReqCount"] = stats_send.num_requests
            history_data["ReqFailCount"] = stats_send.num_failures
            history_data["MaxQPS"] = TcpTestUser.MaxQPS
            history_data["MinQPS"] = TcpTestUser.MinQPS
            history_data["AvgQPS"] = TcpTestUser.AvgQPS
            history_data["MaxDuration"] = stats_send.max_response_time
            history_data["MinDurartion"] = stats_send.min_response_time
            history_data["AvgDuration"] = stats_send.avg_response_time

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
        # print("quitting")
        TcpTestUser.add_history_data()

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
            if all_locusts_spawned.ready():
                all_locusts_spawned.acquire()

        TcpTestUser.environment = kwargs["environment"]

        # 统计汇报线程
        t = threading.Thread(target=TcpTestUser.save_data_thread)
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
        TcpTestUser.quitting()

    def on_start(self):
        # print("on_start")

        if is_hatch_complete_run:
            lock.acquire()

            if all_locusts_spawned.ready():
                all_locusts_spawned.acquire()

            lock.release()

        is_success = self.connect()

        if is_recv:
            # 接收线程
            t = threading.Thread(target=self.recv_data_thread_nothing)
            t.setDaemon(True)
            t.start()

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
        self.is_login = False

    @task(1)
    def send(self):
        if self.is_login:
            package = self.get_package()
            send_is_success = self.send_data(package)
        else:
            self.is_login = self.login()
'
WHERE id = '9adb8033-f28a-43a1-b396-0f36307b213b';






