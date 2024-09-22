<template>
  <div class="userLoginCard">
    <q-card class="q-cardUser q-mt-xl">
      <q-card-section>
        <strong class="text-h4">User Login</strong>
        <!-- {{ store.counter }} -->
      </q-card-section>
      <q-card-section>
        <q-form @submit="onSubmit" @reset="onReset">
          <!-- :rules="[
              (val) => !!val || 'Email is required',
              (val) =>
                /^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$/.test(
                  val
                ) || 'Please enter valid email',
            ]" -->
          <q-input
            ref="inputEmailRef"
            class="q-ma-sm"
            filled
            v-model="userName"
            dense
            label="Username"
          />
          <q-input
            class="q-ma-sm"
            label="Password"
            v-model="userPassword"
            :rules="[
              (val) => !!val || 'Password is required',
              (val) =>
                val.length >= 7 ||
                'Password must be at least 8 characters long',
              (val) =>
                /^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{7,}$/.test(
                  val
                ) ||
                'Password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character',
            ]"
            filled
            dense
            :type="isPwd ? 'password' : 'text'"
          >
            <template v-slot:append>
              <q-icon
                :name="isPwd ? 'visibility_off' : 'visibility'"
                class="cursor-pointer"
                @click="isPwd = !isPwd"
              />
            </template>
          </q-input>
          <q-btn class="q-ma-sm" type="submit" label="Login" color="primary" />
        </q-form>
      </q-card-section>
      <q-card-section>
        <q-chip
          class="user-register"
          icon="person"
          label="User Register"
          @click="handleUserResgister"
          clickable
        ></q-chip>
      </q-card-section>
    </q-card>
  </div>
</template>
<script setup lang="ts">
import axios from 'axios';
import { useQuasar } from 'quasar';
import { Ref, ref } from 'vue';
import { useRouter } from 'vue-router';

const router = useRouter();
const $q = useQuasar();
const userName: Ref<string> = ref('');
const userPassword: Ref<string> = ref('');
const isPwd: Ref<boolean> = ref(true);
const _baseURL: Ref<string> = ref('http://localhost:5175');

const onSubmit = () => {
  let data = {
    username: userName.value,
    password: userPassword.value,
  };
  axios
    .post(`${_baseURL.value}/api/auth/login`, data)
    .then((response) => {
      const UserToken = response.data.token;
      localStorage.setItem('UserToken', UserToken);
      router.push({
        path: '/theater',
        query: {
          role: 'user',
        },
      });
    })
    .catch((err) => {
      $q.notify({
        message: `${err.message}, Please register yourself!!`,
        color: 'red-4',
      });
    })
    .finally(() => onReset());
};

const onReset = () => {
  //
  userName.value = '';
  userPassword.value = '';
};

const handleUserResgister = () => {
  router.push({ path: '/user-register' });
};
</script>
<style scoped>
.userLoginCard {
  display: flex;
  align-items: center;
  justify-content: center;
}
.q-cardUser {
  width: 40%;
}
</style>
<style scoped>
.user-register {
  background-color: #f6c433;
}
</style>
