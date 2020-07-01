SELECT * FROM tpmain.testdatasource;

INSERT INTO tpmain.testdatasource(id, type, name, data, createtime, modifytime)
VALUES('d46f4d6a-b7c4-11ea-8dae-00ffb1d16cf9', 'Json', 'datasource_user_account_list', '[
    {
        "UserName": "zhangsan",
        "Password": "123456"
    },
    {
        "UserName": "lisi",
        "Password": "123456"
    },
]', now(), now());





