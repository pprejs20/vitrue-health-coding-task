import { createRouter, createWebHistory } from 'vue-router'
import SuggestionsView from '../views/SuggestionsView.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'suggestions',
      component: SuggestionsView,
    },
  ],
})

export default router
