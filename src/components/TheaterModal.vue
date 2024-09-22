<template>
  <div>
    <q-dialog v-model="theaterModal as boolean" persistent>
      <q-card>
        <q-card-section class="row items-center">
          <span class="q-ml-sm text-h5">{{ theaterName }}</span>
        </q-card-section>
        <q-form @submit="onSubmit">
          <q-card-section class="city-card">
            <q-input
              class="q-ma-sm"
              label="Theater Name"
              v-model="newTheaterName"
            />
            <q-input
              class="q-ma-sm"
              label="Theater Location"
              v-model="newTheaterLocation"
            />
            <q-input
              class="q-ma-sm"
              filled
              label="Change Morning show"
              v-model="newMorningShow"
            >
              <template v-slot:append>
                <q-icon name="access_time" class="cursor-pointer">
                  <q-popup-proxy
                    cover
                    transition-show="scale"
                    transition-hide="scale"
                  >
                    <q-time v-model="newMorningShow" mask="hh:mm A">
                      <div class="row items-center justify-end">
                        <q-btn
                          v-close-popup
                          label="Close"
                          color="primary"
                          flat
                        />
                      </div>
                    </q-time>
                  </q-popup-proxy>
                </q-icon>
              </template>
            </q-input>
            <q-input
              class="q-ma-sm"
              filled
              label="Change Afternoon show"
              v-model="newAfternoonShow"
            >
              <template v-slot:append>
                <q-icon name="access_time" class="cursor-pointer">
                  <q-popup-proxy
                    cover
                    transition-show="scale"
                    transition-hide="scale"
                  >
                    <q-time v-model="newAfternoonShow" mask="hh:mm A">
                      <div class="row items-center justify-end">
                        <q-btn
                          v-close-popup
                          label="Close"
                          color="primary"
                          flat
                        />
                      </div>
                    </q-time>
                  </q-popup-proxy>
                </q-icon>
              </template>
            </q-input>
            <q-input
              class="q-ma-sm"
              filled
              label="Evening Show time"
              v-model="newEveningShow"
            >
              <template v-slot:append>
                <q-icon name="access_time" class="cursor-pointer">
                  <q-popup-proxy
                    cover
                    transition-show="scale"
                    transition-hide="scale"
                  >
                    <q-time v-model="newEveningShow" mask="hh:mm A">
                      <div class="row items-center justify-end">
                        <q-btn
                          v-close-popup
                          label="Close"
                          color="primary"
                          flat
                        />
                      </div>
                    </q-time>
                  </q-popup-proxy>
                </q-icon>
              </template>
            </q-input>
            <q-input
              class="q-ma-sm"
              filled
              label="Night show Time"
              v-model="newNightShow"
            >
              <template v-slot:append>
                <q-icon name="access_time" class="cursor-pointer">
                  <q-popup-proxy
                    cover
                    transition-show="scale"
                    transition-hide="scale"
                  >
                    <q-time v-model="newNightShow" mask="hh:mm A">
                      <div class="row items-center justify-end">
                        <q-btn
                          v-close-popup
                          label="Close"
                          color="primary"
                          flat
                        />
                      </div>
                    </q-time>
                  </q-popup-proxy>
                </q-icon>
              </template>
            </q-input>
            <q-input
              class="q-ma-sm"
              filled
              label="Midnight Show Time"
              v-model="newMidightShow"
            >
              <template v-slot:append>
                <q-icon name="access_time" class="cursor-pointer">
                  <q-popup-proxy
                    cover
                    transition-show="scale"
                    transition-hide="scale"
                  >
                    <q-time v-model="newMidightShow" mask="hh:mm A">
                      <div class="row items-center justify-end">
                        <q-btn
                          v-close-popup
                          label="Close"
                          color="primary"
                          flat
                        />
                      </div>
                    </q-time>
                  </q-popup-proxy>
                </q-icon>
              </template>
            </q-input>
          </q-card-section>
          <q-card-section>
            <q-btn class="timeSlotAdd" label="cancel" v-close-popup></q-btn>
            <q-btn
              class="q-ml-sm timeSlotAdd"
              :label="props.handleButtonChange ? 'Update' : 'Add'"
              type="submit"
              v-close-popup
            ></q-btn>
          </q-card-section>
        </q-form>
      </q-card>
    </q-dialog>
  </div>
</template>
<script setup lang="ts">
import axios from 'axios';
import { useQuasar } from 'quasar';
import { Ref, ref } from 'vue';

const props = defineProps({
  handleButtonChange: Boolean,
  theaterId: Number
});
const emit = defineEmits(['getTheater']);
const q = useQuasar();
const theaterModal = defineModel();
const theaterName: Ref<string> = ref('');
const newTheaterName: Ref<string> = ref('');
const newTheaterLocation: Ref<string> = ref('');
const newMorningShow: Ref<string> = ref('');
const newAfternoonShow: Ref<string> = ref('');
const newEveningShow: Ref<string> = ref('');
const newNightShow: Ref<string> = ref('');
const newMidightShow: Ref<string> = ref('');
const _baseURL: Ref<string> = ref('https://localhost:7298');

const convertTimeFormat24 = (time12: string | null) => {
  if (time12 === '') {
    return (time12 = null);
  }
  if (time12 !== null && time12 !== undefined) {
    const [time, period] = time12.split(' ');
    const [hours12, minutes] = time.split(':');

    // Convert hours to number
    let hours = parseInt(hours12, 10);

    // Adjust hours for AM and PM
    if (period === 'PM' && hours < 12) {
      hours += 12; // Add 12 hours for PM except for 12 PM (noon)
    } else if (period === 'AM' && hours === 12) {
      hours = 0; // Convert 12 AM (midnight) to 0 hours
    }
    if (period == undefined && minutes == undefined) {
      const emptyTime = '00:00:00';
      return emptyTime;
    }

    // Create a new time string in 24-hour format
    const time24 = `${String(hours).padStart(2, '0')}:${minutes}:00`;

    return time24;
  }
};


const convertTime = (time: string) => {
  if (time !== null) {
    const [hours24, minutes] = time.split(':');

    let hours = parseInt(hours24, 10);

    const suffix = hours >= 12 ? 'PM' : 'AM';

    hours = hours % 12 || 12; // Convert 0 to 12 for 12 AM

    const paddedHours = hours.toString().padStart(2, '0');

    // Create a new time string in 12-hour format
    const time12 = `${paddedHours}:${minutes} ${suffix}`;

    return time12;
  } else {
    return;
  }
};

const onSubmit = () => {
  //
  if (!props.handleButtonChange) {
    let data = {
      theaterName: newTheaterName.value,
      theaterLocation: newTheaterLocation.value,
      morningShow: convertTimeFormat24(newMorningShow.value),
      afternoonShow: convertTimeFormat24(newAfternoonShow.value),
      eveningShow: convertTimeFormat24(newEveningShow.value),
      nightShow: convertTimeFormat24(newNightShow.value),
      midnightShow: convertTimeFormat24(newMidightShow.value),
    };

    const token = localStorage.getItem('AdminToken');
    if (!token) {
      return;
    }
    axios
      .post(`${_baseURL.value}/api/Theater`, data, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      })
      .then()
      .catch((err) => {
        q.notify({
          message: `${err.message}`,
          color: 'red-5',
        });
      })
      .finally(() => emit('getTheater'));
  } else if (props.handleButtonChange) {
    let data = {
      theaterName: newTheaterName.value,
      theaterLocation: newTheaterLocation.value,
      morningShow: convertTime(newMorningShow.value),
      afternoonShow: convertTime(newAfternoonShow.value),
      eveningShow: convertTime(newEveningShow.value),
      nightShow: convertTime(newNightShow.value),
      midnightShow: convertTime(newMidightShow.value),
    };

    const token = localStorage.getItem('AdminToken');
    if (!token) {
      return;
    }
    axios
      .post(`${_baseURL.value}/api/Theater`, data, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      })
      .then()
      .catch((err) => {
        q.notify({
          message: `${err.message}`,
          color: 'red-5',
        });
      })
      .finally(() => emit('getTheater'));
  } else{
    return;
  }
};
</script>
<style scoped>
.timeSlotAdd {
  background-color: rgb(236, 193, 0);
  color: black;
}
</style>
