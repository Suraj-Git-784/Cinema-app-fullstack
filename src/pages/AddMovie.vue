<template>
  <div class="q-ma-md">
    <q-card>
      <div class="text-h5 movieHeading">Movie Details</div>
      <!-- @submit="createMovie" -->
      <q-form @reset="resetFields" class="q-gutter-md q-ma-md">
        <q-input
          filled
          v-model="movieName"
          label="Movie Name"
          lazy-rules
          :rules="[(val) => (val && val.length > 0) || 'Please type something']"
        />
        <q-input
          filled
          v-model="movieGenre"
          label="Genre Name"
          hint="Action, Adventure"
          lazy-rules
          :rules="[(val) => (val && val.length > 0) || 'Please type something']"
        />
        <q-input
          filled
          type="textarea"
          v-model="movieDescription"
          label="Description"
          lazy-rules
          :rules="[(val) => (val && val.length > 0) || 'Please type something']"
        />

        <div class="fieldNumber">
          <q-input
            filled
            type="number"
            v-model="movieGoldPrice"
            label="Gold Price"
            lazy-rules
            :rules="[
              (val) => (val !== null && val !== '') || 'Please type your age',
              (val) => (val > 0 && val < 100) || 'Please type a real age',
            ]"
          />
          <q-input
            filled
            type="number"
            v-model="movieGoldSeat"
            label="Gold Seat Availability"
            lazy-rules
            :rules="[
              (val) => (val !== null && val !== '') || 'Please type your age',
              (val) => (val > 0 && val < 100) || 'Please type a real age',
            ]"
          />
        </div>
        <div class="fieldNumber">
          <q-input
            filled
            type="number"
            v-model="movieSilverPrice"
            label="Silver Price"
            lazy-rules
            :rules="[
              (val) => (val !== null && val !== '') || 'Please type your age',
              (val) => (val > 0 && val < 100) || 'Please type a real age',
            ]"
          />
          <q-input
            filled
            type="number"
            v-model="movieSilverSeat"
            label="Silver Seat Availability"
            lazy-rules
            :rules="[
              (val) => (val !== null && val !== '') || 'Please type your age',
              (val) => (val > 0 && val < 100) || 'Please type a real age',
            ]"
          />
        </div>

        <q-input
          filled
          v-model="moviePoster"
          label="Poster Link"
          lazy-rules
          :rules="[(val) => (val && val.length > 0) || 'Please type something']"
        />
        <q-input
          filled
          v-model="movieTrailer"
          label="Trailer Link"
          lazy-rules
          :rules="[(val) => (val && val.length > 0) || 'Please type something']"
        />
        <div>
          <div class="text-subtitle1">Select Showtime:</div>
          <div v-for="(show, index) in showTimeList" :key="index">
            <q-checkbox
              v-model="movieShowTime"
              :label="convertTime(show.startTimeInd)"
              :val="show.startTimeInd"
            ></q-checkbox>
          </div>
          <div class="q-gutter-sm row">
            <q-input outlined v-model="addTime" mask="time" :rules="['time']">
              <template v-slot:append>
                <q-icon name="access_time" class="cursor-pointer">
                  <q-popup-proxy
                    cover
                    transition-show="scale"
                    transition-hide="scale"
                  >
                    <q-time v-model="addTime">
                      <div class="row items-center justify-end">
                        <q-btn
                          v-close-popup
                          label="Close"
                          color="primary"
                          flat
                        />
                        <q-btn
                          v-close-popup
                          @click="setAddTime"
                          label="Ok"
                          color="primary"
                          flat
                        />
                      </div>
                    </q-time>
                  </q-popup-proxy>
                </q-icon>
              </template>
            </q-input>
          </div>
          <div></div>
        </div>
        <div>
          <q-select
            label="Select City"
            v-model="movieCitySelect"
            :options="cityList"
            outlined
            @update:model-value="handleGetTheaterList(movieCitySelect)"
          ></q-select>
        </div>
        <div class="" v-if="theaterList.length > 0">
          <div class="text-subtitle1">Available Theaters:</div>
          <div v-for="(theater, index) in theaterList" :key="index">
            <q-radio
              v-model="theaterName"
              :val="theater.id"
              :label="theater.theaterName"
            ></q-radio>
          </div>
        </div>
        <div class="formButton">
          <q-btn
            class="AddMovieButton"
            label="Add"
            @click="handleCreateNewShowTime"
          />
          <q-btn
            label="Reset"
            type="reset"
            class="AddMovieButton q-ml-sm"
            flat
          />
        </div>
      </q-form>
    </q-card>
  </div>
</template>
<script setup lang="ts">
import { Ref, onMounted, ref, watch } from 'vue';
import axios from 'axios';
import { useQuasar } from 'quasar';

interface IMovie {
  movieId: number;
  movieName: string;
  movieGenre: string;
  movieDescription: string;
  movieGoldPrice: number;
  movieSilverPrice: number;
  movieGoldSeat: number;
  movieSilverSeat: number;
  moviePoster: string;
  movieReleaseDate: string;
  movieTrailer: string;
  theaterId: number;
}
interface ICity {
  cityId: number;
  cityName: string;
}
interface ITheater {
  id: number;
  theaterName: string;
  theaterLoaction: string;
}
interface IShowTime {
  showTimeId: number;
  startTimeInd: string;
}

onMounted(() => {
  AdminToken.value = localStorage.getItem('AdminToken');
  getCityList();
  getShowTime();
});
const emit = defineEmits(['getMovieList', 'someEvent']);
const props = defineProps({
  movieList: Array,
  openEditDialog: Boolean,
  getMovieId: {
    type: Number,
    default: undefined,
  },
  buttonChange: Boolean,
});

watch(
  () => props.buttonChange,
  () => {
    if (props.buttonChange === true) {
      getMovieById(props.getMovieId);
    }
    // Perform actions based on the new value of getMovieId
  }
);
const q = useQuasar();

const movieDialog: Ref<boolean> = ref(false);
const movieName: Ref<string> = ref('');
const movieGenre: Ref<string> = ref('');
const movieDescription: Ref<string> = ref('');
const edit: Ref<boolean> = ref(false);
const movieReleaseDate: Ref<string> = ref('');
const movieGoldSeat: Ref<number | null> = ref(null);
const movieSilverSeat: Ref<number | null> = ref(null);
const moviePoster: Ref<string> = ref('');
const movieTrailer: Ref<string> = ref('');
const movieSilverPrice: Ref<number> = ref(0);
const movieGoldPrice: Ref<number> = ref(0);
const _baseUrl = ref('https://localhost:7298');
const theaterList: Ref<Array<ITheater>> = ref([]);
const AdminToken: Ref<string | null> = ref('');
const theaterName: Ref<number | null> = ref(0);
const cityList: Ref<string[]> = ref([]);
const movieCitySelect: Ref<string> = ref('');
const showTimeList: Ref<Array<IShowTime>> = ref([]);
const movieShowTime = ref([]);
const addTime = ref('');
const movieId = ref(0);

/**
 * Function to add time for the movie
 */
const setAddTime = () => {
  let time = {
    startTimeInd: convertTimeFormat24(addTime.value),
  };
  axios
    .post(`${_baseUrl.value}/api/Cinema/addtime`, time, {
      headers: {
        Authorization: `Bearer ${
          AdminToken.value ? `${AdminToken.value}` : ''
        }`,
      },
    })
    .then(() => {
      getShowTime();
      addTime.value = '';
    })
    .catch((err) => {
      q.notify({
        message: `${err}`,
        color: 'red-5',
      });
    });
};

/**
 *  Funtion to get show times
 */
const getShowTime = () => {
  axios
    .get(`${_baseUrl.value}/api/Cinema/showtime`, {
      headers: {
        Authorization: `Bearer ${
          AdminToken.value ? `${AdminToken.value}` : ''
        }`,
      },
    })
    .then((response) => {
      showTimeList.value = response.data;
    })
    .catch((err) => {
      q.notify({
        message: `${err.message}`,
        color: 'red-5',
      });
    });
};
/**
 *  Function to get city list
 */
const getCityList = () => {
  axios
    .get(`${_baseUrl.value}/api/CityLocation`, {
      headers: {
        Authorization: `Bearer ${
          AdminToken.value ? `${AdminToken.value}` : ''
        }`,
      },
    })
    .then((response) => {
      response.data.forEach((element: ICity) => {
        cityList.value.push(element.cityName);
      });
    });
};

const convertVideoLink = (link: string) => {
  const convertedLink = link.replace('watch', 'embed').replace('?v=', '/');
  return convertedLink;
};

/**
 *  Function to get theater list
 */
const handleGetTheaterList = (city: string) => {
  axios
    .get(`${_baseUrl.value}/api/CityLocation/Search?city=${city}`, {
      headers: {
        Authorization: `Bearer ${
          AdminToken.value ? `${AdminToken.value}` : ''
        }`,
      },
    })
    .then((response) => {
      theaterList.value = response.data[0].theaterCityDtos;
    })
    .catch((err) => {
      q.notify({
        message: `${err.message}`,
        color: 'red-5',
      });
    });
};

/**
 *  Function to add movie to the list
 */
const createMovie = () => {
  let data = {
    movieName: movieName.value,
    movieGenre: movieGenre.value,
    movieDescription: movieDescription.value,
    moviePoster: moviePoster.value,
    movieReleaseDate: movieReleaseDate.value,
    movieGoldSeat: movieGoldSeat.value,
    movieSilverSeat: movieSilverSeat.value,
    movieGoldPrice: movieGoldPrice.value,
    movieSilverPrice: movieSilverPrice.value,
    movieTrailer: convertVideoLink(movieTrailer.value),
    theaterId: theaterName.value,
  };

  const movieData = data;
  const token = localStorage.getItem('AdminToken');

  axios
    .post(`${_baseUrl.value}/api/Cinema`, movieData, {
      headers: {
        Authorization: `Bearer ${token}`,
      },
    })
    .then((response) => {
      response.data.forEach((element: IMovie) => {
        movieId.value = element.movieId;
        theaterName.value = element.theaterId;
      });
      q.notify({
        icon: 'done',
        message: 'Your movie is added to the list',
        color: 'green-5',
      });
    })
    .catch((err) => {
      q.notify({
        message: `${err}`,
        color: 'red-5',
      });
    })
    .finally(() => {
      // resetFields();
    });
};

const handleCreateNewShowTime = () => {
  let data = {
    startTime: movieShowTime.value,
    MovieId: movieId.value,
    theaterId: theaterName.value,
  };
  console.log('Movie Show Time =>', data);
};

/**
 *  Function to get movie by Id
 */
const getMovieById = (movieId: number | undefined) => {
  const AdminToken = localStorage.getItem('AdminToken');
  const UserToken = localStorage.getItem('UserToken');
  axios
    .get(`${_baseUrl.value}/api/Cinema/${movieId}`, {
      headers: {
        Authorization: `Bearer ${
          AdminToken ? `${AdminToken}` : `${UserToken}`
        }`,
      },
    })
    .then((response) => {
      response.data.forEach((el: IMovie) => {
        movieName.value = el.movieName;
        movieGenre.value = el.movieGenre;
        movieDescription.value = el.movieDescription;
        movieReleaseDate.value = el.movieReleaseDate;
        movieGoldSeat.value = el.movieGoldSeat;
        movieSilverSeat.value = el.movieSilverSeat;
        movieGoldPrice.value = el.movieGoldPrice;
        movieSilverPrice.value = el.movieSilverPrice;
        moviePoster.value = el.moviePoster;
        movieTrailer.value = el.movieTrailer;
      });
    })
    .catch((err) => {
      console.log(err);
    });
};

const updateMovie = () => {
  let data = {
    movieId: props.getMovieId,
    movieName: movieName.value,
    movieGenre: movieGenre.value,
    movieDescription: movieDescription.value,
    moviePoster: moviePoster.value,
    movieTrailer: movieTrailer.value,
    movieReleaseDate: movieReleaseDate.value,
    movieGoldSeat: movieGoldSeat.value,
    movieSilverSeat: movieSilverSeat.value,
    movieGoldPrice: movieGoldPrice.value,
    movieSilverPrice: movieSilverPrice.value,
  };
  const token = localStorage.getItem('AdminToken');
  axios
    .put(`${_baseUrl.value}/api/Cinema/${props.getMovieId}`, data, {
      headers: {
        Authorization: `Bearer ${token}`,
      },
    })
    .then(() => {
      q.notify({
        icon: 'check',
        message: 'Movie list updated',
        color: 'green-5',
      });
    })
    .catch((err) => {
      q.notify({
        message: `${err}`,
        color: 'red-5',
      });
    })
    .finally(() => {
      edit.value = false;
      resetFields();
      emit('getMovieList');
    });
};

const handleOpenEditDialog = () => {
  edit.value;
  movieDialog.value = true;
  props.movieList;
  console.log(props.movieList);
};
defineExpose({
  handleOpenEditDialog,
});

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

/**
 *   Convert the backend date string to a Date object
 *
 * */
const handleDateFormat = (movieReleaseDate: string) => {
  const backendDate = movieReleaseDate;

  const date = new Date(backendDate);

  // Format the date as desired (YYYY/MM/DD)
  const formattedDate = `${date.getFullYear()}/${(date.getMonth() + 1)
    .toString()
    .padStart(2, '0')}/${date.getDate().toString().padStart(2, '0')}`;

  return formattedDate;
};

/**
 *  Function to reset fields
 */
const resetFields = () => {
  movieName.value = '';
  movieDescription.value = '';
  movieGenre.value = '';
  moviePoster.value = '';
  movieTrailer.value = '';
  movieReleaseDate.value = '';
  movieGoldSeat.value = null;
  movieSilverSeat.value = null;
  movieGoldPrice.value = 0;
  movieSilverPrice.value = 0;
  props.buttonChange;
  movieShowTime.value = [];
  theaterName.value = null;
};
</script>
<style scoped>
.AddMovie {
  color: black;
  background-color: rgb(236, 193, 0) !important;
}
.fieldNumber {
  justify-content: space-around;
  display: grid;
  grid-template-columns: 1fr 1fr 1fr;
  gap: 15px;
}
.formButton {
  text-align: center;
  padding-bottom: 10px;
}
.movieHeading {
  text-align: center;
}
.AddMovieButton {
  color: black;
  background-color: rgb(236, 193, 0) !important;
}
</style>
