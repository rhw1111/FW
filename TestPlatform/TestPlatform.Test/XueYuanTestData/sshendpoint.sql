SELECT * FROM tpmain.sshendpoint;

REPLACE INTO tpmain.sshendpoint
VALUES('1b846704-5449-4585-bb15-8b13388cb68b', 'Default', 'name', '{
    "Address": "13.68.249.103",
    "Port": "22",
    "UserName": "TPUser",
    "Password": "Password01asd!"
}', now(), now(), '1'),
('3a80b443-c4eb-11ea-8951-00ffb1d16cf9', 'Default', 'name', '{
    "Address": "20.185.245.250",
    "Port": "22",
    "UserName": "TPUser",
    "Password": "Password01asd!"
}', now(), now(), '2');

