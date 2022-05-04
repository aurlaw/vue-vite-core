// add the beginning of your app entry
import 'vite/modulepreload-polyfill'
import { createApp } from 'vue'
import App from './components/App.vue'
import Privacy from './components/Privacy.vue'
import Contact from './components/Contact.vue'
// import SimpleRouter from './components/route-comp/SimpleRouter.vue'


// const App = () => import('./components/App.vue');
// const Privacy = () => import('./components/Privacy.vue');
// const Contact = () => import('./components/Contact.vue');


import './sass/_custom.scss';
import "bootstrap/dist/js/bootstrap.min.js";

const APPS = { 
    App,
    Privacy,
    Contact
};
// createApp(App).mount('#app')
function renderAppInElement(el) {
    let id = el.id;
    const idArr = id.split('_');
    if(idArr.length > 1) {
        id = idArr[idArr.length-1];
    }

    const App = APPS[id];
    if (!App) return;

    // // get props from elements data attribute, like the post_id
    // const props = Object.assign({}, el.dataset);
    
    // console.log(props);
    createApp(App, { ...el.dataset }).mount(el)

}
document.querySelectorAll('.__vue-root').forEach(renderAppInElement);
