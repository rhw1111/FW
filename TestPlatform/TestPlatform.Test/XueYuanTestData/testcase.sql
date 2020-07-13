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
    "Duration": 60,
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
        }
    ],
    "ConnectInit": {
        "VarSettings": [
            {
                "Name": "json_user_account",
                "Content": "{$nameoncejsondatainvoke({$datasource(user_account_list)})}"
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
                "Name": "login_send_data",
                "Content": "{\'UserName\': {$currconnectkv(\'user_id\')}, \'PassWord\': {$currconnectkv(\'user_password\')}, \'a\': \'a\'}"
            },
            {
                "Name": "{$currconnectkv(\'user_token\')}",
                "Content": "{$tcprrwithconnectinvoke({$curconnect()},json.dumps(login_send_data),\'.*\')}"
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
                "Name": "package",
                "Content": "{$dessecurity(package,\'abcdefghjhijklmn\')}"
            }
        ]
    }
}'
WHERE id = 'cae64c27-8e87-4a38-b94a-32a47a7eea63';

UPDATE tpmain.testcase
SET configuration = '{
    "UserCount": 100,
    "PerSecondUserCount": 10,
    "Duration": 60,
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
        }
    ],
    "ConnectInit": {
        "VarSettings": [
            {
                "Name": "json_user_account",
                "Content": "{$getjsonrowdatainvoke({$datasource(user_account_list)})}"
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
                "Name": "login_send_data",
                "Content": "{\'UserName\': {$currconnectkv(\'user_id\')}, \'PassWord\': {$currconnectkv(\'user_password\')}, \'a\': \'a\'}"
            },
            {
                "Name": "{$currconnectkv(\'user_token\')}",
                "Content": "{$tcprrwithconnectinvoke({$curconnect()},json.dumps(login_send_data),\'.*\')}"
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
                "Name": "package",
                "Content": "{$dessecurity(package,\'abcdefghjhijklmn\')}"
            }
        ]
    }
}'
WHERE id = 'ce514456-8da9-432f-8999-1010fa94a83a';


