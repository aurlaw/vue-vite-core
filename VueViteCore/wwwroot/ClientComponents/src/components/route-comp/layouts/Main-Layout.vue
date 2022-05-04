<script setup>
// This starter template is using Vue 3 <script setup> SFCs
// Check out https://vuejs.org/api/sfc-script-setup.html#script-setup
import VLink from '../components/VLink.vue'

</script>

<template>
<section>
<ul class="nav nav-tabs">
    <li class="nav-item">Home</li>
    <li class="nav-item">About</li>
    <li class="nav-item">Under Construction</li>
  </ul>
  <article class="shadow">
    <div class="container">
      <slot></slot>
    </div>
  </article>
</section>
</template>


// <script>
// import VLink from '../components/VLink.vue'
// export default {
//   name: 'MainLayout',
//   components: {
//     VLink
//   },
//   methods: {
//       getLink(linkPath) {
//           return linkPath;
//       },
//       setActiveClass(linkPath) {
//         return window.location.pathname === linkPath ? 'active' : '';
//       },
//   }
// }
// </script>