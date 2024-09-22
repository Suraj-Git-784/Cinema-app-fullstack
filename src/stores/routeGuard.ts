import { defineStore } from 'pinia';

export const useAuthStore = defineStore('authStore', {
  state: () => ({
    userLoggedIn : localStorage.getItem('UserToken'),
    adminLoggedIn: localStorage.getItem('AdminToken'),
    clearLocal: localStorage.clear(),
  }),
  actions: {
    clearLocalStorage() {
      localStorage.clear();
      this.userLoggedIn = null;
      this.adminLoggedIn = null;
    }
  }
})
