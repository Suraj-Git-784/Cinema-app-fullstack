<template>
  <div>
    <q-chip class="filterMovie" label="Filters" icon="filter_list" clickable>
      <q-menu>
        <q-list class="filterMenu">
          <q-item>
            <q-item-section
              >Genres
              <div class="">
                <!-- <div v-for="(item, index) in props.movieGenre" :key="index">
                  <q-chip class="movieGenre" outline clickable square>{{
                    item
                  }}</q-chip>
                </div> -->
                <div>
                  <q-select
                  label="Select Genres"
                    multiple
                    :options="props.movieGenre"
                    v-model="movieGenere"
                    @update:model-value="handleSearchMovieFilter"
                    outlined
                  ></q-select>
                </div>
              </div>
            </q-item-section>
            <div></div>
          </q-item>
        </q-list>
      </q-menu>
    </q-chip>
  </div>
</template>
<script setup lang="ts">
import { onMounted, ref, Ref } from 'vue';
import axios from 'axios';

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

onMounted(() => {
  UserToken.value = localStorage.getItem('UserToken');
  AdminToken.value = localStorage.getItem('AdminToken');
});

const props = defineProps({
  movieGenre: Array,
});
const emit = defineEmits(['resultMovieFilter', 'defaultMovieList']);
const movieGenere = ref([]);
const UserToken: Ref<string | null> = ref('');
const AdminToken: Ref<string | null> = ref('');
const _baseUrl = ref('https://localhost:7298');

const handleSearchMovieFilter = () => {
  const data = movieGenere.value.join(', ');
  if (data === '' && data.length == 0) {
    emit('defaultMovieList');
    return;
  }
  const searchTerm = encodeURIComponent(data);
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
      const data = response.data as IMovie;
      emit('resultMovieFilter', data);
    })
    .catch((err) => {
      console.log(err);
    });
};
</script>
<style scoped>
.filterMovie {
  color: black;
  background-color: rgb(236, 193, 0) !important;
}
.movieGenre {
  color: rgb(214, 176, 8);
}
.movieGenreChip {
  display: grid;
  grid-template-columns: 1fr 1fr 1fr;
}

.filterMenu {
  width: 300px !important;
}
</style>
