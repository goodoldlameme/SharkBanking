import Vue from "vue";
import App from "./App.vue";
import router from "./router";
import VueTheMask from "vue-the-mask";
import Vuelidate from "vuelidate";
import Notifications from "vue-notification";
// import store from "./store";

Vue.use(VueTheMask);
Vue.use(Vuelidate);
Vue.use(Notifications);

Vue.config.productionTip = false;

new Vue({
  el: "#app",
  router: router,
  render: h => h(App)
});
