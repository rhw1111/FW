SELECT * FROM tpmain.testcase;
SELECT id, masterhostid, ownerid, enginetype, name, status, createtime, modifytime, sequence, testcasehistoryid, treeid FROM tpmain.testcase;

REPLACE INTO tpmain.testcase
VALUES('82962da0-e83b-11ea-bf37-00ffb1d16cf9', '822114cf-5277-4667-961f-e231f9e67e4d', '46f8bcca-af6e-11ea-8e6a-0242ac110002', 'WebSocket', 'XueYuanTestWebSocket', '', '0', now(), now(), '194', null, '11110f0a-02ee-11eb-94de-00ffb1d16cf9');

#SELECT * FROM tpmain.treeentity;

#REPLACE INTO tpmain.treeentity
#VALUES('11110f0a-02ee-11eb-94de-00ffb1d16cf9', 'XueYuanTestWebSocket', '82962da0-e83b-11ea-bf37-00ffb1d16cf9', '2', '78b9de7c-2ce4-41ba-89ec-10dfd1bbdff0', now(), now(), '613');

UPDATE tpmain.testcase
SET status = '0'
where id = '82962da0-e83b-11ea-bf37-00ffb1d16cf9';

UPDATE tpmain.testcase
SET configuration = '{
    "LocustMasterBindPort": 5557,
    "UserCount": 10,
    "PerSecondUserCount": 10,
    "Duration": 100,
    "Address": "ws://127.0.0.1:12345/",
    "Port": 12345,
    "ResponseSeparator": "</package>",
    "IsPrintLog": false,
    "SyncType": true,
    "DataSourceVars": [
        {
            "Name": "user_account_list",
            "Type": "",
            "DataSourceName": "datasource_user_account_list",
            "Data": ""
        },
        {
            "Name": "json_user_account_list",
            "Type": "",
            "DataSourceName": "datasource_json_user_account_list",
            "Data": ""
        },
        {
            "Name": "user_parameter_list",
            "Type": "",
            "DataSourceName": "datasource_user_parameter_list",
            "Data": ""
        }
    ],
    "ConnectInit": {
        "VarSettings": [
            {
                "Name": "json_user_account",
                "Content": "{$nameoncejsondatainvoke(json_user_account_list)}"
            },
            {
                "Name": "{$currconnectkv(\'user_name\')}",
                "Content": "{$varkv(json_user_account,\'UserName\')}"
            },
            {
                "Name": "{$currconnectkv(\'user_password\')}",
                "Content": "{$varkv(json_user_account,\'Password\')}"
            },
            {
                "Name": "json_user_parameter_list",
                "Content": "{$filterjsondatainvoke({$datasource(user_parameter_list)},\'UserName\',{$currconnectkv(\'user_name\')})}"
            },
            {
                "Name": "json_user_parameter_list",
                "Content": "{$splitjsondatainvoke({$datasource(user_parameter_list)},1)}"
            },
            {
                "Name": "{$currconnectkv(\'user_parameter\')}",
                "Content": "{$getjsonrowdatainvoke(json_user_parameter_list)}"
            },
            {
                "Name": "parameter",
                "Content": "{$varkv({$currconnectkv(\'user_parameter\')},\'Parameter\')}"
            },
            {
                "Name": "login_send_data",
                "Content": "{\'UserName\': {$currconnectkv(\'user_name\')}, \'PassWord\': {$currconnectkv(\'user_password\')}, \'a\': parameter}"
            },
            {
                "Name": "self.senddata",
                "Content": "login_send_data"
            },
            {
                "Name": "self.recvdata",
                "Content": "{$websocketwithconnectinvoke({$curconnect()},self.senddata,\'.*\')}"
            },
            {
                "Name": "{$currconnectkv(\'user_token\')}",
                "Content": "self.recvdata"
            },
            {
                "Name": "self.user_name",
                "Content": "{$varkv(json_user_account,\'UserName\')}"
            },
            {
                "Name": "self.user_password",
                "Content": "{$varkv(json_user_account,\'Password\')}"
            },
            {
                "Name": "self.user_token",
                "Content": "{$currconnectkv(\'user_token\')}"
            },
            {
                "Name": "self.is_success",
                "Content": "len(self.recvdata) > 0 and self.user_name"
            }
        ]
    },
    "SendInit": {
        "VarSettings": [
            {
                "Name": "parameter2",
                "Content": "{$varkv({$currconnectkv(\'user_parameter\')},\'Parameter2\')}"
            },
            {
                "Name": "request_body",
                "Content": "{\'UserName\': {$currconnectkv(\'user_name\')}, \'UserToken\': {$currconnectkv(\'user_token\')}, \'a\': parameter2}"
            },
            {
                "Name": "request_body",
                "Content": "\'8=IMIX.2.0\\\\x019=414\\\\x0135=D\\\\x0134=2\\\\x0149=100005111000000103001\\\\x0150=test.dealer@eibc\\\\x0152=20200302-10:05:39.124\\\\x0156=CFETS-TRADING-INFI\\\\x0157=ODM\\\\x01115=100005111000000103001\\\\x01116=test.dealer@eibc\\\\x0111=346989db-7087-4c0f-8e1d-f600756fa870\\\\x0138=100000\\\\x0140=2\\\\x0144=1\\\\x0154=1\\\\x0155=USD.CNY\\\\x0163=1\\\\x01126=20200302-23:23:00\\\\x01167=FXSWAP\\\\x01803=2\\\\x012422=ORDE20200302test.dealer@eibc00000000\\\\x0110176=11\\\\x0111233=554710090226511\\\\x01453=1\\\\x01448=100005111000000103001\\\\x01452=1\\\\x01802=1\\\\x01523=test.dealer@eibc\\\\x01\'"
            },
            {
                "Name": "request_body_head",
                "Content": "\'8=IMIX.2.0\\\\x019=414\\\\x01\'"
            },
            {
                "Name": "request_body_len",
                "Content": "\'35=D\\\\x0134=2\\\\x0149=100005111000000103001\\\\x0150=test.dealer@eibc\\\\x0152=20200302-10:05:39.124\\\\x0156=CFETS-TRADING-INFI\\\\x0157=ODM\\\\x01115=100005111000000103001\\\\x01116=test.dealer@eibc\\\\x0111=346989db-7087-4c0f-8e1d-f600756fa870\\\\x0138=100000\\\\x0140=2\\\\x0144=1\\\\x0154=1\\\\x0155=USD.CNY\\\\x0163=1\\\\x01126=20200302-23:23:00\\\\x01167=FXSWAP\\\\x01803=2\\\\x012422=ORDE20200302test.dealer@eibc00000000\\\\x0110176=11\\\\x0111233=554710090226511\\\\x01453=1\\\\x01448=100005111000000103001\\\\x01452=1\\\\x01802=1\\\\x01523=test.dealer@eibc\\\\x01\'"
            },
            {
                "Name": "request_body_calcchecksum",
                "Content": "request_body_head + \'44=\' + str(len(request_body_len)) + \'\\\\x01\' + request_body_len"
            },
            {
                "Name": "request_body_all",
                "Content": "request_body_calcchecksum + \'10=\' + str({$numberfill({$calcchecksuminvoke(request_body_calcchecksum)},0,3)}) + \'\'"
            },
            {
                "Name": "request_body",
                "Content": "request_body_all"
            },
            {
                "Name": "testNumberFill",
                "Content": "{$numberfill({$getnameserialnoinvoke({$currconnectkv(\'user_name\')},\'int\',0)},0,6)}"
            },
            {
                "Name": "package",
                "Content": "request_body"
            },
            {
                "Name": "package",
                "Content": "{$dessecurity(package,\'abcdefghjhijklmn\')}"
            },
            {
                "Name": "self.senddata",
                "Content": "package"
            },
            {
                "Name": "self.recvdata",
                "Content": "{$websocketwithconnectinvoke({$curconnect()},self.senddata,\'.*\')}"
            },
            {
                "Name": "self.is_success",
                "Content": "len(self.recvdata) > 0"
            }
        ]
    },
    "StopInit": {
        "VarSettings": [
            {
                "Name": "request_body",
                "Content": "{\'UserName\': {$currconnectkv(\'user_name\')}, \'UserToken\': {$currconnectkv(\'user_token\')}, \'a\': \'a\'}"
            },
            {
                "Name": "package",
                "Content": "request_body"
            },
            {
                "Name": "self.senddata",
                "Content": "package"
            },
            {
                "Name": "self.recvdata",
                "Content": "{$websocketwithconnectinvoke({$curconnect()},self.senddata,\'.*\')}"
            },
            {
                "Name": "self.is_success",
                "Content": "len(self.recvdata) > 0"
            }
        ]
    }
}'
where id = '82962da0-e83b-11ea-bf37-00ffb1d16cf9';


