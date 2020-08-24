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
{$additionfunc(0)}
# -----------------------------------------------------------


setup_logging("INFO", None)

is_print_log = {IsPrintLog}
host = "{Address}"
port = {Port}
# address
address = None
# client_id
client_id = "{SlaveName}"
# case_id
case_id = "{CaseID}"
# # 数据包头
package_start = ""
# # 数据包尾
package_end = "{ResponseSeparator}"
# API地址
case_service_base_address = "{CaseServiceBaseAddress}"
# 同步类型
sync_type = {SyncType}

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
{$datavardeclareinit(0)}
# -----------------------------------------------------------


class HttpTestUser(HttpUser):
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
            {$connectinit(12)}
            pass
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
            pass
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
            {$sendinit(12)}
            pass
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
            {$stopinit(12)}
            pass
            #--------------------------------------------------
        except Exception as e:
            print("[%s] [%s] [%s]: Error, %s." % (datetime.datetime.now().strftime(datetime_format), client_id, self.user_name, str(e)))
            print("[%s] [%s] [%s]: Error, %s." % (datetime.datetime.now().strftime(datetime_format), client_id, self.user_name, traceback.format_exc()))

        if self.is_success:
            self.is_success = True
        # elif self.recvdata:
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
