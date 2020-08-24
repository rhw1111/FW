-- --------------------------------------------------------
-- 主机:                           127.0.0.1
-- 服务器版本:                        8.0.20 - MySQL Community Server - GPL
-- 服务器操作系统:                      Linux
-- HeidiSQL 版本:                  11.0.0.5919
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;


-- 导出 tpconfig 的数据库结构
CREATE DATABASE IF NOT EXISTS `tpconfig` /*!40100 DEFAULT CHARACTER SET utf8 */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `tpconfig`;

-- 导出  表 tpconfig.commonlog_local 结构
CREATE TABLE IF NOT EXISTS `commonlog_local` (
  `id` char(36) NOT NULL,
  `parentid` char(36) NOT NULL,
  `prelevelid` char(36) NOT NULL,
  `currentlevelid` char(36) NOT NULL,
  `contextinfo` varchar(500) NOT NULL,
  `actionname` varchar(300) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `categoryname` varchar(300) NOT NULL,
  `parentactionname` varchar(150) NOT NULL,
  `requestbody` varchar(6000) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `responsebody` varchar(6000) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `requesturi` varchar(200) NOT NULL,
  `message` varchar(6000) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `root` bit(1) NOT NULL DEFAULT b'0',
  `level` int NOT NULL DEFAULT '0',
  `duration` bigint NOT NULL DEFAULT '0',
  `createtime` datetime NOT NULL,
  `modifytime` datetime NOT NULL,
  `sequence` bigint NOT NULL AUTO_INCREMENT,
  PRIMARY KEY (`sequence`),
  UNIQUE KEY `id` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8;



-- 导出  表 tpconfig.systemconfiguration 结构
CREATE TABLE IF NOT EXISTS `systemconfiguration` (
  `id` char(36) NOT NULL,
  `name` varchar(150) NOT NULL,
  `content` varchar(4000) NOT NULL,
  `createtime` datetime NOT NULL,
  `modifytime` datetime NOT NULL,
  `sequence` bigint NOT NULL AUTO_INCREMENT,
  PRIMARY KEY (`sequence`),
  UNIQUE KEY `id` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8;

-- 正在导出表  tpconfig.systemconfiguration 的数据：~0 rows (大约)
/*!40000 ALTER TABLE `systemconfiguration` DISABLE KEYS */;
REPLACE INTO `systemconfiguration` (`id`, `name`, `content`, `createtime`, `modifytime`, `sequence`) VALUES
	('94e0b048-af6e-11ea-8e6a-0242ac110002', 'DefaultUserID', '"46f8bcca-af6e-11ea-8e6a-0242ac110002"', '2020-06-16 01:13:26', '2020-06-16 01:13:26', 1),
	('3e552936-af6f-11ea-8e6a-0242ac110002', 'TestPlatform.Portal.Api_CrosOrigin', '["http://52.188.14.158"]', '2020-06-16 01:18:11', '2020-06-16 01:18:11', 2),
	('efa02231-af6f-11ea-8e6a-0242ac110002', 'LogExcludePaths', '["api/monitor"]', '2020-06-16 01:23:08', '2020-06-16 01:23:08', 3),
	('057d6ca4-af70-11ea-8e6a-0242ac110002', 'OutputStreamReplaceExcludePaths', '[]', '2020-06-16 01:23:45', '2020-06-16 01:23:45', 4),
	('f48e95ab-b836-11ea-bed4-025041000001', 'CaseServiceBaseAddress', '"http://52.188.14.158:8082/"', '2020-06-27 13:23:45', '2020-06-27 13:23:45', 5),
	('d4fdc4e2-4efd-4a1c-8372-5a6eca74e381', 'TestPlatform.CaseService_CrosOrigin', '["http://52.188.14.158"]', '2020-06-27 13:23:45', '2020-06-27 13:23:45', 6),
	('d2be1a01-bcd9-11ea-813c-025041000001', 'Tcp_TestMonitorAddress', '"http://52.188.14.158:3000/d/kr5bLGMMz/test-case-monitor?orgId=1"', now(), now(), 7),
	('1316b30b-bcdb-11ea-813c-025041000001', 'Http_TestMonitorAddress', '"http://52.188.14.158:3000/d/kr5bLGMMz/test-case-monitor?orgId=1"', now(), now(), 8),
	('2316b30b-bcdb-11ea-813c-025041000001', 'TestHistoryMonitorAddress', '"http://52.188.14.158:3000/d/VQG1ohSGz/test-case-history-monitor?orgId=1"', now(), now(), 9),
	('a41d654c-ddf6-11ea-8205-025041000001', 'NetGatewayDataFolder', '"/home/TPUser/NetGateway"', now(), now(), 10),
	('b191b7a1-ddf6-11ea-8205-025041000001', 'NetGatewayDataTempFolder', '"/home/TPUser/TempNetGateway"', now(), now(), 11),
	('b9c6b7e8-ddf6-11ea-8205-025041000001', 'NetGatewayDataSSHEndpoint', '"NetGatewayDataSSHEndpoint"', now(), now(), 12);
/*!40000 ALTER TABLE `systemconfiguration` ENABLE KEYS */;

DROP TABLE IF EXISTS `influxdbendpoint`;
CREATE TABLE `influxdbendpoint` (
  `id` char(36) NOT NULL,
  `name` varchar(150) NOT NULL,
  `address` varchar(150) NOT NULL,
  `isauth` bit NOT NULL,
  `username` varchar(150),
  `password` varchar(150),
  `createtime` datetime NOT NULL,
  `modifytime` datetime NOT NULL,
  `sequence` bigint NOT NULL AUTO_INCREMENT,
  PRIMARY KEY (`sequence`),
  UNIQUE KEY `id` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;

REPLACE INTO `influxdbendpoint` (`id`, `name`, `address`, `isauth`, `username`, `password`, `createtime`, `modifytime`) VALUES ('c7a290e6-eddd-4126-abc9-5e129718e0fc', 'EndpointName', 'http://172.17.0.1:8086', b'0', 'admin', 'admin', UTC_TIMESTAMP(),UTC_TIMESTAMP());

-- 导出 tpmain 的数据库结构
CREATE DATABASE IF NOT EXISTS `tpmain` /*!40100 DEFAULT CHARACTER SET utf8 */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `tpmain`;

-- 导出  表 tpmain.testdatasource 结构
CREATE TABLE IF NOT EXISTS `testdatasource` (
  `id` char(36) NOT NULL,
  `type` varchar(150) NOT NULL DEFAULT '',
  `name` varchar(150) NOT NULL DEFAULT '',
  `data` mediumtext NOT NULL,
  `createtime` datetime NOT NULL,
  `modifytime` datetime NOT NULL,
  `sequence` bigint NOT NULL AUTO_INCREMENT,
  PRIMARY KEY (`sequence`),
  UNIQUE KEY `id` (`id`),
  KEY `name` (`name`),
  KEY `createtime` (`createtime`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;



-- 导出  表 tpmain.testhost 结构
CREATE TABLE IF NOT EXISTS `testhost` (
  `id` char(36) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `address` varchar(100) NOT NULL DEFAULT '',
  `sshendpointid` char(36) NOT NULL DEFAULT '0',
  `createtime` datetime NOT NULL,
  `modifytime` datetime NOT NULL,
  `sequence` bigint NOT NULL AUTO_INCREMENT,
  PRIMARY KEY (`sequence`),
  UNIQUE KEY `id` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- 正在导出表  tpmain.testhost 的数据：~0 rows (大约)
/*!40000 ALTER TABLE `testhost` DISABLE KEYS */;
/*!40000 ALTER TABLE `testhost` ENABLE KEYS */;

-- 导出  表 tpmain.user 结构
CREATE TABLE IF NOT EXISTS `user` (
  `id` varchar(36) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `name` varchar(150) NOT NULL,
  `createtime` datetime NOT NULL,
  `modifytime` datetime NOT NULL,
  `sequence` bigint NOT NULL AUTO_INCREMENT,
  PRIMARY KEY (`sequence`),
  UNIQUE KEY `id` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;

-- 正在导出表  tpmain.user 的数据：~0 rows (大约)
/*!40000 ALTER TABLE `user` DISABLE KEYS */;
REPLACE INTO `user` (`id`, `name`, `createtime`, `modifytime`, `sequence`) VALUES
	('46f8bcca-af6e-11ea-8e6a-0242ac110002', 'admin', '2020-06-16 01:11:16', '2020-06-16 01:11:16', 1);
/*!40000 ALTER TABLE `user` ENABLE KEYS */;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;

DROP TABLE IF EXISTS `sshendpoint`;
CREATE TABLE `sshendpoint` (
  `id` char(36) NOT NULL,
  `type` varchar(150) NOT NULL DEFAULT '',
  `name` varchar(150) NOT NULL DEFAULT '',
  `configuration` varchar(1000) NOT NULL DEFAULT '',
  `createtime` datetime NOT NULL,
  `modifytime` datetime NOT NULL,
  `sequence` bigint NOT NULL AUTO_INCREMENT,
  PRIMARY KEY (`sequence`),
  UNIQUE KEY `id` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;

REPLACE INTO `sshendpoint` (`id`, `type`, `name`, `configuration`, `createtime`, `modifytime`) VALUES
('55b81537-e5b4-11ea-8205-025041000001', 'Default', 'NetGatewayDataSSHEndpoint', '{
    "Address": "10.0.0.5",
    "Port": "22",
    "UserName": "TPUser",
    "Password": "Password01asd!"
}', now(), now())

DROP TABLE IF EXISTS `testcase`;
CREATE TABLE `testcase` (
  `id` char(36) NOT NULL,
  `masterhostid` char(36) NOT NULL,
  `ownerid` char(36) NOT NULL,
  `testcasehistoryid` char(36) DEFAULT NULL,
  `enginetype` varchar(150) NOT NULL DEFAULT '',
  `name` varchar(150) NOT NULL DEFAULT '',
  `configuration` mediumtext NOT NULL,
  `status` int NOT NULL DEFAULT '0',
  `createtime` datetime NOT NULL,
  `modifytime` datetime NOT NULL,
  `sequence` bigint NOT NULL AUTO_INCREMENT,
  PRIMARY KEY (`sequence`),
  UNIQUE KEY `id` (`id`),
  KEY `name` (`name`),
  KEY `createtime` (`createtime`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;

DROP TABLE IF EXISTS `testcaseslavehost`;
CREATE TABLE `testcaseslavehost` (
  `id` char(36) NOT NULL,
  `hostid` char(36) NOT NULL,
  `testcaseid` char(36) NOT NULL,
  `slavename` varchar(150) NOT NULL DEFAULT '',
  `count` int NOT NULL DEFAULT '0',
  `extensioninfo` varchar(1000) NOT NULL DEFAULT '',
  `createtime` datetime NOT NULL,
  `modifytime` datetime NOT NULL,
  `sequence` bigint NOT NULL AUTO_INCREMENT,
  PRIMARY KEY (`sequence`),
  UNIQUE KEY `id` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;

DROP TABLE IF EXISTS `testcasehistory`;
CREATE TABLE `testcasehistory` (
  `id` char(36) NOT NULL,
  `caseid` char(36) NOT NULL,
  `summary` varchar(4000) NOT NULL DEFAULT '',
  `netgatewaydataformat` varchar(150) NOT NULL DEFAULT '',
  `createtime` datetime NOT NULL,
  `modifytime` datetime NOT NULL,
  `sequence` bigint NOT NULL AUTO_INCREMENT,
  PRIMARY KEY (`sequence`),
  UNIQUE KEY `id` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;

DROP TABLE IF EXISTS `scripttemplate`;
CREATE TABLE `scripttemplate` (
  `id` char(36) NOT NULL,
  `name` varchar(150) NOT NULL DEFAULT '',
  `content` mediumtext NOT NULL,
  `createtime` datetime NOT NULL,
  `modifytime` datetime NOT NULL,
  `sequence` bigint NOT NULL AUTO_INCREMENT,
  PRIMARY KEY (`sequence`),
  UNIQUE KEY `id` (`id`),
  KEY `name` (`name`),
  KEY `createtime` (`createtime`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;








-- 导出  表 tpconfig.scheduleaction 结构
CREATE TABLE IF NOT EXISTS `scheduleaction` (
  `id` char(36) NOT NULL,
  `name` varchar(150) NOT NULL,
  `triggercondition` varchar(200) NOT NULL,
  `groupid` char(36) NOT NULL,
  `configuration` mediumtext NOT NULL,
  `mode` int NOT NULL,
  `scheduleactionservicefactorytype` varchar(200) DEFAULT NULL,
  `scheduleactionservicefactorytypeusedi` bit(1) DEFAULT NULL,
  `scheduleactionserviceweburl` varchar(200) DEFAULT NULL,
  `websignature` varchar(200) DEFAULT NULL,
  `status` int NOT NULL,
  `createtime` datetime NOT NULL,
  `modifytime` datetime NOT NULL,
  `sequence` bigint NOT NULL AUTO_INCREMENT,
  PRIMARY KEY (`sequence`),
  UNIQUE KEY `id` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;

-- 正在导出表  tpconfig.scheduleaction 的数据：~0 rows (大约)
/*!40000 ALTER TABLE `scheduleaction` DISABLE KEYS */;
REPLACE INTO `scheduleaction` (`id`, `name`, `triggercondition`, `groupid`, `configuration`, `mode`, `scheduleactionservicefactorytype`, `scheduleactionservicefactorytypeusedi`, `scheduleactionserviceweburl`, `websignature`, `status`, `createtime`, `modifytime`, `sequence`) VALUES
	('482a79ff-d089-4c20-9311-c2c579965bd9', 'NetGatewayFactory', '0/10 * * * * ?', '25329e82-fa23-4530-892e-d74c4df08f02', '', 0, 'FW.TestPlatform.Main.Schedule.Actions.ScheduleActionServiceForNetGatewayFactory, TestPlatform.Main', b'1', NULL, NULL, 1, '2020-07-29 04:08:20', '2020-07-29 04:08:20', 1);
/*!40000 ALTER TABLE `scheduleaction` ENABLE KEYS */;

-- 导出  表 tpconfig.scheduleactiongroup 结构
CREATE TABLE IF NOT EXISTS `scheduleactiongroup` (
  `id` char(36) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `name` varchar(150) NOT NULL,
  `uselog` bit(1) NOT NULL,
  `executeactioninittype` varchar(150) NOT NULL,
  `executeactioninitconfiguration` mediumtext NOT NULL,
  `createtime` datetime NOT NULL,
  `modifytime` datetime NOT NULL,
  `sequence` bigint NOT NULL AUTO_INCREMENT,
  PRIMARY KEY (`sequence`),
  UNIQUE KEY `id` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;

-- 正在导出表  tpconfig.scheduleactiongroup 的数据：~0 rows (大约)
/*!40000 ALTER TABLE `scheduleactiongroup` DISABLE KEYS */;
REPLACE INTO `scheduleactiongroup` (`id`, `name`, `uselog`, `executeactioninittype`, `executeactioninitconfiguration`, `createtime`, `modifytime`, `sequence`) VALUES
	('25329e82-fa23-4530-892e-d74c4df08f02', 'G1', b'0', 'Default', '{"EnvironmentClaimGeneratorName":"Default","ClaimContextGeneratorName":"Default"}', '2020-07-29 02:41:40', '2020-07-29 02:41:40', 1);
/*!40000 ALTER TABLE `scheduleactiongroup` ENABLE KEYS */;

-- 导出  表 tpconfig.schedulehostconfiguration 结构
CREATE TABLE IF NOT EXISTS `schedulehostconfiguration` (
  `id` char(36) NOT NULL,
  `name` varchar(150) NOT NULL,
  `schedulegroupname` varchar(150) NOT NULL,
  `environmentclaimgeneratorname` varchar(150) NOT NULL,
  `claimcontextgeneratorname` varchar(150) NOT NULL,
  `createtime` datetime NOT NULL,
  `modifytime` datetime NOT NULL,
  `sequence` bigint NOT NULL AUTO_INCREMENT,
  PRIMARY KEY (`sequence`),
  UNIQUE KEY `id` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;

-- 正在导出表  tpconfig.schedulehostconfiguration 的数据：~0 rows (大约)
/*!40000 ALTER TABLE `schedulehostconfiguration` DISABLE KEYS */;
REPLACE INTO `schedulehostconfiguration` (`id`, `name`, `schedulegroupname`, `environmentclaimgeneratorname`, `claimcontextgeneratorname`, `createtime`, `modifytime`, `sequence`) VALUES
	('725f3681-da0a-435b-8010-daa2113db2cb', 'TestPlatform_NetGatewayFactory', 'G1', 'Default', 'Default', '2020-07-29 02:13:41', '2020-07-29 02:13:41', 1);
/*!40000 ALTER TABLE `schedulehostconfiguration` ENABLE KEYS */;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;

/*!ALTER TABLE testcasehistory*/
ALTER TABLE `testcasehistory` ADD COLUMN `netgatewaydataformat` varchar(150) NOT NULL DEFAULT '';