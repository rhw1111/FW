<template>
  <q-dialog v-model="fixed"
            persistent>
    <q-card>
      <q-card-section>
        <div class="text-h6">Master Host List</div>
      </q-card-section>

      <q-separator />
      <div class="new_input">
        <q-item tag="label"
                v-for="(val,ind) in masterHostList"
                :key="ind"
                v-ripple>
          <q-item-section avatar>
            <q-radio v-model="selectIndex"
                     :val="ind"
                     color="teal" />
          </q-item-section>
          <q-item-section>
            <q-item-label>{{val.address}}</q-item-label>
          </q-item-section>
        </q-item>
      </div>

      <q-separator />

      <q-card-actions align="right">
        <q-btn flat
               label="取消"
               color="primary"
               @click="cancel" />
        <q-btn flat
               label="添加"
               color="primary"
               @click="confirm" />
      </q-card-actions>
    </q-card>
  </q-dialog>
</template>

<script>
export default {
  props: ['fixed', 'masterHostList', 'masterSelectIndex'],
  name: 'lookUp',
  data () {
    return {
      selectIndex: -1,
    }
  },
  watch: {
    masterSelectIndex (val) {
      this.selectIndex = val;
    }
  },
  methods: {
    confirm () {
      this.$emit('addMasterHost', this.selectIndex)
    },
    cancel () {
      this.$emit('cancelMasterHost')
    }
  }
}
</script>

<style lang="scss" scoped>
</style>

