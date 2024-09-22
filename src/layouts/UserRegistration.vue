<template>
  <q-layout view="lHh Lpr lFf">
    <div class="container">
      <q-card class="q-card-form">
        <q-card-section
          ><strong class="text-h5"> User Register</strong>
        </q-card-section>
        <div class="q-pa-md">
          <q-form @submit="onSubmit" @reset="onReset" class="q-gutter-md">
            <q-input
              filled
              v-model="userName"
              label="Username*"
              lazy-rules
              :rules="[
                (val) => (val && val.length > 0) || 'Please type something',
              ]"
            />
            <q-input
              filled
              v-model="userEmail"
              label="Your Email *"
              lazy-rules
              :rules="[
                (val) => !!val || 'Email is required',
                (val) =>
                  /^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$/.test(
                    val
                  ) || 'Please enter valid email',
              ]"
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
            <!-- <q-input
              filled
              v-model="userPhone"
              label="Your Phone Number *"
              lazy-rules
              :rules="[
                (val) => (val && val.length > 0) || 'Please type something',
              ]"
            /> -->

            <div>
              <q-btn label="Sign Up" type="submit" color="primary" no-caps />
              <q-btn
                label="Reset"
                type="reset"
                color="primary"
                flat
                class="q-ml-sm"
                no-caps
              />
            </div>
          </q-form>
          <div class="q-mt-sm">
            <q-chip
              class="user-login"
              text-color="black"
              icon="person"
              @click="handleLoginUser"
              label="user Login"
              clickable
            >
            </q-chip>
          </div>
        </div>
      </q-card>
    </div>
    <q-page-container>
      <router-view />
    </q-page-container>
  </q-layout>
</template>
<script setup lang="ts">
import axios from 'axios';
import { useQuasar } from 'quasar';
import { Ref, ref } from 'vue';
import { useRouter } from 'vue-router';

const router = useRouter();
const $q = useQuasar();
const isPwd: Ref<boolean> = ref(true);
const userName: Ref<string> = ref('');
const userEmail: Ref<string> = ref('');
const userPassword: Ref<string> = ref('');
const _baseUrl: Ref<string> = ref('http://localhost:5175');

const onSubmit = () => {
  let data = {
    username: userName.value,
    email: userEmail.value,
    password: userPassword.value,
  };
  axios
    .post(`${_baseUrl.value}/api/auth/register`, data)
    .then((response) => {
      console.log(response);
      let status = response.data.status;
      let message = response.data.message;
      $q.notify({
        message: `${status}. ${message}`,
        color: 'green-5',
      });
      router.push({ path: '/user-login' });
    })
    .catch((err) => {
      console.log(err);
    })
    .finally(() => onReset());
  //
};

const handleLoginUser = () => {
  router.push({ path: '/user-login' });
  //
};

const onReset = () => {
  userName.value = '';
  userEmail.value = '';
  userPassword.value = '';
};
</script>
<style scoped>
.user-login {
  background-color: #f6c433;
}
.container {
  display: flex;
  justify-content: center;
}
.q-card-form {
  text-align: center;
  width: 40%;
}
</style>
