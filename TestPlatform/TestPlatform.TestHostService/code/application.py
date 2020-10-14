#!/usr/bin/env python
# -*- coding: utf-8 -*-

"""application.py"""

import re
import datetime

datetime_format = "%Y-%m-%d %H:%M:%S.%f"


class my_app:
    """my simple web framework"""

    headers = []

    def __init__(self, urls=(), fvars={}):
        self._urls = urls
        self._fvars = fvars

    def __call__(self, environ, start_response):
        try:
            self._status = "200 OK"  # 默认状态OK
            del self.headers[:]  # 清空上一次的headers

            result = self._delegate(environ)
            start_response(self._status, self.headers)

            # 将返回值result（字符串 或者 字符串列表）转换为迭代对象
            if isinstance(result, str):
                return iter([result.encode("utf-8")])
            else:
                return iter(result.encode("utf-8"))
        except Exception as e:
            print("[%s] [__call__] Error: %s" % (datetime.datetime.now().strftime(datetime_format), str(e)))

            return iter(str(e).encode("utf-8"))

    def _delegate(self, environ):
        path = environ["PATH_INFO"]
        method = environ["REQUEST_METHOD"]

        for pattern, name in self._urls:
            m = re.match("^" + pattern + "$", path)

            if m:
                # pass the matched groups as arguments to the function
                args = m.groups()
                funcname = method.upper()  # 方法名大写（如GET、POST）
                klass = self._fvars.get(name)  # 根据字符串名称查找类对象

                if hasattr(klass, funcname):
                    func = getattr(klass, funcname)

                    if funcname == "POST":
                        try:
                            request_body_size = int(environ.get("CONTENT_LENGTH", 0))
                        except (ValueError):
                            request_body_size = 0

                        request_body = environ["wsgi.input"].read(request_body_size)

                        return func(klass(), *args, request_body)
                    else:
                        return func(klass(), *args)

        return self._notfound()

    def _notfound(self):
        self._status = "404 Not Found"
        self.header("Content-type", "text/plain")
        return "Not Found\n"

    @classmethod
    def header(cls, name, value):
        cls.headers.append((name, value))
