import { createRouter, createWebHistory } from 'vue-router'
import DashboardPage from '../features/dashboard/Dashboard.page.vue'
import EventsPage from '../features/events/pages/Events.page.vue'
import EventFormPage from '../features/events/pages/EventForm.page.vue'

const router = createRouter({
  history: createWebHistory(),
  routes: [
    {
      path: '/',
      name: 'dashboard',
      component: DashboardPage
    },
    {
      path: '/events',
      name: 'events',
      component: EventsPage
    },
    {
      path: '/events/create',
      name: 'event-create',
      component: EventFormPage
    },
    {
      path: '/events/edit/:id',
      name: 'event-edit',
      component: EventFormPage,
      props: true
    }
  ]
})

export default router 