<template>
    <div id="admin-panel">
        <div class="flex-container">
            <router-link to="/admin/requested">
                Запросы</router-link>
            <router-link to="/admin/payments">
                По карте</router-link>
        </div>
        <router-view></router-view>
    </div>
</template>

<script>
    export default {
        name: "AdminPanel"
    }
</script>

<style scoped>

</style>