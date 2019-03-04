<template>
    <div id="admin-requests">
        <div class="req" v-for="request in requests" :key="request.id">
            {{request}}
        </div>
    </div>
</template>

<script>
import axios from "axios";
export default {
  name: "AdminRequests",
  data() {
    return {
      requests: []
    };
  },
  mounted: function() {
    axios
      .get("http://api.sharkbank.ru/admin/request", { withCredentials: true })
      .then(resp => {
        console.log(resp.data);
        this.requests = resp.data;
      })
      .catch(e => console.log(e));
  }
};
</script>

<style scoped>
#admin-requests {
  margin: 5%;
}
#admin-requests > div {
  font-size: xx-small;
}
</style>
