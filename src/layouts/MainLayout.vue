<template>
  <div>
    <!-- <div class="animation">
      <strong>30% off for new user</strong>
    </div> -->
    <div class="container row">
      <div>
        <strong class="text-h1">Welcome!</strong>
        <p class="q-pl-sm text-subtitle1">
          Enjoy your all favorite movies near you
        </p>
        <div>
          <q-chip
            class="login-chip"
            icon="admin_panel_settings"
            label="Admin Login"
            @click="handleAdminLoginRoute"
            clickable
          />
          <q-chip
            class="login-chip"
            icon="person"
            label="User Login"
            @click="handleUserLoginPage"
            clickable
          />
        </div>
      </div>
      <q-img
        src="https://upload.wikimedia.org/wikipedia/en/a/ab/Star_Cinema_logo.svg"
        width="50%"
        height="40%"
      />
    </div>
  </div>
</template>
<script setup lang="ts">
import { ref } from 'vue';
import { useRouter } from 'vue-router';

const router = useRouter();

const show = ref(false);
const handleAdminLoginRoute = () => [router.push({ path: '/login-admin' })];
const handleUserLoginPage = () => [router.push({ path: '/user-login' })];
</script>
<style scoped>
/* .container {
  max-width: 1100px;
  padding-right: 15px;
  padding-left: 15px;
  margin: auto 0px;
} */
.container {
  align-items: center;
  justify-content: center;
}

.login-chip {
  background-color: #f6c433;
}
/* ANIMATION CSS-VUE */

.v-enter-active,
.v-leave-active {
  transition: opacity 2s ease;
}

.v-enter-from,
.v-leave-to {
  opacity: 0;
}
/* .animation {
  overflow: hidden;
  white-space: nowrap;
  animation: marquee 22s linear infinite;
} */
</style>
