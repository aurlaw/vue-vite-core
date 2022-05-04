<template>
  <keep-alive>
    <component :is="ViewComponent"/>
  </keep-alive>
</template>
<script>
// import  Vue  from 'vue'
import { defineAsyncComponent, createApp } from 'vue'

import routes from './routes'
const NotFound = defineAsyncComponent(() => import('./pages/404.vue'));

export default {
  name: 'SimpleRouter',
  props: {
    routeStart: String,
  },
  data: () => ({
    currentRoute: window.location.pathname
  }),

  computed: {
    currentRouteAdjusted() {
      const p = this.currentRoute.replace(this.routeStart, "");
      if(p === "") {
        return "/";
      }  
      return p;
    },
    ViewComponent () {
        // console.log(this.currentRouteAdjusted);
      const matchingPage = routes[this.currentRouteAdjusted] || NotFound;
      return matchingPage;
      // return require(`./pages/${matchingPage}.vue`).default
    }
  },


  created () {
    // window.bus = createApp({});
    // window.addEventListener('popstate', () => {
    //   this.currentRoute = window.location.pathname;
    // });
    // let _this = this;
    // window.bus.$on('currentRoute', function (route) {
    //     console.log('currentRoute: ', route);
    //     _this.currentRoute = route;
    //   });        
  }
}
</script>
