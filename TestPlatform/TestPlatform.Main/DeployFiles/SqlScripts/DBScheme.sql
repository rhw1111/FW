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
	('3e552936-af6f-11ea-8e6a-0242ac110002', 'TestPlatform.Portal.Api_CrosOrigin', '["http://localhost:8333/"]', '2020-06-16 01:18:11', '2020-06-16 01:18:11', 2),
	('efa02231-af6f-11ea-8e6a-0242ac110002', 'LogExcludePaths', '[]', '2020-06-16 01:23:08', '2020-06-16 01:23:08', 3),
	('057d6ca4-af70-11ea-8e6a-0242ac110002', 'OutputStreamReplaceExcludePaths', '[]', '2020-06-16 01:23:45', '2020-06-16 01:23:45', 4);
/*!40000 ALTER TABLE `systemconfiguration` ENABLE KEYS */;


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

DROP TABLE IF EXISTS `testcase`;
CREATE TABLE `testcase` (
  `id` char(36) NOT NULL,
  `masterhostid` char(36) NOT NULL,
  `ownerid` char(36) NOT NULL,
  `enginetype` varchar(150) NOT NULL DEFAULT '',
  `name` varchar(150) NOT NULL DEFAULT '',
  `configuration` varchar(4000) NOT NULL '',
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
  `createtime` datetime NOT NULL,
  `modifytime` datetime NOT NULL,
  `sequence` bigint NOT NULL AUTO_INCREMENT,
  PRIMARY KEY (`sequence`),
  UNIQUE KEY `id` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;
