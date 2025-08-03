<script setup lang="ts">
import { computed, onMounted } from 'vue'
import { useEvents } from '../../composable/useEvents'
import { formatDate } from '../../utils/formatters'

const {
  events,
  loading,
  error,
  pageSize,
  fetchEvents
} = useEvents()

// Set initial values for dashboard
pageSize.value = 100 // Get more events for accurate stats

// Computed stats based on events
const stats = computed(() => {
  const now = new Date()
  const upcomingEvents = events.value.filter(event => new Date(event.startDate) > now)
  const pastEvents = events.value.filter(event => new Date(event.endDate) < now)
  
  return [
    { name: 'Total Events', value: events.value.length.toString() },
    { name: 'Upcoming Events', value: upcomingEvents.length.toString() },
    { name: 'Past Events', value: pastEvents.length.toString() },
    { 
      name: 'Available Seats', 
      value: events.value.reduce((sum, event) => sum + event.availableSeats, 0).toString()
    },
    {
      name: 'Total Seats',
      value: events.value.reduce((sum, event) => sum + event.totalSeats, 0).toString()
    }
  ]
})

// Get most recent events
const recentEvents = computed(() => {
  return [...events.value]
    .sort((a, b) => new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime())
    .slice(0, 5)
})

onMounted(() => {
  fetchEvents()
})
</script>

<template>
  <div class="max-w-7xl mx-auto py-6 sm:px-6 lg:px-8">
    <!-- Loading State -->
    <div
      v-if="loading"
      class="flex justify-center py-12"
    >
      <div class="inline-block h-8 w-8 animate-spin rounded-full border-4 border-solid border-blue-600 border-r-transparent" />
    </div>

    <!-- Error State -->
    <div
      v-else-if="error"
      class="rounded-lg bg-red-50 p-6 mb-6"
    >
      <div class="flex">
        <div class="flex-shrink-0">
          <svg
            class="h-5 w-5 text-red-400"
            xmlns="http://www.w3.org/2000/svg"
            viewBox="0 0 20 20"
            fill="currentColor"
          >
            <path
              fill-rule="evenodd"
              d="M10 18a8 8 0 100-16 8 8 0 000 16zM8.707 7.293a1 1 0 00-1.414 1.414L8.586 10l-1.293 1.293a1 1 0 101.414 1.414L10 11.414l1.293 1.293a1 1 0 001.414-1.414L11.414 10l1.293-1.293a1 1 0 00-1.414-1.414L10 8.586 8.707 7.293z"
              clip-rule="evenodd"
            />
          </svg>
        </div>
        <div class="ml-3">
          <h3 class="text-sm font-medium text-red-800">
            Error Loading Dashboard
          </h3>
          <p class="mt-2 text-sm text-red-700">
            {{ error }}
          </p>
        </div>
      </div>
    </div>

    <div v-else>
      <!-- Stats -->
      <div class="grid grid-cols-1 gap-5 sm:grid-cols-2 lg:grid-cols-4">
        <div
          v-for="item in stats"
          :key="item.name" 
          class="bg-white overflow-hidden shadow rounded-lg hover:shadow-md transition-shadow duration-200"
        >
          <div class="px-4 py-5 sm:p-6">
            <dt class="text-sm font-medium text-gray-500 truncate">
              {{ item.name }}
            </dt>
            <dd class="mt-1 text-3xl font-semibold text-gray-900">
              {{ item.value }}
            </dd>
          </div>
        </div>
      </div>

      <!-- Quick Actions -->
      <div class="mt-8">
        <div class="flex items-center justify-between mb-4">
          <h2 class="text-lg font-medium text-gray-900">
            Quick Actions
          </h2>
        </div>
        <div class="grid grid-cols-1 gap-4 sm:grid-cols-2">
          <router-link
            to="/events/create" 
            class="relative block p-4 border border-gray-200 rounded-lg hover:border-blue-500 hover:shadow-md bg-white transition-all duration-200"
          >
            <div class="flex items-center">
              <div class="flex-shrink-0">
                <svg
                  class="h-6 w-6 text-blue-600"
                  xmlns="http://www.w3.org/2000/svg"
                  fill="none"
                  viewBox="0 0 24 24"
                  stroke="currentColor"
                >
                  <path
                    stroke-linecap="round"
                    stroke-linejoin="round"
                    stroke-width="2"
                    d="M12 6v6m0 0v6m0-6h6m-6 0H6"
                  />
                </svg>
              </div>
              <div class="ml-4">
                <h3 class="text-sm font-medium text-gray-900">
                  Create New Event
                </h3>
                <p class="mt-1 text-sm text-gray-500">
                  Add a new event to your calendar
                </p>
              </div>
            </div>
          </router-link>
          <router-link
            to="/events" 
            class="relative block p-4 border border-gray-200 rounded-lg hover:border-blue-500 hover:shadow-md bg-white transition-all duration-200"
          >
            <div class="flex items-center">
              <div class="flex-shrink-0">
                <svg
                  class="h-6 w-6 text-blue-600"
                  xmlns="http://www.w3.org/2000/svg"
                  fill="none"
                  viewBox="0 0 24 24"
                  stroke="currentColor"
                >
                  <path
                    stroke-linecap="round"
                    stroke-linejoin="round"
                    stroke-width="2"
                    d="M4 6h16M4 10h16M4 14h16M4 18h16"
                  />
                </svg>
              </div>
              <div class="ml-4">
                <h3 class="text-sm font-medium text-gray-900">
                  View All Events
                </h3>
                <p class="mt-1 text-sm text-gray-500">
                  See your complete event list
                </p>
              </div>
            </div>
          </router-link>
        </div>
      </div>

      <!-- Recent Events -->
      <div class="mt-8">
        <div class="flex items-center justify-between mb-4">
          <h2 class="text-lg font-medium text-gray-900">
            Recent Events
          </h2>
          <router-link
            to="/events"
            class="text-sm text-blue-600 hover:text-blue-800"
          >
            View all
          </router-link>
        </div>
        <div class="bg-white shadow overflow-hidden sm:rounded-lg">
          <ul
            role="list"
            class="divide-y divide-gray-200"
          >
            <li
              v-for="event in recentEvents"
              :key="event.id" 
              class="px-6 py-4 hover:bg-gray-50 transition-colors duration-200"
            >
              <div class="flex items-center justify-between">
                <div class="min-w-0 flex-1">
                  <div class="flex items-center justify-between">
                    <p class="text-sm font-medium text-gray-900 truncate">
                      {{ event.title }}
                    </p>
                    <div class="ml-4">
                      <span
                        :class="[
                          event.availableSeats > 0 ? 'bg-green-100 text-green-800' : 'bg-red-100 text-red-800',
                          'px-2.5 py-0.5 rounded-full text-xs font-medium'
                        ]"
                      >
                        {{ event.availableSeats }} seats left
                      </span>
                    </div>
                  </div>
                  <div class="mt-2">
                    <p class="text-sm text-gray-500">
                      {{ event.venueName }}
                    </p>
                    <div class="mt-1 flex items-center text-xs text-gray-500">
                      <span>{{ formatDate(event.startDate) }}</span>
                      <span class="mx-2">•</span>
                      <span>{{ formatDate(event.endDate) }}</span>
                      <span
                        v-if="event.apiVersion"
                        class="ml-auto px-2 py-1 bg-blue-100 text-blue-800 rounded-full text-xs font-medium"
                      >
                        API {{ event.apiVersion }}
                      </span>
                    </div>
                  </div>
                </div>
              </div>
            </li>
          </ul>
        </div>
      </div>
    </div>
  </div>
</template> 