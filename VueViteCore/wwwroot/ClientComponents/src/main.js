// add the beginning of your app entry
import 'vite/modulepreload-polyfill'
import { createApp } from 'vue'
import App from './App.vue'

createApp(App).mount('#app')
