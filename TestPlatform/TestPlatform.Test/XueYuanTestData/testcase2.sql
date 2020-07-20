REPLACE INTO tpmain.testcase
VALUES('455154e8-ca60-11ea-87e0-00ffb1d16cf9', '822114cf-5277-4667-961f-e231f9e67e4d', '46f8bcca-af6e-11ea-8e6a-0242ac110002', 'Tcp', 'XueYuanTest2', '', '0', now(), now(), '1');

UPDATE tpmain.testcase
SET configuration = '{
    "UserCount": 10,
    "PerSecondUserCount": 10,
    "Duration": 100,
    "Address": "172.17.38.12",
    "Port": 10011,
    "ResponseSeparator": "</package>",    
    "DataSourceVars": [
        {
            "Name": "order_list",
            "Type": "",
            "DataSourceName": "ctrade_poc_order_list",
            "Data": ""
        },
        {
            "Name": "user_list",
            "Type": "",
            "DataSourceName": "ctrade_poc_user_list",
            "Data": ""
        }    
    ],
    "ConnectInit": {
        "VarSettings": [
            {
                "Name": "json_user_account",
                "Content": "{$nameoncejsondatainvoke({$datasource(user_list)})}"
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
                "Name": "{$currconnectkv(\'org_id_21\')}",
                "Content": "{$varkv(json_user_account,\'OrgId21\')}"
            },
            {
                "Name": "request_body_head",
                "Content": "\'8=IMIXT.1.0\\x01\'"
            },
            {
                "Name": "request_body_len",
                "Content": "\'35=A\\x0134=1\\x0149=\'+{$currconnectkv(\'org_id_21\')}+\'\\x0150=\'+{$currconnectkv(\'user_id\')}+\'\\x0152=20200403-19:56:46.263\\x0156=CFETS-TRADING-INFI\\x0157=ODM\\x0198=0\\x01108=30\\x01141=Y\\x01553=\'+{$currconnectkv(\'user_id\')}+\'\\x01554=\'+{$currconnectkv(\'user_password\')}+\'\\x011137=IMIXT.1.0\\x011408=3.0\\x01\'"
            },
            {
                "Name": "request_body_calcchecksum",
                "Content": "request_body_head + \'9=\' + str(len(request_body_len)) + \'\\x01\' + request_body_len"
            },
            {
                "Name": "request_body_all",
                "Content": "request_body_calcchecksum + \'10=\' + str({$calcchecksuminvoke(request_body_calcchecksum)}) + \'\\x01\'"
            },                                    
            {
                "Name": "self.recvdata",
                "Content": "{$tcprrwithconnectinvoke({$curconnect()},request_body_all,\'.*\')}"
            }            
        ]
    },
    "SendInit": {
        "VarSettings": [
            {
                "Name": "json_poc_orders",
                "Content": "{$getjsondatainvoke({$datasource(order_list)},0,1)}"
            },
            {
                "Name": "{$currconnectkv(\'unique_num\')}",
                "Content": "{$varkv(json_poc_orders,\'UniqueNum\')}"
            },
            {
                "Name": "{$currconnectkv(\'order_price\')}",
                "Content": "{$varkv(json_poc_orders,\'Price\')}"
            },
            {
                "Name": "{$currconnectkv(\'order_symbol\')}",
                "Content": "{$varkv(json_poc_orders,\'Symbol\')}"
            },
            {
                "Name": "{$currconnectkv(\'order_num\')}",
                "Content": "{$varkv(json_poc_orders,\'OrderNum\')}"
            },
            {
                "Name": "{$currconnectkv(\'buy_or_sell\')}",
                "Content": "{$varkv(json_poc_orders,\'BuyOrSell\')}"
            },
            {
                "Name": "{$currconnectkv(\'settl_type\')}",
                "Content": "{$varkv(json_poc_orders,\'settlType\')}"
            },                       
            {
                "Name": "request_body_head",
                "Content": "\'8=IMIX.2.0\\x01\'"
            },
            {
                "Name": "request_body_len",
                "Content": "\'35=D\\x0134=4\\x0149=\'+{$currconnectkv(\'org_id_21\')}+\'\\x0150=\'+{$currconnectkv(\'user_id\')}+\'\\x0152=20200403-19:56:48.376\\x0156=CFETS-TRADING-INFI\\x0157=ODM\\x0112087=\'+{$currconnectkv(\'unique_num\')}+\'\\x01115=\'+{$currconnectkv(\'org_id_21\')}+\'\\x01116=\'+{$currconnectkv(\'user_id\')}+\'\\x0111=346989db-7087-4c0f-8e1d-f600756fa870\\x0138=100000\\x0140=2\\x0144=\'+{$currconnectkv(\'order_price\')}+\'\\x0154=\'+{$currconnectkv(\'buy_or_sell\')}+\'\\x0155=\'+{$currconnectkv(\'order_symbol\')}+\'\\x0163=\'+{$currconnectkv(\'settl_type\')}+\'\\x01126=20200403-23:23:00\\x01167=FXSWAP\\x01803=2\\x012422=ORDE20200403\'+{$currconnectkv(\'user_id\')}+{$currconnectkv(\'order_num\')}+\'\\x0110176=11\\x0111233=554710090226511\\x01453=1\\x01448=\'+{$currconnectkv(\'org_id_21\')}+\'\\x01452=1\\x01802=1\\x01523=\'+{$currconnectkv(\'user_id\')}+\'\\x01\'"
            },
            {
                "Name": "request_body_calcchecksum",
                "Content": "request_body_head + \'9=\' + str(len(request_body_len)) + \'\\x01\' + request_body_len"
            },
            {
                "Name": "request_body_all",
                "Content": "request_body_calcchecksum + \'10=\' + str({$calcchecksuminvoke(request_body_calcchecksum)}) + \'\\x01\'"
            },                                    
            {
                "Name": "self.recvdata",
                "Content": "{$tcprrwithconnectinvoke({$curconnect()},request_body_all,\'.*\')}"
            }                    
        ]
    },
    "StopInit": {
        "VarSettings": [

        ]
    }
}'
where id = '455154e8-ca60-11ea-87e0-00ffb1d16cf9';