<template>
  <div class="TreeEntity">
    <div class="q-pa-md">
      <q-tree :nodes="simple"
              node-key="label"
              selected-color="primary"
              :selected.sync="selected"
              :expanded.sync="expanded"
              ref="Tree" />
    </div>
  </div>
</template>

<script>
export default {
  name: 'TreeEntity',
  data () {
    return {
      simple: [
        {
          label: '目录',
          children: [
            {
              ID: '1',
              label: '文件夹1',
              Type: 0,
              Value: '',
              ParentID: null,
            },
            {
              ID: '2',
              label: '文件夹2',
              Type: 0,
              Value: '',
              ParentID: null,
            },
            {
              ID: '3',
              label: '文件夹3',
              Type: 0,
              Value: '',
              ParentID: null,
            },
          ]
        }
      ],
      selected: ['文件夹1'],
      expanded: ['目录']
    }
  },
  mounted () {

  },
  methods: {

  }
}
</script>

<style lang="scss" scoped>
</style>