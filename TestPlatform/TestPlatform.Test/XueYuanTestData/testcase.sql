SELECT * FROM tpmain.testcase;
SELECT id, name FROM tpmain.testcase;

REPLACE INTO tpmain.testcase
VALUES('ce514456-8da9-432f-8999-1010fa94a83a', '822114cf-5277-4667-961f-e231f9e67e4d', '46f8bcca-af6e-11ea-8e6a-0242ac110002', 'Tcp', 'XueYuanTest', '', '0', now(), now(), '1');

UPDATE tpmain.testcase
SET status = '0'
where id = 'ce514456-8da9-432f-8999-1010fa94a83a';

UPDATE tpmain.testcase
SET configuration = '{
    "UserCount": 10,
    "PerSecondUserCount": 10,
    "Duration": 100,
    "ReadyTime": 0,
    "Address": "127.0.0.1",
    "Port": 12345,
    "ResponseSeparator": "</package>",    
    "RequestBody": "{\'UserName\': {$currconnectkv(\'user_id\')}, \'UserToken\': {$currconnectkv(\'user_token\')}, \'a\': \'a\'}",
    "DataSourceVars": [
        {
            "Name": "user_account_list",
            "Type": "",
            "DataSourceName": "datasource_user_account_list",
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
                "Name": "json_user_account_list",
                "Content": "{$filterjsondatainvoke({$datasource(user_account_list)},\'SlaveName\',{$SlaveName()})}"
            },
            {
                "Name": "json_user_account",
                "Content": "{$nameoncejsondatainvoke(json_user_account_list)}"
            },
            {
                "Name": "{$currconnectkv(\'user_id\')}",
                "Content": "{$varkv(json_user_account,\'UserName\')}"
            },
            {
                "Name": "{$currconnectkv(\'user_password\')}",
                "Content": "{$varkv(json_user_account,\'Password\')}"
            },
            {
                "Name": "json_user_parameter_list",
                "Content": "{$filterjsondatainvoke({$datasource(user_parameter_list)},\'UserName\',{$currconnectkv(\'user_id\')})}"
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
                "Content": "{\'UserName\': {$currconnectkv(\'user_id\')}, \'PassWord\': {$currconnectkv(\'user_password\')}, \'a\': parameter}"
            },
            {
                "Name": "{$currconnectkv(\'user_token\')}",
                "Content": "{$tcprrwithconnectinvoke({$curconnect()},login_send_data,\'.*\')}"
            },
            {
                "Name": "",
                "Content": "a = 1
b = 2"
            },
            {
                "Name": "",
                "Content": "
a = 1
b = 2
                "
            },
            {
                "Name": "self.user_id",
                "Content": "{$varkv(json_user_account,\'UserName\')}"
            },
            {
                "Name": "self.user_password",
                "Content": "{$varkv(json_user_account,\'Password\')}"
            },
            {
                "Name": "self.user_token",
                "Content": "{$currconnectkv(\'user_token\')}"
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
                "Content": "{\'UserName\': {$currconnectkv(\'user_id\')}, \'UserToken\': {$currconnectkv(\'user_token\')}, \'a\': parameter2}"
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
                "Name": "self.recv_data",
                "Content": "{$tcprrwithconnectinvoke({$curconnect()},package,\'.*\')}"
            }
        ]
    },
    "StopInit": {
        "VarSettings": [
            {
                "Name": "request_body",
                "Content": "{\'UserName\': {$currconnectkv(\'user_id\')}, \'UserToken\': {$currconnectkv(\'user_token\')}, \'a\': \'a\'}"
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
                "Name": "self.recv_data",
                "Content": "{$tcprrwithconnectinvoke({$curconnect()},package,\'.*\')}"
            }
        ]
    }
}'
where id = 'ce514456-8da9-432f-8999-1010fa94a83a';



