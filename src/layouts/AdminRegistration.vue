<template>
  <q-layout view="lHh Lpr lFf">
    <div class="container">
      <q-card class="q-card-form">
        <q-card-section
          ><strong class="text-h5"> Admin Register</strong>
        </q-card-section>
        <div class="q-pa-md">
          <q-form @submit="onSubmit" @reset="onReset" class="q-gutter-md">
            <q-input
              filled
              v-model="adminUsername"
              label="Username*"
              lazy-rules
              :rules="[
                (val) => (val && val.length > 0) || 'Please type something',
              ]"
            />
            <q-input
              filled
              v-model="adminEmail"
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
              v-model="adminPassword"
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
              v-model="adminPhone"
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
              class="admin-login"
              text-color="black"
              icon="admin_panel_settings"
              @click="handleLoginAdmin"
              label="Admin Login"
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

const isPwd: Ref<boolean> = ref(true);
const adminUsername: Ref<string> = ref('');
const adminEmail: Ref<string> = ref('');
const adminPassword: Ref<string> = ref('');
const adminPhone: Ref<number | null> = ref(null);
const q = useQuasar();
const router = useRouter();
const _baseUrl = ref('http://localhost:5175');

const onSubmit = () => {
  let data = {
    username: adminUsername.value,
    email: adminEmail.value,
    password: adminPassword.value,
  };
  axios
    .post(`${_baseUrl.value}/api/auth/register-admin`, data)
    .then((response) => {
      console.log(response);

      q.notify({
        message: 'Registered Successfully',
        color: 'green-8',
      });
      router.push({ path: '/login-admin' });
    })
    .catch((err) => {
      q.notify({
        message: `${err}`,
        color: 'red-14',
      });
    })
    .finally(() => {
      //
      onReset();
    });
};

const handleLoginAdmin = () => {
  router.push({ path: '/login-admin' });
};

const onReset = () => {
  adminUsername.value = '';
  adminEmail.value = '';
  adminPassword.value = '';
};
</script>
<style scoped>
.admin-login {
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
