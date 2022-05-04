<script setup>
import HelloWorld from './App/HelloWorld.vue'

defineProps({
  msg: String
})

</script>

<template>
    <HelloWorld :msg="msg">
      <img alt="Vue logo" src="../../../assets/logo.png" />
    </HelloWorld>

</template>
