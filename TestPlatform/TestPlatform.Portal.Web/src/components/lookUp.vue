<template>
  <q-dialog v-model="fixed">
    <q-card>
      <q-card-section>
        <div class="text-h6">Terms of Agreement</div>
      </q-card-section>

      <q-separator />
      <div class="new_input">
        <q-input v-model="text"
                 label="Name" />
        <q-input v-model="text"
                 label="EngineType" />
        <q-input v-model="text"
                 label="EngineType" />
      </div>

      <q-separator />

      <q-card-actions align="right">
        <q-btn flat
               label="Decline"
               color="primary"
               v-close-popup />
        <q-btn flat
               label="Accept"
               color="primary"
               v-close-popup />
      </q-card-actions>
    </q-card>
  </q-dialog>
</template>

<script>
export default {
  props: ['fixed'],
  name: 'lookUp'
}
</script>

<style lang="scss" scoped>
</style>

