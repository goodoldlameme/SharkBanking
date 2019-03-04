<template>
    <div id="app">
        <div id="nav">
            <router-link v-if="authenticated" to="/login" v-on:click.native="logout()" replace>Logout</router-link>
            <router-link v-if="authenticated" to="/admin">Админ панель</router-link>
        </div>
        <router-view @authenticated="setAuthenticated"></router-view>
    </div>
</template>

<script>
export default {
  name: "App",
  data() {
    return {
      authenticated: false,
      token: ""
    };
  },
  mounted() {
    if (!this.authenticated) {
      this.$router.replace({ name: "login" });
    }
  },
  methods: {
    setAuthenticated(status) {
      console.log("auth status changed");
      this.authenticated = status;
    },
    logout() {
      this.authenticated = false;
    }
  }
};
</script>

<style>
#app #nav a {
  margin: 5%;
  color: cadetblue;
}
</style>
