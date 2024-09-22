import { defineStore } from 'pinia';

export const useLocationStore = defineStore('location', {
  state: () => ({
    userLocation: null as null | string,
    userLocationId: null as null | number,
  }),

  actions: {
    setUserLocation(location: null | string) {
      this.userLocation = location;
    },
    setUserLocationId(id: null | number) {
      this.userLocationId = id;
    },
  },
});
