<script setup>
defineProps({
    href: {
      type:String,
      required: true
    }
})
function go() {
  window.bus.$emit('currentRoute', this.href);
  //   this.$root.$root.currentRoute = this.href
  window.history.pushState(null, "", this.href)
  //   history.go();
}
</script>

<template>
  <a
    :href="href">
    <slot></slot>
  </a>
</template>