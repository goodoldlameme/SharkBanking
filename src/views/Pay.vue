<template>
    <div class="pay-card-nav">
        <div class="flex-container">
            <router-link to="/pay/pay-any-bank">
                <i class="fa fa-credit-card fa-fw fa-1x"></i>
                 С карты любого банка</router-link>
            <router-link to="/pay/pay-your-bank">
                <i class="fa fa-desktop fa-fw fa-1x"></i>
                Из своего интернет-банка</router-link>
        </div>
        <router-view></router-view>
    </div>
</template>

<script>
export default {
  name: "Pay"
};
</script>

<style scoped>
.pay-card-nav {
  width: 90%;
  margin: 2% 5% 5% 7%;
}
.pay-card-nav a {
  margin-right: 5%;
  padding: 1% 2%;
  color: cadetblue;
  text-decoration: none;
  font-family: "Franklin Gothic Medium", "Arial Narrow", Arial, sans-serif;
  font-size: smaller;
  font-weight: lighter;
}

.pay-card-nav .router-link-active {
  color: black;
  background: lightgray;
}

@media (max-width: 630px) {
  .pay-card-nav .flex-container {
    flex-direction: column;
  }
}
</style>
