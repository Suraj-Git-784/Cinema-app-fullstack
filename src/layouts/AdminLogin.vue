<template>
  <div class="adminLoginCard">
    <q-card class="q-cardAdmin q-mt-xl">
      <q-card-section>
        <strong class="text-h4">Admin Login</strong>
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
            v-model="adminUserName"
            dense
            label="Username"
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
          <q-btn class="q-ma-sm" type="submit" label="Login" color="primary" />
        </q-form>
      </q-card-section>
      <q-card-section>
        <q-chip
          class="admin-register"
          icon="admin_panel_settings"
          label="Admin Register"
          @click="handleAdminResgister"
          clickable
        ></q-chip>
      </q-card-section>
    </q-card>
  </div>
</template>
<script setup lang="ts">
import { ref, Ref, onMounted } from 'vue';
import axios from 'axios';
import { useQuasar } from 'quasar';
import { useRouter, useRoute } from 'vue-router';
import { storeToRefs } from 'pinia';
// import { useCounterStore } from 'stores/counter';
import { useAuthStore } from 'stores/routeGuard';

interface IAdmin {
  id: number;
  adminEmail: string;
  adminGender: string;
  adminName: string;
  adminNationality: string;
  adminPassword: string;
  adminPhone: string;
  adminAge: number;
}

const store = useAuthStore();

// const store = useCounterStore();
const isPwd: Ref<boolean> = ref(true);
const adminUserName = ref('');
const adminPassword = ref('');
const q = useQuasar();
const router = useRouter();
const route = useRoute();
const role: Ref<string> = ref('');
const registeredAdmin: Ref<Array<IAdmin>> = ref([]);
const isAdminEmail: Ref<boolean | null> = ref(null);
const isAdminPassword: Ref<boolean | null> = ref(null);
const inputEmailRef = ref();
const _baseUrl = ref('http://localhost:5157');

onMounted(() => {
  onReset();

  // getAdminList();
});

const onSubmit = () => {
  let data = {
    username: adminUserName.value,
    password: adminPassword.value,
  };

  axios
    .post(`${_baseUrl.value}/api/auth/login`, data)
    .then((response) => {
      localStorage.setItem('AdminToken', response.data.token);
      const token = localStorage.getItem('AdminToken');
      if (token !== null && token.length > 0) {
        router.push({
          path: '/theater',
          query: {
            role: 'admin',
          },
        });
      }
    })
    .catch((err) => {
      q.notify({
        message: `Login Failed.${err}`,
        color: 'red-6',
      });
    });

  // q.notify({
  //   message: 'Login Failed. Please register yourself',
  //   color: 'red-6',
  // });
  // onReset();
};

const handleAdminResgister = () => {
  router.push({ path: '/admin-register' });
};

const onReset = () => {
  //
  inputEmailRef.value?.resetValidation();
  adminUserName.value = '';
  adminPassword.value = '';
};
</script>
<style scoped>
.adminLoginCard {
  display: flex;
  align-items: center;
  justify-content: center;
}
.q-cardAdmin {
  width: 40%;
}
</style>
<style scoped>
.admin-register {
  background-color: #f6c433;
}
</style>
