<script setup>
// This starter template is using Vue 3 <script setup> SFCs
// Check out https://vuejs.org/api/sfc-script-setup.html#script-setup
import VLink from '../components/VLink.vue'

      function getLink(linkPath) {
          return linkPath;
      }
      function setActiveClass(linkPath) {
        return window.location.pathname === linkPath ? 'active' : '';
      }


</script>

<template>
<section>
<ul class="nav nav-tabs">
    <li class="nav-item"><v-link class="nav-link" :class="setActiveClass('/home/router')" :href="getLink('/home/router')">Home</v-link></li>
    <li class="nav-item"><v-link class="nav-link" :class="setActiveClass('/home/router/about')" :href="getLink('/home/router/about')">About</v-link></li>
    <li class="nav-item"><v-link class="nav-link" :class="setActiveClass('/home/router/foo')" :href="getLink('/home/router/foo')">Under Construction</v-link></li>
  </ul>
  <article class="shadow">
    <div class="container">
      <slot></slot>
    </div>
  </article>
</section>
</template>