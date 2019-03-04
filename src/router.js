import Vue from "vue";
import Router from "vue-router";
import Pay from "./views/Pay.vue";
import RequestPayment from "./views/RequestPayment.vue";
import PayYourBank from "./components/PayYourBank.vue";
import PayAnyBank from "./components/PayAnyBank.vue";
import Login from "./views/Login.vue";
import MainPage from "./views/MainPage.vue";
import AdminPanel from "./views/AdminPanel.vue";
import AdminPayments from "./components/AdminPayments.vue";
import AdminRequests from "./components/AdminRequests.vue";

Vue.use(Router);

export default new Router({
  mode: "history",
  /*  base: process.env.BASE_URL,*/
  routes: [
    {
      path: "/",
      redirect: {
        name: "login"
      }
    },
    {
      path: "/main-page",
      name: "mainpage",
      component: MainPage,
      children: [
        {
          path: "/pay",
          component: Pay,
          children: [
            { path: "/pay/pay-your-bank", component: PayYourBank },
            { path: "/pay/pay-any-bank", component: PayAnyBank }
          ]
        },
        {
          path: "/request",
          component: RequestPayment
        }
      ]
    },
    {
      path: "/login",
      name: "login",
      component: Login
    },
    {
      path: "/admin",
      name: "admin",
      component: AdminPanel,
      children: [
        { path: "/admin/requested", component: AdminRequests },
        { path: "/admin/payments", component: AdminPayments }
      ]
    }
  ]
});
