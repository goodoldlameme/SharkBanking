<template>
    <div id="login">
        <notifications position="top center" group="foo"/>
        <h1>Login</h1>
        <form>
        <input type="text" name="username" v-model="username" placeholder="Username"/>
        <input type="text" name="password" v-model="password" placeholder="Password"/>
        <div class="submit-button">
            <button
                    type="submit"
                    class="submit-button__title"
                    :disabled="isDisabled"
                    @click.prevent="login">Login</button>
            <span class="submit-button__bg"></span>
        </div>
        </form>
    </div>
</template>

<script>
import axios from "axios";
export default {
  name: "Login",
  data() {
    return {
      username: "",
      password: ""
    };
  },
  computed: {
    isDisabled() {
      return this.username === "" || this.password === "";
    }
  },
  methods: {
    login() {
      console.log("submit!");
      const user = { username: this.username, password: this.password };
      axios
        .post(
          "http://api.sharkbank.ru/auth",
          { body: user },
          { withCredentials: true }
        )
        .then(response => {
          if (response.status === 200) {
            if (response.headers !== undefined)
              this.$parent.token = response.headers["x-csrf-token"];
            console.log(this.$parent.token);
            this.$emit("authenticated", true);
            this.$router.replace({ name: "mainpage" });
            this.$notify({
              group: "foo",
              type: "success",
              title: "Авторизация прошла успешно!",
              text: "Ок"
            });
          }
        })
        .catch(e => {
          this.$notify({
            group: "foo",
            type: "error",
            title: "Авторизация не удалась: " + e,
            text: "Что-то пошло не так!"
          });
        });
    }
  }
};
</script>

<style scoped>
#login {
  margin: 5%;
}
.submit-button {
  margin-left: 5%;
}
input {
  margin-right: 2px;
}
</style>
