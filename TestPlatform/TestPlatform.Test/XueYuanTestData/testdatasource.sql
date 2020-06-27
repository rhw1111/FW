SELECT * FROM tpmain.testdatasource;

insert into tpmain.testdatasource(id, type, name, data, createtime, modifytime)
values('dae3d35b-f618-47b9-b852-4ebee4b4e046', 'String', 'datasource_host', '127.0.0.1', now(), now());

insert into tpmain.testdatasource(id, type, name, data, createtime, modifytime)
values('72850a04-b5e0-11ea-b70a-00ffb1d16cf9', 'Int', 'datasource_port', '12345', now(), now());

insert into tpmain.testdatasource(id, type, name, data, createtime, modifytime)
values('fe6b2355-b708-11ea-8dae-00ffb1d16cf9', 'String', 'datasource_client_id', '{SlaveName}', now(), now());

insert into tpmain.testdatasource(id, type, name, data, createtime, modifytime)
values('fe6e3ca4-b708-11ea-8dae-00ffb1d16cf9', 'String', 'datasource_case_id', '{CaseID}', now(), now());

insert into tpmain.testdatasource(id, type, name, data, createtime, modifytime)
values('4b78fbb0-b709-11ea-8dae-00ffb1d16cf9', 'String', 'datasource_package_start', '"<package>"', now(), now());

insert into tpmain.testdatasource(id, type, name, data, createtime, modifytime)
values('4b7bddb8-b709-11ea-8dae-00ffb1d16cf9', 'String', 'datasource_package_end', '"</package>"', now(), now());

insert into tpmain.testdatasource(id, type, name, data, createtime, modifytime)
values('98d045c4-b7bd-11ea-8dae-00ffb1d16cf9', 'Json', 'datasource_send_data', '{$senddata()}', now(), now());

insert into tpmain.testdatasource(id, type, name, data, createtime, modifytime)
values('d46f4d6a-b7c4-11ea-8dae-00ffb1d16cf9', 'Json', 'datasource_user_account_list', '{
    "user_account_list": [
        {
            "UserName": "zhangsan",
            "Password": "123456"
        },
        {
            "UserName": "lisi",
            "Password": "123456"
        },
    ]
}', now(), now());





