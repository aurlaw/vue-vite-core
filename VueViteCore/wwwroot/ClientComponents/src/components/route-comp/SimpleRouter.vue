<script setup>
// This starter template is using Vue 3 <script setup> SFCs
// Check out https://vuejs.org/api/sfc-script-setup.html#script-setup
import NotFound from './pages/404.vue'
import routes from './routes'

defineProps({
  routeStart: String
})

const currentRoute = window.location.pathname;

</script>
<template>
  <keep-alive>
    <!-- <component :is="ViewComponent"/> -->
  </keep-alive>
</template>

// <script>

https://www.thisdot.co/blog/vue-3-2-using-composition-api-with-script-setup
// import  Vue  from 'vue'
// import routes from './routes'
// const NotFound = () => import('./pages/404.vue');

// export default {
//   name: 'SimpleRouter',
//   props: {
//     routeStart: String,
//   },
//   data: () => ({
//     currentRoute: window.location.pathname
//   }),

//   computed: {
//     currentRouteAdjusted() {
//       const p = this.currentRoute.replace(this.routeStart, "");
//       if(p === "") {
//         return "/";
//       }  
//       return p;
//     },
//     ViewComponent () {
//         // console.log(this.currentRouteAdjusted);
//       const matchingPage = routes[this.currentRouteAdjusted] || NotFound;
//       return matchingPage;
//       // return require(`./pages/${matchingPage}.vue`).default
//     }
//   },


//   created () {
//     window.bus = new Vue({});
//     window.addEventListener('popstate', () => {
//       this.currentRoute = window.location.pathname;
//     });
//     let _this = this;
//     window.bus.$on('currentRoute', function (route) {
//         console.log('currentRoute: ', route);
//         _this.currentRoute = route;
//       });    
    
//   }
// }
// </script>
