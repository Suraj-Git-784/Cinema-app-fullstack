<template>
  <div>
    <!-- Theater List -->
    <div class="q-ma-md">
      <div class="text-h3 theaterHeading" v-if="theaterList.length > 0">
        Theaters Available
      </div>
      <div class="text-h3 theaterHeading" v-else>
        No theaters in your area!! :(
      </div>
      <q-list v-for="(theater, index) in theaterList" :key="index">
        <q-item>
          <q-item-section>
            <q-item-label>{{ theater.name }}</q-item-label>
            <q-item-label caption lines="2">{{
              theater.theaterLocation
            }}</q-item-label>
          </q-item-section>
          <q-item-section class="movieTime">
            <div v-for="(item, index) in showTimings" :key="index">
              <q-chip
                :label="getTimeFromDateTime(item.showTime)"
                color="green"
                outline
                square
                clickable
              ></q-chip>
            </div>
          </q-item-section>
          <q-item-section side top>
            <q-icon name="edit" @click="buttonClicked" />
          </q-item-section>
        </q-item>
        <q-separator />
      </q-list>
    </div>
  </div>
</template>
<script setup lang="ts">
import { Ref, ref, onMounted, watch } from 'vue';
import axios from 'axios';
import { useQuasar } from 'quasar';
import { useLocationStore } from 'src/stores/location';
import { useRouter } from 'vue-router';

interface ITheater {
  id: number;
  name: string;
  theaterLocation: string;
}

interface IShowTime {
  showTimingId: number;
  showTime: string;
}

onMounted(() => {
  UserToken.value = localStorage.getItem('UserToken');
  AdminToken.value = localStorage.getItem('AdminToken');
  if (userLocation.userLocationId !== null) {
    getTheater(userLocation.userLocationId);
  }
});

const userLocation = useLocationStore();
const router = useRouter();
const $q = useQuasar();
const _baseUrl = ref('http://localhost:5157');
const setUserCity: Ref<string> = ref('');
const UserToken: Ref<string | null> = ref('');
const AdminToken: Ref<string | null> = ref('');
const theaterList: Ref<Array<ITheater>> = ref([]);
const showTimings: Ref<Array<IShowTime>> = ref([]);

watch(
  () => userLocation.userLocationId,
  (newValue) => {
    if (newValue) {
      getTheater(newValue);
      showTimings.value = [];
    }
  }
);

const buttonClicked = () => {
  alert();
};
const handleThearterClick = (id: number) => {
  router.push({ path: '/movie-collection', query: { theater: `${id}` } });
};

/**
 *
 * @param id Function to get the list of theater based on the city location
 */
const getTheater = (id: number) => {
  if (setUserCity.value == null) {
    return;
  }
  axios
    .get(`${_baseUrl.value}/api/city/${id}`, {
      headers: {
        Authorization: `Bearer ${
          AdminToken.value ? `${AdminToken.value}` : `${UserToken.value}`
        }`,
      },
    })
    .then((response) => {
      theaterList.value = response.data.theaters;

      response.data.theaters.forEach((element) => {
        showTimings.value = element.showTimings;
      });
    })
    .catch((err) => {
      $q.notify({
        message: `${err.message}`,
        color: 'red-5',
      });
    });
};
/**
 *
 * @param dateTimeString Function to Convert hours to 12-hour format
 */
const getTimeFromDateTime = (dateTimeString: string) => {
  // Parse the input string to a Date object
  const dateTime = new Date(dateTimeString);

  // Extract hours and minutes
  const hours = dateTime.getHours();
  const minutes = dateTime.getMinutes();

  // Convert hours to 12-hour format
  const amPm = hours >= 12 ? 'PM' : 'AM';
  const hour = hours % 12 || 12; // The hour '0' should be '12'

  // Format the time as a string
  const formattedTime = `${hour.toString().padStart(2, '0')}:${minutes
    .toString()
    .padStart(2, '0')} ${amPm}`;

  return formattedTime;
};
</script>
<style scoped>
.theaterHeading {
  text-align: center !important;
}
.movieTime {
  display: grid;
  grid-template-columns: 1fr 1fr 1fr 1fr 1fr;
}
</style>
