<template>
  <div class="q-ma-md">
    <div class="movieHeaders">
      <div>
        <!-- <add-movie
          v-if="role == 'admin'"
          ref="editDialog"
          :movie-list="movieCollection"
          @get-movie-list="getMovies"
          @some-event="changeAddDialogButton = false"
          :open-edit-dialog="movieDialog"
          :get-movie-id="movieId"
          :button-change="changeAddDialogButton"
        /> -->
        <filter-movie
          class="q-ml-sm"
          :movie-genre="movieFilterGenres"
          @result-movie-filter="handleFilterSearch"
          @default-movie-list="getMovies"
        />
      </div>
      <div class="q-mr-lg serachFilter">
        <div class="searchInput">
          <q-input
            outlined
            v-model="searchMovieName"
            label="Movie Name"
            dense
          />
        </div>
        <div>
          <q-btn
            class="searchIcon"
            icon="search"
            @click="handleSearchMovie"
            round
            dense
          ></q-btn>
        </div>
      </div>
    </div>
    <div class="text-h3 movieHeading">Available Movies</div>
    <div class="movieCollection">
      <div
        class="q-pa-md row"
        v-for="(movie, index) in movieCollection"
        :key="index"
      >
        <q-card class="my-card bg-secondary text-white movieCard">
          <q-img :src="movie.moviePoster" />
          <q-card-section>
            <div class="text-h6">{{ movie.movieName }}</div>
            <div class="text-subtitle3">
              <strong>Genre:</strong>
              {{ movie.movieGenre }}
            </div>
            <div class="text-subtitle4">
              <strong>Description:</strong>
              {{ movie.movieDescription }}
            </div>
            <div class="text-subtitle4">
              <strong>Release Date:</strong>
              {{ handleReleaseDate(movie.movieReleaseDate) }}
            </div>
            <q-chip
              v-if="role == 'admin'"
              label="Edit"
              icon="edit"
              color="green"
              @click="editMovieOpenModal(movie.movieId)"
              clickable
            ></q-chip>
          </q-card-section>
          <q-separator dark />

          <q-card-actions>
            <div class="bookTicket">
              <div>
                <div class="GoldMovie">
                  <q-avatar icon="workspace_premium"></q-avatar>
                  Gold
                </div>
                <div>Seats Available: {{ movie.movieGoldSeat }}</div>
                <div>Price: INR {{ movie.movieGoldPrice }}</div>
                <q-chip
                  label="Book"
                  icon="local_activity"
                  @click="
                    handleTheaterBookGold(movie.movieId, movie.movieGoldPrice)
                  "
                  clickable
                ></q-chip>
              </div>
              <div>
                <div>
                  <q-avatar icon="workspace_premium" color=""></q-avatar>
                  Silver
                </div>
                <div>Seats Available: {{ movie.movieSilverSeat }}</div>
                <div>Price: INR {{ movie.movieSilverPrice }}</div>
                <q-chip
                  label="Book"
                  icon="local_activity"
                  @click="
                    handleTheaterBookSilver(
                      movie.movieId,
                      movie.movieSilverPrice
                    )
                  "
                  clickable
                ></q-chip>
              </div>
            </div>
            <!-- <q-btn flat @click="editMovieOpenModal(movie)">Edit</q-btn>
            <q-btn flat @click="deleteMovieDialogOpen(movie.id)">Delete</q-btn> -->
          </q-card-actions>
        </q-card>
      </div>
    </div>
    <div>
      <q-dialog v-model="deleteMovieDialog" persistent>
        <q-card>
          <q-card-section class="row items-center">
            <q-avatar icon="delete" color="primary" text-color="white" />
            <span class="q-ml-sm">Are you sure to delete this movie?</span>
          </q-card-section>

          <q-card-actions align="right">
            <q-btn flat label="Cancel" color="primary" v-close-popup />
            <q-btn
              flat
              label="Delete"
              color="primary"
              @click="deleteMovie"
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
import { type Ref, onMounted, ref } from 'vue';
import { useRouter, useRoute, LocationQueryValue } from 'vue-router';
import FilterMovie from 'components/FilterMovie.vue';

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
}
interface ICity {
  id: number;
  cityName: string;
}

onMounted(() => {
  UserToken.value = localStorage.getItem('UserToken');
  AdminToken.value = localStorage.getItem('AdminToken');
  // role.value = route.query.role;
  const theaterId = route.query.theater as null;
  if (theaterId !== null || theaterId !== undefined) {
    getMovies(theaterId);
  }
});

// Variables
const q = useQuasar();
const router = useRouter();
const route = useRoute();
const role: Ref<string | null | LocationQueryValue[]> = ref('');
const movieCollection: Ref<Array<IMovie>> = ref([]);
const deleteMovieDialog: Ref<boolean> = ref(false);
const searchMovieName: Ref<string> = ref('');
const userCity: Ref<string> = ref('');
const movieId: Ref<number | undefined> = ref();
const _baseUrl = ref('https://localhost:7298');
const cityLocation: Ref<Array<ICity>> = ref([]);
const editDialog = ref();
const UserToken: Ref<string | null> = ref('');
const AdminToken: Ref<string | null> = ref('');
const changeAddDialogButton: Ref<boolean> = ref(false);
const movieFilterGenres: Ref<Array<string>> = ref([]);

/**
 *  Function to get the movies
 */
const getMovies = (id: number | null) => {
  if (AdminToken.value !== null && AdminToken.value.length > 0) {
    axios
      .get(`${_baseUrl.value}/api/Cinema/${id}`, {
        headers: {
          Authorization: `Bearer ${localStorage.getItem('AdminToken')}`,
        },
      })
      .then((response) => {
        movieCollection.value = response.data;
        if (Array.isArray(response.data)) {
          const genres = response.data.flatMap((movie) =>
            movie.movieGenre.split(', ')
          );
          const uniqueGenres = [...new Set(genres)];

          movieFilterGenres.value = uniqueGenres;
        } else {
          const genres = response.data.movieGenre.split(', ');
          movieFilterGenres.value = genres;
        }
      })
      .catch((err) => {
        console.log(err);
      });
  } else {
    axios
      .get(`${_baseUrl.value}/api/Cinema/GetMovies`, {
        headers: {
          Authorization: `Bearer ${UserToken.value}`,
        },
      })
      .then((response) => {
        movieCollection.value = response.data;
      })
      .catch((err) => {
        console.log(err);
      });
  }
};

/**
 *  Function to get the list of available city
 */
const getCityLocation = () => {
  if (AdminToken.value !== null || UserToken.value !== null) {
    axios
      .get(`${_baseUrl.value}/api/CityLocation`, {
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
 *  Function to add city
 */
const addCityLocation = () => {
  if (userCity.value.trim() == '') {
    return;
  }
  let data = {
    city: userCity.value,
  };
  axios
    .post(`${_baseUrl.value}/api/CityLocation`, data, {
      headers: {
        Authorization: `Bearer ${
          AdminToken.value ? `${AdminToken.value}` : `${UserToken.value}`
        }`,
      },
    })
    .then(() => {
      getCityLocation();
      userCity.value = '';
    })
    .catch((err) => {
      console.log(err);
    });
};

/**
 *  Function to handle movie select
 */
const handleTheaterBookSilver = (movieId: number, price: number) => {
  const userRole = route.query.role;
  if (userRole?.includes('admin')) {
    router.push({
      path: '/theater-book',
      query: {
        movieID: `${movieId}`,
        moviePrice: `${price}`,
        theaterType: 'Silver',
        role: userRole,
      },
    });
  } else {
    router.push({
      path: '/theater-book',
      query: {
        movieID: `${movieId}`,
        moviePrice: `${price}`,
        theaterType: 'Silver',
      },
    });
  }
};
/**
 *  Function to handle movie select
 */
const handleTheaterBookGold = (movieId: number, price: number) => {
  const userRole = route.query.role;
  if (userRole?.includes('admin')) {
    router.push({
      path: '/theater-book',
      query: {
        movieID: `${movieId}`,
        moviePrice: `${price}`,
        theaterType: 'Gold',
        role: userRole,
      },
    });
  } else {
    router.push({
      path: '/theater-book',
      query: {
        movieID: `${movieId}`,
        moviePrice: `${price}`,
        theaterType: 'Gold',
      },
    });
  }
};

/**
 *   Convert the backend date string to a Date object
 *
 * */
const handleReleaseDate = (movieReleaseDate: string) => {
  const backendDate = movieReleaseDate;

  const date = new Date(backendDate);

  // Format the date as desired (YYYY/MM/DD)
  const formattedDate = `${date.getFullYear()}/${(date.getMonth() + 1)
    .toString()
    .padStart(2, '0')}/${date.getDate().toString().padStart(2, '0')}`;

  return formattedDate;
};

/**
 *
 * @param id Function to open edit modal
 */
const editMovieOpenModal = (id: number) => {
  changeAddDialogButton.value = true;
  movieId.value = id;
  editDialog.value.handleOpenEditDialog();
};

/**
 *  Function to delete movie
 */
const deleteMovie = () => {
  axios
    .delete(`${_baseUrl.value}/api/Movie/${movieId.value}`)
    .then(() => {
      q.notify({
        message: 'Deleted Successfuly',
        color: 'red-5',
      });
    })
    .catch((err) => {
      q.notify({
        message: `${err}`,
        color: 'red-6',
      });
    })
    .finally(() => getMovies());
};

/**
 * Function to search the movies
 */

const handleSearchMovie = () => {
  if (searchMovieName.value.trimStart().trimEnd().length === 0) {
    return;
  }
  const searchTerm = encodeURIComponent(
    searchMovieName.value.trimStart().trimEnd()
  );
  axios
    .get(
      `${_baseUrl.value}/api/Cinema/search?searchItem=${searchTerm.trim()}`,
      {
        headers: {
          Authorization: `Bearer ${
            AdminToken.value ? `${AdminToken.value}` : `${UserToken.value}`
          }`,
        },
      }
    )
    .then((response) => {
      movieCollection.value = response.data;
    })
    .catch((err) => {
      console.log(err);
      movieCollection.value = [];
    });
};

const handleFilterSearch = (filteredMovie: Array<IMovie>) => {
  movieCollection.value = filteredMovie;
};
</script>
<style scoped>
.movieCard {
  width: 80%;
}

.movieCollection {
  display: grid;
  grid-template-columns: 1fr 1fr 1fr 1fr 1fr;
}
.GoldMovie {
  color: rgb(236, 193, 0);
}
.bg-secondary {
  background-color: black !important;
}
.bookTicket {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 20px;
}
.city-card {
  display: grid;
  grid-template-columns: 1fr 1fr 1fr;
  gap: 10px;
}

.movieHeaders {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.searchIcon {
  color: black;
  background-color: rgb(236, 193, 0) !important;
}
.serachFilter {
  display: flex;
  align-items: center;
  gap: 10px;
}
.movieHeading {
  text-align: center;
}
/* .searchInput {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 10px;
} */
</style>
