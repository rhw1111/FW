SELECT * FROM tpmain.testcase;
SELECT * FROM tpmain.treeentity
WHERE name LIKE '%XueYuan%';
SELECT id, masterhostid, ownerid, enginetype, name, status, createtime, modifytime, sequence, testcasehistoryid, treeid FROM tpmain.testcase
WHERE name LIKE '%XueYuan%';

SELECT sequence FROM tpmain.testcase;
SELECT * FROM tpmain.testcase
WHERE id = 'c9198a53-137b-11eb-bbfc-00ffb1d16cf9';

REPLACE INTO tpmain.testcase
VALUES('c9198a53-137b-11eb-bbfc-00ffb1d16cf9', '822114cf-5277-4667-961f-e231f9e67e4d', '46f8bcca-af6e-11ea-8e6a-0242ac110002', 'Jmeter', 'XueYuanTestJmeter', '', '0', now(), now(), '258', null, '1e6d9aed-1436-11eb-8e14-00ffb1d16cf9');
REPLACE INTO tpmain.treeentity
VALUES('1e6d9aed-1436-11eb-8e14-00ffb1d16cf9', 'XueYuanTestJmeter', 'c9198a53-137b-11eb-bbfc-00ffb1d16cf9', '2', '78b9de7c-2ce4-41ba-89ec-10dfd1bbdff0', now(), now(), '801');


UPDATE tpmain.testcase
SET status = '0'
where id = 'c9198a53-137b-11eb-bbfc-00ffb1d16cf9';

UPDATE tpmain.testcase
SET configuration = '{
    "FileContent": "<?xml version=\\\"1.0\\\" encoding=\\\"UTF-8\\\"?>\\n<jmeterTestPlan version=\\\"1.2\\\" properties=\\\"5.0\\\" jmeter=\\\"5.3\\\">\\n  <hashTree>\\n    <TestPlan guiclass=\\\"TestPlanGui\\\" testclass=\\\"TestPlan\\\" testname=\\\"Test Plan\\\" enabled=\\\"true\\\">\\n      <stringProp name=\\\"TestPlan.comments\\\"></stringProp>\\n      <boolProp name=\\\"TestPlan.functional_mode\\\">false</boolProp>\\n      <boolProp name=\\\"TestPlan.tearDown_on_shutdown\\\">true</boolProp>\\n      <boolProp name=\\\"TestPlan.serialize_threadgroups\\\">false</boolProp>\\n      <elementProp name=\\\"TestPlan.user_defined_variables\\\" elementType=\\\"Arguments\\\" guiclass=\\\"ArgumentsPanel\\\" testclass=\\\"Arguments\\\" testname=\\\"User Defined Variables\\\" enabled=\\\"true\\\">\\n        <collectionProp name=\\\"Arguments.arguments\\\"/>\\n      </elementProp>\\n      <stringProp name=\\\"TestPlan.user_define_classpath\\\"></stringProp>\\n    </TestPlan>\\n    <hashTree>\\n    </hashTree>\\n  </hashTree>\\n</jmeterTestPlan>",
    "DataSourceVars": [
        {
            "Name": "数据文件1",
            "Path": "E:/Document/Jmeter/Users.csv",
            "Type": "",
            "DataSourceName": "datasource_users.csv",
            "Data": ""
        }
    ]
}'
where id = 'c9198a53-137b-11eb-bbfc-00ffb1d16cf9';

SELECT * FROM tpmain.testcase
WHERE id = '8e1accf9-1a3e-466e-b71f-7eb81c9f7065';
