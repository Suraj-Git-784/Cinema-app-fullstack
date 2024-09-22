<template>
  <div>
    <q-layout view="hHh Lpr lFf">
      <q-header
        elevated
        :class="$q.dark.isActive ? 'bg-secondary' : 'bg-grey-1'"
      >
        <q-toolbar>
          <q-btn
            flat
            @click="drawer = !drawer"
            round
            dense
            icon="menu"
            color="black"
          />
          <q-toolbar-title>
            <q-img
              src="https://upload.wikimedia.org/wikipedia/en/a/ab/Star_Cinema_logo.svg"
              width="50px"
              height="40px"
          /></q-toolbar-title>
          <q-chip
            class="q-ml-sm"
            v-if="location"
            :label="setUserCity"
            icon="expand_more"
            @click="locationDialog = true"
            clickable
          ></q-chip>
        </q-toolbar>
      </q-header>
      <!-- City Dialog -->
      <div>
        <q-dialog v-model="locationDialog" persistent>
          <q-card>
            <q-card-section>
              <span class="q-ml-sm text-h5">Popular Cities</span>
            </q-card-section>
            <q-card-section class="city-card">
              <div v-for="(item, index) in cityLocation" :key="index">
                <q-btn
                  :label="item.name"
                  @click="handleSetCity(item)"
                  icon="location_city"
                  flat
                  no-caps
                  v-close-popup
                ></q-btn>
              </div>
            </q-card-section>
            <q-card-section>
              <div>
                <q-chip
                  class="GoldMovie"
                  label="City not listed? Click here"
                  clickable
                  @click="handleAddCity"
                ></q-chip>
              </div>
              <div v-if="addCity">
                <q-input
                  type="text"
                  label="Enter your city"
                  v-model="userAddCity"
                />
                <q-chip
                  class="GoldMovie"
                  label="Add"
                  icon="add"
                  clickable
                  @click="addCityLocation"
                ></q-chip>
              </div>
            </q-card-section>
          </q-card>
        </q-dialog>
      </div>
      <q-drawer
        v-model="drawer"
        show-if-above
        :width="200"
        :breakpoint="500"
        :class="$q.dark.isActive ? 'bg-grey-9' : 'bg-grey-3'"
      >
        <q-scroll-area class="fit">
          <q-list>
            <template v-for="(menuItem, index) in menuList" :key="index">
              <q-item
                clickable
                :active="menuItem.label === 'Outbox'"
                v-ripple
                :to="menuItem.link"
              >
                <q-item-section avatar>
                  <q-icon :name="menuItem.icon" />
                </q-item-section>
                <q-item-section>
                  {{ menuItem.label }}
                </q-item-section>
              </q-item>
              <q-separator :key="'sep' + index" v-if="menuItem.separator" />
            </template>
          </q-list>
        </q-scroll-area>
      </q-drawer>

      <q-page-container>
        <router-view :userLocation="setUserCity"></router-view>
      </q-page-container>
    </q-layout>
  </div>
</template>
<script setup lang="ts">
import { useQuasar } from 'quasar';
import { ref, Ref, onMounted } from 'vue';
import axios from 'axios';
import { useLocationStore } from 'stores/location';
import { useRouter } from 'vue-router';

interface ICity {
  cityId: number;
  name: string;
}

onMounted(() => {
  UserToken.value = localStorage.getItem('UserToken');
  AdminToken.value = localStorage.getItem('AdminToken');

  if (UserToken.value || AdminToken.value) {
    getCityLocation();
    locationDialog.value = true;
    return;
  } else {
    router.push({
      path: '/',
    });
  }
});

const userLocation = useLocationStore();
const q = useQuasar();
const router = useRouter();
const drawer = ref(true);
const location = ref(false);
const UserToken: Ref<string | null> = ref('');
const AdminToken: Ref<string | null> = ref('');
const locationDialog: Ref<boolean> = ref(false);
const cityLocation: Ref<Array<ICity>> = ref([]);
const setUserCity: Ref<string> = ref('');
const userAddCity: Ref<string> = ref('');
const addCity: Ref<boolean> = ref(false);
const _baseUrl = ref('http://localhost:5157');
const menuList = ref([
  {
    icon: 'theaters',
    label: 'Theater',
    link: '/theater',
    separator: true,
  },
  {
    icon: 'add',
    label: 'Add Moive',
    link: '/add-movie',
    separator: true,
  },
]);

/**
 *  Function to get the list of available city
 */
const getCityLocation = () => {
  if (AdminToken.value !== null || UserToken.value !== null) {
    axios
      .get(`${_baseUrl.value}/api/city`, {
        headers: {
          Authorization: `Bearer ${
            AdminToken.value ? `${AdminToken.value}` : `${UserToken.value}`
          }`,
        },
      })
      .then((response) => {
        cityLocation.value = response.data;
      })
      .catch((err) => {
        console.log(err);
      });
  } else {
    return;
  }
};

/**
 *
 * @param city Function to select city
 */
const handleSetCity = (city: ICity) => {
  setUserCity.value = city.name;
  location.value = true;
  userLocation.setUserLocation(city.name);
  userLocation.setUserLocationId(city.cityId);
};

/**
 * Function to show add city
 */
const handleAddCity = () => {
  addCity.value = !addCity.value;
};

/**
 * Function to add city in the list
 */

const addCityLocation = () => {
  if (!userAddCity.value || userAddCity.value.trim() == '') {
    return;
  }
  const data = {
    name: userAddCity.value,
  };
  axios
    .post(`${_baseUrl.value}/api/city`, data, {
      headers: {
        Authorization: `Bearer ${AdminToken.value}`,
      },
    })
    .then(() => {
      q.notify({
        message: 'City added successfully',
        color: 'green-5',
        icon: 'cloud',
      });
    })
    .catch((err) => {
      q.notify({
        message: `${err.message} ${err.response.data.title}`,
        color: 'red-5',
        icon: 'error_outline',
      });
    })
    .finally(() => {
      getCityLocation();
      userAddCity.value = '';
    });
};
</script>
<style>
.city-card {
  display: grid;
  grid-template-columns: 1fr 1fr 1fr;
  text-align: center;
}
.GoldMovie {
  background-color: #f6c433;
}
</style>
