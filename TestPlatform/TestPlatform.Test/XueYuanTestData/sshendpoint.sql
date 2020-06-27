SELECT * FROM tpmain.sshendpoint;

insert into tpmain.sshendpoint
values('1b846704-5449-4585-bb15-8b13388cb68b', 'Linux', 'name', '{}', now(), now(), '2');

delete from tpmain.sshendpoint
where id = 'dbb136d0-b71f-11ea-8dae-00ffb1d16cf9';
