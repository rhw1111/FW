SELECT * FROM tpconfig.systemconfiguration;

REPLACE INTO tpconfig.systemconfiguration
VALUES('f48e95ab-b836-11ea-bed4-025041000001', 'CaseServiceBaseAddress', '"http://52.188.14.158:8082/"', '2020-06-27 13:23:45', '2020-06-27 13:23:45', 5)

update tpconfig.systemconfiguration
set content = '"E:\\\\Documents\\\\Visual Studio Code\\\\TestPython\\\\pcapreader\\\\cap"'
where id = 'a41d654c-ddf6-11ea-8205-025041000001'

update tpconfig.systemconfiguration
set content = '"D:\\\\Test"'
where id = 'a41d654c-ddf6-11ea-8205-025041000001'

update tpconfig.systemconfiguration
set content = '"E:\\\\Documents\\\\Visual Studio Code\\\\TestPython\\\\pcapreader\\\\cap\\\\temp"'
where id = 'b191b7a1-ddf6-11ea-8205-025041000001'

