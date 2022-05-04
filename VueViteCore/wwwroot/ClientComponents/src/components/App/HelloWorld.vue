<script setup>
import { ref } from 'vue'

defineProps({
  msg: String
})

const count = ref(0)
</script>

<template>
  <div>
    <h1>{{ msg }}</h1>
    <slot></slot>
    <div>
      <button class="btn btn-primary" type="button" @click="count++">count is: {{ count }}</button>
    </div>
  </div>
</template>

<style scoped>
a {
  color: #42b983;
}
</style>
