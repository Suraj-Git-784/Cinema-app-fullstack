import { RouteRecordRaw } from 'vue-router';

const routes: RouteRecordRaw[] = [
  {
    path: '/',
    component: () => import('layouts/MainLayout.vue'),
    meta: { auth: false },
  },
  {
    path: '/theater',
    component: () => import('layouts/DashboardLayout.vue'),
    children: [
      {
        path: '',
        component: () => import('pages/CityTheater.vue'),
        meta: { auth: true },
      },
      {
        path: '/theater-book',
        component: () => import('layouts/BookTheater.vue'),
        meta: { auth: true },
      },
      {
        path: '/movie-collection',
        component: () => import('pages/MovieCollection.vue'),
        meta: { auth: true },
      },
      {
        path: '/add-movie',
        component: () => import('pages/AddMovie.vue'),
        meta: { auth: true },
      },
    ],
  },
  // {
  //   path: '/',
  //   component: () => import('layouts/MainLayout.vue'),
  //   meta: { auth: false },
  //   children: [
  //     { path: '/index', component: () => import('pages/IndexPage.vue') },
  //   ],
  // },
  // {
  //   path: '/newLayout',
  //   component: () => import('layouts/NewMainLayout.vue'),
  //   children: [
  //     {
  //       path: '/newMovies',
  //       component: () => import('layouts/MovieCollection.vue'),
  //     },
  //   ],
  //   meta: { auth: true },
  // },
  // {
  //   path: '/movies',
  //   component: () => import('Pages/MovieCollection.vue'),
  //   meta: { auth: true },
  // },
  {
    path: '/login-admin',
    component: () => import('layouts/AdminLogin.vue'),
    meta: { auth: false },
  },
  {
    path: '/admin-register',
    component: () => import('layouts/AdminRegistration.vue'),
    meta: { auth: false },
  },
  {
    path: '/theater-book',
    component: () => import('layouts/BookTheater.vue'),
    meta: { auth: true },
  },
  {
    path: '/user-login',
    component: () => import('layouts/UserLogin.vue'),
    meta: { auth: false },
  },
  {
    path: '/user-register',
    component: () => import('layouts/UserRegistration.vue'),
    meta: { auth: false },
  },

  // Always leave this as last one,
  // but you can also remove it
  {
    path: '/:catchAll(.*)*',
    component: () => import('pages/ErrorNotFound.vue'),
  },
];

export default routes;
