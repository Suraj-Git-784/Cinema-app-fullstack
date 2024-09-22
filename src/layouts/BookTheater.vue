<template>
  <div>
    <div class="text-h3 theater-list">
      {{ movieDetail.movieName }}
    </div>
    <div class="q-ma-sm">
      <q-video :src="movieDetail.movieTrailer" :ratio="10 / 2" />
    </div>
    <div class="q-pa-md theater-list">
      <q-list v-for="(item, index) in theaterList" :key="index">
        <q-item>
          <q-item-section>
            <q-item-label
              ><q-icon
                v-if="role == 'admin'"
                class="q-ma-sm cursor-pointer"
                name="delete"
                size="20px"
                @click="deleteTheaterDialogOpen(item.id)"
                round
                dense
              ></q-icon>
              <strong
                >{{ item.theaterName }}, {{ item.theaterLocation }}</strong
              ></q-item-label
            >
          </q-item-section>
          <q-item-section class="movieTime"
            ><q-chip
              v-if="item.morningShow !== null"
              :label="convertTime(item.morningShow)"
              color="green"
              @click="handleOpenBookTicketDialog"
              outline
              square
              clickable
            ></q-chip
            ><q-chip
              v-if="item.afternoonShow !== null"
              :label="convertTime(item.afternoonShow)"
              @click="handleOpenBookTicketDialog"
              color="green"
              outline
              square
              clickable
            ></q-chip
            ><q-chip
              v-if="item.eveningShow"
              :label="convertTime(item.eveningShow)"
              @click="handleOpenBookTicketDialog"
              color="green"
              outline
              square
              clickable
            ></q-chip>
            <q-chip
              v-if="item.nightShow !== null"
              :label="convertTime(item.nightShow)"
              @click="handleOpenBookTicketDialog"
              color="green"
              outline
              square
              clickable
            ></q-chip>
            <q-chip
              v-if="item.midnightShow !== null"
              :label="convertTime(item.midnightShow)"
              @click="handleOpenBookTicketDialog"
              color="green"
              outline
              square
              clickable
            ></q-chip>
          </q-item-section>
          <q-btn
            class="q-ml-xl timeSlotAdd"
            size="14px"
            icon="edit"
            @click="handleOpenEditModal(item.id)"
            round
            dense
          ></q-btn>
          <theater-modal
            v-model="openTheaterDialog"
            :handle-button-change="handleButtonChange"
            @get-theater="getTheaterList"
            :theater-id="theaterId"
          />
        </q-item>
        <div></div>
        <q-separator spaced inset />
      </q-list>
      <q-chip
        v-if="role == 'admin'"
        label="Add Theater"
        icon="add"
        class="timeSlotAdd"
        @click="handleOpenAddTheater"
        clickable
      ></q-chip>
    </div>
    <!-- Book Ticket Dialog -->
    <div>
      <q-dialog v-model="bookTicketDialog" persistent>
        <q-card>
          <q-card-section align="center">
            <div class="text-h6">Select your choice!!! :)</div>
          </q-card-section>

          <q-card-actions align="center">
            <q-btn @click="handleSilverChoice" v-if="showSilverChoice">
              <div class="q-mr-sm goldMovie">
                <div>Silver</div>
                <div>
                  <strong>Rs. {{ movieDetail.movieSilverPrice }}</strong>
                </div>
                <div class="text-green-9">
                  Available: {{ movieDetail.movieSilverSeat }}
                </div>
              </div>
            </q-btn>
            <q-btn @click="handleGoldChoice" v-if="showGoldChoice">
              <div class="q-ml-sm silverMovie">
                <div>Gold</div>
                <div>
                  <strong>Rs. {{ movieDetail.movieGoldPrice }}</strong>
                </div>
                <div class="text-green-9">
                  Available: {{ movieDetail.movieGoldSeat }}
                </div>
              </div>
            </q-btn>
          </q-card-actions>
          <q-separator />
          <q-card-section align="center" v-if="showToSelectSeat">
            <div class="text-h6">How many seats?</div>
            <q-card-section class="q-pt-none">
              <q-radio v-model="ticketSeat" val="1" label="1" />
              <q-radio v-model="ticketSeat" val="2" label="2" />
              <q-radio v-model="ticketSeat" val="3" label="3" />
              <q-radio v-model="ticketSeat" val="4" label="4" />
              <q-radio v-model="ticketSeat" val="5" label="5" />
              <q-radio v-model="ticketSeat" val="6" label="6" />
              <q-radio v-model="ticketSeat" val="7" label="7" />
              <q-radio v-model="ticketSeat" val="8" label="8" />
              <q-radio v-model="ticketSeat" val="9" label="9" />
              <q-radio v-model="ticketSeat" val="10" label="10" />
            </q-card-section>
          </q-card-section>

          <q-card-actions align="center">
            <q-btn
              class="selectSeatButton"
              label="Select Seats"
              :disable="!ticketSeat"
              @click="handleBookTicket"
            />
          </q-card-actions>
        </q-card>
      </q-dialog>
    </div>
    <!-- Delete Theater Dialog -->
    <div>
      <q-dialog v-model="deleteTheaterDialog" persistent>
        <q-card>
          <q-card-section class="row items-center">
            <q-avatar icon="delete" color="primary" text-color="white" />
            <span class="q-ml-sm">Are you sure to delete this theater?</span>
          </q-card-section>

          <q-card-actions align="right">
            <q-btn flat label="Cancel" color="primary" v-close-popup />
            <q-btn
              flat
              label="Delete"
              color="primary"
              @click="deleteTheater"
              v-close-popup
            />
          </q-card-actions>
        </q-card>
      </q-dialog>
    </div>
  </div>
</template>
<script setup lang="ts">
import axios from 'axios';
import { useQuasar } from 'quasar';
import { Ref, onMounted, ref } from 'vue';
import { useRoute } from 'vue-router';
import TheaterModal from 'src/components/TheaterModal.vue';

interface ITheater {
  id: number;
  theaterName: string;
  theaterLocation: string;
  morningShow: string;
  afternoonShow: string;
  eveningShow: string;
  nightShow: string;
  midnightShow: string;
}

interface IMovieDetail {
  movieId: number | null;
  movieName: string;
  movieGoldPrice: number | null;
  movieSilverPrice: number | null;
  movieGoldSeat: number | null;
  movieSilverSeat: number | null;
  movieTrailer: string;
}

onMounted(() => {
  UserToken.value = localStorage.getItem('UserToken');
  AdminToken.value = localStorage.getItem('AdminToken');
  getTheaterList();
  getMovieById(routeQuery.movieID);
});

const q = useQuasar();
const handleButtonChange: Ref<boolean> = ref(false);
const bookTicketDialog: Ref<boolean> = ref(false);
const ticketSeat: Ref<number | null> = ref(null);
const theaterList: Ref<Array<ITheater>> = ref([]);
const openTheaterDialog: Ref<boolean> = ref(false);
const deleteTheaterDialog: Ref<boolean> = ref(false);
const _baseUrl: Ref<string> = ref('https://localhost:7298');
const theaterId: Ref<number | undefined> = ref();
const route = useRoute();
const routeQuery = route.query;
const movieDetail = ref<IMovieDetail>({
  movieId: null,
  movieName: '',
  movieGoldPrice: null,
  movieSilverPrice: null,
  movieGoldSeat: null,
  movieSilverSeat: null,
  movieTrailer: '',
});
const UserToken: Ref<string | null> = ref('');
const AdminToken: Ref<string | null> = ref('');
const role = ref(route.query.role);
const showToSelectSeat: Ref<boolean> = ref(false);
const showGoldChoice: Ref<boolean> = ref(true);
const showSilverChoice: Ref<boolean> = ref(true);

/**
 * Function to book ticket
 */

const handleOpenBookTicketDialog = () => {
  bookTicketDialog.value = true;
};
/**
 *  Function to book ticket
 */
const handleBookTicket = () => {
  if (
    showSilverChoice.value &&
    ticketSeat.value !== null &&
    movieDetail.value.movieSilverSeat
  ) {
    const movieId = movieDetail.value.movieId;
    movieDetail.value.movieSilverSeat =
      movieDetail.value.movieSilverSeat - ticketSeat.value;
    const data = [
      {
        op: 'replace',
        path: '/MovieSilverSeat',
        value: `${movieDetail.value.movieSilverSeat}`,
      },
    ];
    axios
      .patch(`${_baseUrl.value}/api/cinema/${movieId}`, data, {
        headers: {
          Authorization: `Bearer ${
            AdminToken.value ? `${AdminToken.value}` : `${UserToken.value}`
          }`,
        },
      })
      .then(() => {
        getMovieById(routeQuery.movieID);
      })
      .catch((err) => {
        q.notify({
          message: `${err.message}`,
          color: 'red-5',
        });
      });
    bookTicketDialog.value = false;
    ticketSeat.value = null;
    return;
  }

  if (
    showGoldChoice.value &&
    ticketSeat.value !== null &&
    movieDetail.value.movieGoldSeat
  ) {
    movieDetail.value.movieGoldSeat =
      movieDetail.value.movieGoldSeat - ticketSeat.value;
    const movieId = movieDetail.value.movieId;
    movieDetail.value.movieGoldSeat =
      movieDetail.value.movieGoldSeat - ticketSeat.value;
    const data = [
      {
        op: '/replace',
        path: '/MovieGoldSeat',
        value: `${movieDetail.value.movieGoldSeat}`,
      },
    ];
    axios
      .patch(`${_baseUrl.value}/api/cinema/${movieId}`, data, {
        headers: {
          Authorization: `Bearer ${
            AdminToken.value ? `${AdminToken.value}` : `${UserToken.value}`
          }`,
        },
      })
      .then(() => {
        getMovieById(routeQuery.movieID);
      })
      .catch((err) => {
        q.notify({
          message: `${err.message}`,
          color: 'red-5',
        });
      });
    bookTicketDialog.value = false;
    ticketSeat.value = null;
    return;
  }
};

/**
 *  Function to get the list of available theaters
 */
const getTheaterList = () => {
  UserToken.value
    ? axios.get(`${_baseUrl.value}/api/Theater/GetTheater`, {
        headers: {
          Authorization: `Bearer ${UserToken.value}`,
        },
      })
    : axios
        .get(`${_baseUrl.value}/api/Theater/GetTheater`, {
          headers: {
            Authorization: `Bearer ${AdminToken.value}`,
          },
        })
        .then((response) => {
          theaterList.value = response.data;
        })
        .catch((err) => {
          q.notify({
            message: `${err.message}`,
            color: 'red-5',
          });
        });
};

/**
 *  Function to get the movie of available theater
 * @param id
 */
const getMovieById = (id: unknown) => {
  axios
    .get(`${_baseUrl.value}/api/Cinema/${id}`, {
      headers: {
        Authorization: `Bearer ${
          AdminToken.value ? `${AdminToken.value}` : `${UserToken.value}`
        }`,
      },
    })
    .then((response) => {
      response.data.forEach((el: IMovieDetail) => {
        movieDetail.value.movieId = el.movieId;
        movieDetail.value.movieName = el.movieName;
        movieDetail.value.movieTrailer = el.movieTrailer;
        movieDetail.value.movieGoldPrice = el.movieGoldPrice;
        movieDetail.value.movieSilverPrice = el.movieSilverPrice;
        movieDetail.value.movieGoldSeat = el.movieGoldSeat;
        movieDetail.value.movieSilverSeat = el.movieSilverSeat;
      });
    })
    .catch((err) => {
      console.log(err);
    });
};

/**
 *  Function to open add theater dialog
 */
const handleOpenAddTheater = () => {
  openTheaterDialog.value = true;
  handleButtonChange.value = false;
};

/**
 * Function to open edit dialog
 * @param theater
 */
const handleOpenEditModal = (id: number) => {
  openTheaterDialog.value = true;
  handleButtonChange.value = true;
  theaterId.value = id;
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

/**
 *  Function to choose the silver ticket
 */
const handleSilverChoice = () => {
  showGoldChoice.value = false;
  showToSelectSeat.value = true;
  ticketSeat.value = null;
};
/**
 *  Function to choose the gold ticket
 */
const handleGoldChoice = () => {
  showSilverChoice.value = false;
  ticketSeat.value = null;
  showToSelectSeat.value = true;
};

/**
 *  Function to opne dialog
 * @param id
 */
const deleteTheaterDialogOpen = (id: number) => {
  deleteTheaterDialog.value = true;
  theaterId.value = id;
};

const deleteTheater = () => {
  if (AdminToken.value !== null && AdminToken.value.length > 0) {
    axios
      .delete(`${_baseUrl.value}/api/Theater/${theaterId.value}`, {
        headers: {
          Authorization: `Bearer ${AdminToken.value}`,
        },
      })
      .then(() => {
        q.notify({
          message: 'Deleted',
          color: 'red',
        });
      })
      .catch((err) => {
        console.log(err);
      })
      .finally(() => getTheaterList());
  } else return;
};
</script>
<style scoped>
.theater-list {
  width: 50%;
  margin: 0px auto;
}
.movieTime {
  display: grid;
  grid-template-columns: 1fr 1fr 1fr 1fr 1fr;
}
.timeSlotAdd {
  background-color: rgb(236, 193, 0);
  color: black;
}
.selectSeatButton {
  background-color: rgb(236, 193, 0);
  color: black;
  width: 100%;
}
.goldMovie {
  text-align: center;
}
.silverMovie {
  text-align: center;
}
</style>
