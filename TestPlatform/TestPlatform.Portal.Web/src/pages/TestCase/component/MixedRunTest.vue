<template>
  <div>
    <q-dialog v-model="runFixed"
              persistent>
      <!-- 运行执行框 -->
      <q-card style="width: 100%; max-width: 50vw; float:left;margin-right:5vw;">
        <q-card-section class="row">
          <div class="text-h6 col-10">请选择运行测试用例的模式</div>
          <q-btn flat
                 color="primary"
                 label="全部停止运行"
                 class="col-2"
                 @click="runAllStop" />

        </q-card-section>

        <q-separator />
        <div class="q-pa-md">
          <q-radio v-model="runModel"
                   val="parallel"
                   :disable="runBtnDisable"
                   label="并行模式" />
          <q-radio v-model="runModel"
                   val="sequential"
                   :disable="runBtnDisable"
                   label="顺序运行" />

          <!-- 并行模式 -->
          <div v-show="runModel=='parallel'">
            <div class="input_row row"
                 style="margin-bottom:10px;width:100%;display:inlin-block;"
                 v-for="(value,index) in runModelArray"
                 :key="index">
              <q-input v-model="value.executionTime"
                       outlined
                       :disable="runBtnDisable"
                       class="col-10"
                       :dense="true"
                       placeholder="请输入测试用例开始运行的延迟秒数"
                       @input="forceUpdate(value.executionTime,index)">
                <template v-slot:before>
                  <span style="font-size:14px;width:150px;word-wrap:break-word;">{{value.name}}:</span>
                </template>
                <template v-slot:after>
                  <span v-show="value.runStatus=='没有运行'"
                        style="font-size:14px;width:150px;word-wrap:break-word;">{{value.runStatus}}</span>
                  <span v-show="value.runStatus=='正在运行' || value.runStatus=='运行结束'|| value.runStatus=='停止运行'"
                        style="font-size:14px;width:150px;word-wrap:break-word; color:green">{{value.runStatus}}</span>
                  <span v-show="value.runStatus=='运行失败'"
                        style="font-size:14px;width:150px;word-wrap:break-word; color:red">{{value.runStatus}}</span>
                </template>
                <template v-slot:append>
                  <q-avatar>
                    秒
                  </q-avatar>
                </template>
              </q-input>
            </div>

          </div>

          <!-- 顺序执行 -->
          <div v-show="runModel=='sequential'">
            <div class="q-pa-md">拖动测试用例调整运行的先后顺序</div>
            <q-list separator>
              <draggable v-model="runOrderArray"
                         :disabled="runBtnDisable">
                <q-item clickable
                        v-for="(item,index) in runOrderArray"
                        :key="index"
                        :disable="runBtnDisable">
                  <q-item-section>
                    <q-item-label>{{item.name}}</q-item-label>

                    <q-item-label v-show="item.runStatus=='没有运行'"
                                  style="font-size:14px;width:150px;word-wrap:break-word;">{{item.runStatus}}</q-item-label>
                    <q-item-label v-show="item.runStatus=='正在运行' || item.runStatus=='运行结束' || item.runStatus=='停止运行'"
                                  style="font-size:14px;width:150px;word-wrap:break-word; color:green">{{item.runStatus}}</q-item-label>
                    <q-item-label v-show="item.runStatus=='运行失败'"
                                  style="font-size:14px;width:150px;word-wrap:break-word; color:red">{{item.runStatus}}</q-item-label>
                  </q-item-section>

                  <q-item-section avatar>{{index+1}}</q-item-section>
                </q-item>

              </draggable>
            </q-list>
            <q-separator />
          </div>
        </div>

        <q-card-actions align="right">
          <q-btn flat
                 label="关闭"
                 color="primary"
                 :disable="runBtnDisable"
                 @click="runCancelTestCase" />
          <q-btn flat
                 label="运行"
                 color="primary"
                 :disable="runBtnDisable"
                 @click="runTestCase" />
        </q-card-actions>
      </q-card>
      <!-- 控制台 -->
      <q-card style="width: 100%; max-width: 40vw;float:right;">
        <q-card-section>
          <div class="text-h6">运行结果</div>
        </q-card-section>

        <q-separator />
        <div class="q-pa-md">

          <q-list separator>
            <q-item clickable
                    v-for="(item,index) in runResults"
                    :key="index">
              <q-item-section>
                <q-item-label v-show="item.runStatus == '开始运行'"
                              style="color:green">{{item.name}}</q-item-label>
                <q-item-label v-show="item.runStatus == '开始停止'"
                              style="color:green">{{item.name}}</q-item-label>
                <q-item-label v-show="item.runStatus == '运行完毕'"
                              style="color:green">{{item.name}}</q-item-label>
                <q-item-label v-show="item.runStatus == '停止运行'"
                              style="color:green">{{item.name}}</q-item-label>
                <q-item-label v-show="item.runStatus == '正在运行' || item.runStatus == '运行结束' || item.runStatus == '已停止运行'"
                              style="color:green">{{item.name}}{{item.runStatus}}</q-item-label>
                <q-item-label v-show="item.runStatus == '运行失败'"
                              style="color:red">{{item.name}}{{item.runStatus}}</q-item-label>
              </q-item-section>

              <q-item-section avatar>{{item.date}}</q-item-section>
            </q-item>
          </q-list>
        </div>
        <q-separator />

      </q-card>
    </q-dialog>
  </div>
</template>

<script>
import * as Apis from "@/api/index"
import draggable from "vuedraggable"
export default {
  props: ['runFixedSH', 'selectedArr'],
  components: {
    draggable
  },
  watch: {
    runFixedSH (value) {
      console.log(value)
      this.runFixed = value;
    },
    selectedArr (value) {
      console.log(value)
      this.selected = value;
    },
  },
  data () {
    return {
      selected: [],
      // --------------------------------- 运行 --------------------------
      runBtnDisable: false,//当前是否在运行
      runFixed: false,//运行执行逻辑框
      stopRunFlag: false,//全部停止按钮Flag
      runModel: 'parallel',//运行模式
      runModelArray: [],//并行运行模式数组
      runOrderTimerArray: [],//并行运行模式定时器数组
      runOrderArray: [],//顺序运行模式数组
      runResults: [],//运行结果数组
    }
  },
  mounted () {
    this.runFixed = this.runFixedSH;
    this.selected = this.selectedArr;
  },
  methods: {
    //获得从机列表
    getSlaveHostsList (id, callback) {
      Apis.getSlaveHostsList({ caseId: id }).then((res) => {
        if (res.data.length == 0) {
          callback(true)
        } else {
          callback(false)
        }
      })
    },
    // --------------------- 运行 --------------------
    //打开运行选择模式界面
    openRunModel () {
      //判断是否选择测试用例
      if (this.selected.length != 0) {
        this.isSlaveHost()
      } else {
        this.$q.notify({
          position: 'top',
          message: '提示',
          caption: '请选择测试用例',
          color: 'red',
        })
      }
    },
    // 判断当前运行的测试用例是否运行或者包含从主机，必须包含从主机才能进行运行
    isSlaveHost () {
      this.$q.loading.show();
      let runArray = [];
      //------------------------------- 判断是否有正在运行的测试用例 ------------------------------- 
      for (let i = 0; i < this.selected.length; i++) {
        if (this.selected[i].status == '正在运行') {
          runArray.push(this.selected[i].name)
        }
      }
      if (runArray.length != 0) {
        this.$q.notify({
          position: 'top',
          message: '提示',
          caption: `当前测试用例${runArray.join('，')}正在运行当中,请重新选择`,
          color: 'red',
        })
        this.$q.loading.hide();
        return;
      }

      //-------------------------------  判断当前选择的测试用例端口号是否重复 ------------------------------- 
      let rechecking = [];
      for (var i = 0; i < this.selected.length; i++) {
        for (var j = i + 1; j < this.selected.length; j++) {
          if (this.selected[i].masterHostID === this.selected[j].masterHostID && JSON.parse(this.selected[i]['configuration']).LocustMasterBindPort === JSON.parse(this.selected[j]['configuration']).LocustMasterBindPort) {
            rechecking.push(this.selected[i].name);
            rechecking.push(this.selected[j].name);
          }
        }
      }
      rechecking = unique(rechecking);
      if (rechecking.length != 0) {
        this.$q.notify({
          position: 'top',
          message: '提示',
          caption: `当前测试用例${rechecking.join('，')}主机端口号重复，请修改或者重新选择。`,
          color: 'red',
        })
        this.$q.loading.hide();
        return;
      }
      //数组去重
      function unique (arr) {
        const res = new Map();
        return arr.filter((a) => !res.has(a) && res.set(a, 1))
      }


      //------------------------------- 判断当前选择的测试用例下是否有从主机 ------------------------------- 
      for (let i = 0; i < this.selected.length; i++) {
        this.getSlaveHostsList(this.selected[i].id, (flag) => {
          if (flag) { runArray.push(this.selected[i].name) }
          if (i == this.selected.length - 1) {
            if (runArray.length != 0) {
              this.$q.notify({
                position: 'top',
                message: '提示',
                caption: `当前测试用例${runArray.join('，')}下没有从主机，请添加从主机再进行运行。`,
                color: 'red',
              })
              this.$q.loading.hide();
              return;
            } else {
              this.$q.loading.hide();
              this.runFixed = true;
              for (let i = 0; i < this.selected.length; i++) {
                this.runModelArray.push(this.selected[i]);
                this.runModelArray[i].executionTime = null;
                this.runModelArray[i].runStatus = '没有运行';
                this.runOrderArray.push(this.selected[i]);
                this.runOrderArray[i].runStatus = '没有运行';
              }
            }
          }
        })
      }
    },
    //运行TestCase
    runTestCase () {
      //判断是并行模式还是顺序模式
      if (this.runModel == 'parallel') {
        //并行模式执行
        this.runResults.push({
          name: '----- 测试用例并行模式开始运行 -----',
          runStatus: '开始运行',
          date: this.nowTime()
        })
        this.ParallelExecution();
      } else {
        //顺序模式执行
        this.runResults.push({
          name: '----- 测试用例顺序模式开始运行 -----',
          runStatus: '开始运行',
          date: this.nowTime()
        })
        this.OrderRun(0);
      }
      this.runBtnDisable = true;
    },
    //关闭运行TestCase
    runCancelTestCase () {
      this.runFixed = false;
      this.runBtnDisable = false;
      this.selected = [];
      this.runModelArray = [];
      this.runResults = [];
      this.runOrderArray = [];
      this.selected = [];
      this.$parent.closeRunModel();
    },
    //运行全部停止
    runAllStop () {
      if (this.stopRunFlag) {
        this.$q.notify({
          position: 'top',
          message: '提示',
          caption: '当前测试用例正在停止中',
          color: 'secondary',
        })
        return;
      }
      //判断是并行模式还是顺序模式
      let _this = this;
      let stopArr = []//停止的测试用例
      let stopArrnum = 0;
      //判断当前是否有在运行的测试用例
      if (this.runModel == 'parallel') {
        for (let i = 0; i < this.runModelArray.length; i++) {
          if (this.runModelArray[i].runStatus === '正在运行') {
            stopArr.push(this.runModelArray[i])
          }
        }
      } else {
        for (let i = 0; i < this.runOrderArray.length; i++) {
          if (this.runOrderArray[i].runStatus === '正在运行') {
            stopArr.push(this.runOrderArray[i])
          }
        }
      }

      //判断当前是否有在运行的测试用例
      if (stopArr.length) {
        this.runResults.push({
          name: '开始停止当前运行的测试用例',
          runStatus: '开始停止',
          date: _this.nowTime()
        })
        stop();
      } else {
        this.$q.notify({
          position: 'top',
          message: '提示',
          caption: '当前没有正在运行的测试用例',
          color: 'secondary',
        })
      }



      //停止测试用例
      function stop () {
        if (_this.runModel == 'parallel') {
          console.log(_this.runOrderTimerArray)
          _this.runOrderTimerArray.map(item => { clearTimeout(item) });
        }
        _this.stopRunFlag = true;
        if (stopArrnum == stopArr.length) {
          _this.runResults.push({
            name: '当前所有正在运行的测试用例',
            runStatus: '已停止运行',
            date: _this.nowTime()
          })
          if (_this.runModel == 'parallel') {
            _this.stopRunFlag = false;
            _this.runBtnDisable = false;
          }
          return;
        }
        let para = `?caseId=${stopArr[stopArrnum].id}`
        Apis.postTestCaseStop(para).then((res) => {
          console.log(res)
          stopArrnum++;
          stop();
        }).catch(err => {
          //判断当前是否没有停止成功
          if (String(err).indexOf('500') == -1) {
            Apis.postTestCaseStop(para).then((res) => {
              console.log(res)
              stopArrnum++;
              stop();
            })
          }
        })
      }

    },
    //--------------------------------------------------------- 并行运行 ---------------------------------------------------------
    //并行模式运行
    ParallelExecution () {
      for (let i = 0; i < this.runModelArray.length; i++) {
        let timer = setTimeout(() => {
          let para = `?caseId=${this.runModelArray[i].id}`
          Apis.postTestCaseRun(para).then((res) => {
            console.log(res)
            this.$set(this.runModelArray[i], 'runStatus', '正在运行')
            console.log(this.runModelArray[i])
            this.runResults.push({
              name: this.runModelArray[i].name,
              runStatus: '正在运行',
              date: this.nowTime()
            })
            this.getParallelRunStatus(i);
          }).catch(err => {
            console.log(err)
            //判断当前是否没运行起来
            if (String(err).indexOf('500') == -1) {
              console.log('`当前测试用例${this.runModelArray[i].name}没有启动成功，正在重新启动`');
              Apis.postTestCaseRun(para).then((res) => {
                console.log(res)
                this.$set(this.runModelArray[i], 'runStatus', '正在运行')
                console.log(this.runModelArray[i])
                this.runResults.push({
                  name: this.runModelArray[i].name,
                  runStatus: '正在运行',
                  date: this.nowTime()
                })
                this.getParallelRunStatus(i);
              }).catch(err => {
                console.log(err, `当前测试用例${this.runModelArray[i].name}启动又一次失败`);
                this.$set(this.runModelArray[i], 'runStatus', '运行失败')
                this.runResults.push({
                  name: this.runModelArray[i].name,
                  runStatus: '运行失败',
                  date: this.nowTime()
                })
              })
              this.$set(this.runModelArray[i], 'runStatus', '运行失败')
              this.runResults.push({
                name: this.runModelArray[i].name,
                runStatus: '运行失败',
                date: this.nowTime()
              })
            } else {
              this.$set(this.runModelArray[i], 'runStatus', '运行失败')
              this.runResults.push({
                name: this.runModelArray[i].name,
                runStatus: '运行失败',
                date: this.nowTime()
              })
            }
          })
        }, this.runModelArray[i].executionTime * 1000);
        this.runOrderTimerArray.push(timer)
      }
    },
    //正则验证并行运行测试用例是否正确
    forceUpdate (val, index) {
      this.$set(this.runModelArray[index], 'executionTime', Number(val.replace(/[^\d]/g, "").replace(/^0/g, "")));
      this.$forceUpdate();
    },
    //获取当前时间
    nowTime () {
      let now = new Date();
      let _month = (10 > (now.getMonth() + 1)) ? '0' + (now.getMonth() + 1) : now.getMonth() + 1;
      let _day = (10 > now.getDate()) ? '0' + now.getDate() : now.getDate();
      let _hour = (10 > now.getHours()) ? '0' + now.getHours() : now.getHours();
      let _minute = (10 > now.getMinutes()) ? '0' + now.getMinutes() : now.getMinutes();
      let _second = (10 > now.getSeconds()) ? '0' + now.getSeconds() : now.getSeconds();
      return now.getFullYear() + '-' + _month + '-' + _day + ' ' + _hour + ':' + _minute + ':' + _second;
    },
    //查看并行模式运行的测试用例状态
    getParallelRunStatus (index) {
      Apis.getTestCaseStatus({ caseId: this.runModelArray[index].id }).then((res) => {
        if (res.data) {
          //如果还在运行则继续监听当前测试用例的状态
          setTimeout(() => {
            this.getParallelRunStatus(index);
          }, 3000)
        } else {
          //判断当前是否点击全部停止按钮是的话不提示运行结束
          this.$set(this.runModelArray[index], 'runStatus', '运行结束');
          this.runResults.push({
            name: this.runModelArray[index].name,
            runStatus: '运行结束',
            date: this.nowTime()
          })
          //如果全部执行完成则打开按钮的点击
          let IsItAllRunning = false;
          for (let i = 0; i < this.runModelArray.length; i++) {
            if (this.runModelArray[i].runStatus == '没有运行' || this.runModelArray[i].runStatus == '正在运行') {
              IsItAllRunning = true;
              break;
            }
          }
          if (!IsItAllRunning) { this.runBtnDisable = false; }
        }
      })
    },
    //--------------------------------------------------------- 顺序运行 ---------------------------------------------------------
    //顺序运行TestCase
    OrderRun (index) {
      console.log(index, this.runOrderArray)
      if (index == this.runOrderArray.length) {
        this.runResults.push({
          name: '----- 当前所有测试用例已运行完毕 -----',
          runStatus: '运行完毕',
          date: this.nowTime()
        })
        this.runBtnDisable = false;
        return;
      }
      let para = `?caseId=${this.runOrderArray[index].id}`
      Apis.postTestCaseRun(para).then((res) => {
        //顺序运行成功回调
        console.log(res)
        this.$set(this.runOrderArray[index], 'runStatus', '正在运行')
        this.runResults.push({
          name: this.runOrderArray[index].name,
          runStatus: '正在运行',
          date: this.nowTime()
        })
        this.getTestCaseStatus(index);
      }).catch(err => {
        //顺序运行失败回调
        console.log(err)
        this.$set(this.runOrderArray[index], 'runStatus', '运行失败')
        this.runResults.push({
          name: this.runOrderArray[index].name,
          runStatus: '运行失败',
          date: this.nowTime()
        })
        this.OrderRun(index += 1)
      })

    },
    //查看TestCase是否运行
    getTestCaseStatus (index) {
      Apis.getTestCaseStatus({ caseId: this.runOrderArray[index].id }).then((res) => {
        if (res.data) {
          //如果还在运行则继续监听当前测试用例的状态
          setTimeout(() => {
            this.getTestCaseStatus(index);
          }, 3000)
        } else {
          this.$set(this.runOrderArray[index], 'runStatus', '运行结束')
          this.runResults.push({
            name: this.runOrderArray[index].name,
            runStatus: '运行结束',
            date: this.nowTime()
          })
          if (this.stopRunFlag) {
            this.runBtnDisable = false;
            this.stopRunFlag = false;
            return;
          }
          this.OrderRun(index += 1)
        }
      })
    },
  }
}
</script>

<style lang="scss" scoped>
</style>