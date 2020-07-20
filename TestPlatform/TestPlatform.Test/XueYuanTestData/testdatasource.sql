SELECT * FROM tpmain.testdatasource;

REPLACE INTO tpmain.testdatasource(id, type, name, data, createtime, modifytime)
VALUES('d46f4d6a-b7c4-11ea-8dae-00ffb1d16cf9', 'Json', 'datasource_user_account_list', '[
    {
        "SlaveName": "client_id",
        "UserName": "zhangsan",
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
]', now(), now());

REPLACE INTO tpmain.testdatasource(id, type, name, data, createtime, modifytime)
VALUES('d46f4d6a-b7c4-11ea-8dae-00ffb1d16cf9', 'Json', 'datasource_user_account_list', '[
    {
        "SlaveName": "client_id",
        "UserName": "zhangsan",
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
]', now(), now());

REPLACE INTO tpmain.testdatasource(id, type, name, data, createtime, modifytime)
VALUES('d7290a77-c721-11ea-9f38-00ffb1d16cf9', 'Label', 'datasource_json_user_account_list', '{$filterjsondatainvoke({$datasource(user_account_list)},\'SlaveName\',{$SlaveName()})}', now(), now());

REPLACE INTO tpmain.testdatasource(id, type, name, data, createtime, modifytime)
VALUES('65552bae-c4d8-11ea-8951-00ffb1d16cf9', 'Json', 'datasource_port_list', '[
    8001,
    8002,
    8003,
    8004,
    8005,
    8006,
    8007,
    8008
]', now(), now());
