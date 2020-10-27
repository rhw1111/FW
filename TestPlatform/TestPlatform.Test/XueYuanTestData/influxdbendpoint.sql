SELECT * FROM tpconfig.influxdbendpoint;

use tpconfig;

REPLACE INTO `influxdbendpoint` (`id`, `name`, `address`, `isauth`, `username`, `password`, `createtime`, `modifytime`) 
VALUES ('c7a290e6-eddd-4126-abc9-5e129718e0fc', 'EndpointName', 'http://localhost:8086', b'0', '', '', UTC_TIMESTAMP(),UTC_TIMESTAMP());

