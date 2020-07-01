SELECT * FROM tpmain.sshendpoint;

INSERT INTO tpmain.sshendpoint
VALUES('1b846704-5449-4585-bb15-8b13388cb68b', 'Default', 'name', '{}', now(), now(), '2');

update tpmain.sshendpoint
set configuration = '{
    "Address": "13.68.249.103",
    "Port": "22",
    "UserName": "TPUser",
    "Password": "Password01asd!"
}'
where id = '1b846704-5449-4585-bb15-8b13388cb68b';


