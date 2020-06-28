SELECT * FROM tpmain.testcase;

INSERT INTO tpmain.testcase
VALUES('cae64c27-8e87-4a38-b94a-32a47a7eea63', '822114cf-5277-4667-961f-e231f9e67e4d', '46f8bcca-af6e-11ea-8e6a-0242ac110002', 'Tcp', 'Case1', '', '0', now(), now(), '1');

UPDATE tpmain.testcase
SET status = '0'
where id = 'cae64c27-8e87-4a38-b94a-32a47a7eea63';

UPDATE tpmain.testcase
SET configuration = '{
    "UserCount": 10,
    "PerSecondUserCount": 10,
    "Duration": 100,
    "ReadyTime": 0,
    "Address": "127.0.0.1",
    "Port": 12345,
    "ResponseSeparator": "</package>",    
    "RequestBody": "RequestBody",
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
                "Name": "ConnectInit_1",
                "Content": "Content"
            },
            {
                "Name": "ConnectInit_2",
                "Content": "Content"
            }
        ]
    },
    "SendInit": {
        "VarSettings": [
            {
                "Name": "SendInit_1",
                "Content": "Content"
            },
            {
                "Name": "SendInit_2",
                "Content": "Content"
            }
        ]
    },  
    "SendData": {
        "VarSettings": [
            {
                "Name": "send_data",
                "Content": "[{\'UserName\': \'{$GetUserName()}\', \'Token\': \'{$GetToken()}\', \'a\': \'a\'}]"
            }
        ]
    }
}'
WHERE id = 'cae64c27-8e87-4a38-b94a-32a47a7eea63';


