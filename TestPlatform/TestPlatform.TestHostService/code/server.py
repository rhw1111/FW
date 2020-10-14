#!/usr/bin/env python
# -*- coding: utf-8 -*-

"""code.py"""

import datetime
import json
import os
import base64
import struct
import subprocess
import socket

from application import my_app

urls = (
    ("/", "index"),
    ("/favicon.ico", "favicon"),
    ("/js/(.*)", "js"),
    ("/css/(.*)", "css"),
    ("/images/(.*)", "images"),
    ("/hello/(.*)", "hello"),
    ("/command/(.*)", "command"),
    ("/commands/(.*)", "commands"),
    ("/upload/(.*)", "upload"),
)

wsgiapp = my_app(urls, globals())

datetime_format = "%Y-%m-%d %H:%M:%S.%f"


def net_is_used(port, ip="127.0.0.1"):
    s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

    try:
        s.connect((ip, port))
        s.shutdown(2)
        print("%s:%d is used" % (ip, port))

        return True
    except Exception as e:
        print("%s:%d is unused" % (ip, port))

        return False


class index:
    def GET(self):
        my_app.header("Content-type", "text/plain")
        return "Welcome!\n"


class hello:
    def GET(self, name):
        my_app.header("Content-type", "text/plain")
        return "Hello %s!\n" % name


class js:
    def GET(self, name):
        my_app.header("Content-type", "text/javascript")
        # path = self.environ["PATH_INFO"]
        # tpl = Template(Static(environ["PATH_INFO"]))
        # js = tpl.safe_substitute()
        # return js
        return "Hello %s!\n" % name


class css:
    def GET(self, name):
        my_app.header("Content-type", "text/css")
        return "Hello %s!\n" % name


class favicon:
    def GET(self):
        my_app.header("Content-type", "image/x-icon")
        # path = self.environ["PATH_INFO"]

        return "Hello favicon !\n"


class images:
    def GET(self, name):
        my_app.header("Content-type", "image/jpeg")
        # image = Image.open(os.getcwd()+environ["PATH_INFO"])

        return "Hello %s!\n" % name

        # ext = environ["PATH_INFO"].split(".")
        # for n in ext:
        #     mime = n
        # m = [("content-type", "image/"+mime)]	
        # start_response("200 OK", m)	
        # image = Image.open(os.getcwd()+environ["PATH_INFO"])
        # return [image]


class command:
    def POST(self, name, request_body):
        my_app.header("Content-type", "application/json")

        try:
            json_request_body = json.loads(request_body)

            str_command = json_request_body

            # 判断is_wait
            if str_command.endswith("&"):
                is_wait = False
            else:
                is_wait = True

            print("[%s] [command] [POST] %s" % (datetime.datetime.now().strftime(datetime_format), str_command))

            if is_wait:
                out = os.popen(str_command).read()
            else:
                out = os.popen(str_command)

            # out = os.system(str_command)
            # out = os.popen(str_command)
            # out = os.popen(str_command).read()
            # out = subprocess.call(str_command, shell=True)
            # out = subprocess.getstatusoutput(str_command)

            # out = subprocess.Popen(str_command, shell=True, stdout=subprocess.PIPE, stderr=subprocess.PIPE)
            # out.wait()
            # returncode = out.returncode

            # if out.returncode != 0:
            #     print "Error."
            #     return -1
        except Exception as e:
            result = {
                "is_success": False,
                "code": "Fail",
                "message": "失败：" + str(e)
            }
        else:
            result = {
                "is_success": True,
                "code": "OK",
                "message": "成功"
            }

            result["out"] = str(out)

        str_result = json.dumps(result)

        return str_result


class commands:
    def POST(self, name, request_body):
        my_app.header("Content-type", "application/json")

        try:
            # {
            #     "commands": [
            #         {
            #             "name": "dir",
            #             "command": "dir"
            #         },
            #         {
            #             "name": "locust",
            #             "command": "locust -f \"E:\\Documents\\Visual Studio Code\\TestPython\\locust-tcp\\locustfiles\\TcpSocketClient-locust.py\" &"
            #         }
            #     ]
            # }
            json_request_body = json.loads(request_body)

            for command in json_request_body["commands"]:
                str_command = command["command"]

                # 判断is_wait
                if str_command.endswith("&"):
                    is_wait = False
                else:
                    is_wait = True

                print("[%s] [command] [POST] %s" % (datetime.datetime.now().strftime(datetime_format), str_command))

                if is_wait:
                    out = os.popen(str_command).read()
                else:
                    out = os.popen(str_command)

                # out = os.system(str_command)
                # out = os.popen(str_command)
                # out = os.popen(str_command).read()
                # out = subprocess.call(str_command, shell=True)
                # out = subprocess.getstatusoutput(str_command)

                # out = subprocess.Popen(str_command, shell=True, stdout=subprocess.PIPE, stderr=subprocess.PIPE)
                # out.wait()
                # returncode = out.returncode

                # if out.returncode != 0:
                #     print "Error."
                #     return -1

                command["out"] = str(out)
        except Exception as e:
            result = {
                "is_success": False,
                "code": "Fail",
                "message": "失败：" + str(e)
            }
        else:
            result = {
                "is_success": True,
                "code": "OK",
                "message": "成功"
            }

            result["result"] = json_request_body

        str_result = json.dumps(result)

        return str_result


class upload:
    def POST(self, name, request_body):
        my_app.header("Content-type", "application/json")

        try:
            # {
            #     "path": "E:\\Downloads\\Python\\test.py",
            #     "content": "123"
            # }
            json_request_body = json.loads(request_body)

            # file_name = json_request_body["file_name"]
            path = json_request_body["path"]
            # content_type = json_request_body["content_type"]
            content_type = "text"
            content = json_request_body["content"]

            # 判断文件夹是否存在，不存在就创建
            try:
                dirname = os.path.dirname(path)

                if not os.path.exists(dirname):
                    os.makedirs(dirname)
            except Exception as e:
                pass

            # 判断文件是否存在，存在就删除
            if os.path.exists(path):
                os.remove(path)

            print("[%s] [upload] [POST] %s" % (datetime.datetime.now().strftime(datetime_format), path))            

            if content_type == "text":
                with open(path, "w") as f:
                    f.write(content)
            elif content_type == "base64":
                data = base64.b64decode(content)
                file = open(path, "wb")
                file.write(data)
                file.close()
            elif content_type == "binary":
                with open(path, "wb") as f:
                    for x in content:
                        s = struct.pack("b", x)
                        f.write(s)
            else:
                pass
        except Exception as e:
            result = {
                "is_success": False,
                "code": "Fail",
                "message": "失败：" + str(e)
            }
        else:
            result = {
                "is_success": True,
                "code": "OK",
                "message": "成功"
            }

            result["result"] = json_request_body

        str_result = json.dumps(result)

        return str_result


if __name__ == "__main__":
    if not net_is_used(8085):
        from wsgiref.simple_server import make_server
        httpd = make_server("", 8085, wsgiapp)

        sa = httpd.socket.getsockname()
        print("Server is started, http://%s:%s/" % sa)

        # Respond to requests until process is killed
        httpd.serve_forever()

# python "E:\Documents\Visual Studio Code\TestPython\webapi\code\server.py"
