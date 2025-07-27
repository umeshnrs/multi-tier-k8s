<script setup lang="ts">
import { onMounted, ref } from 'vue'
import { useDebounceFn } from '@vueuse/core'
import { useEvents } from '../../../composable/useEvents'
import EventCard from './EventCard.vue'
import { useRouter } from 'vue-router'

const router = useRouter()
const showDeleteConfirm = ref(false)
const eventToDelete = ref<string | null>(null)
const deleteError = ref('')

const searchInput = ref('')
const isSearchFocused = ref(false)
const {
  events,
  loading,
  error,
  getAvailabilityColor,
  currentPage,
  totalPages,
  hasPreviousPage,
  hasNextPage,
  handlePageChange,
  handleSearch,
  fetchEvents,
  deleteEvent
} = useEvents()

onMounted(() => {
  fetchEvents()
})

const debouncedSearch = useDebounceFn((value: string) => {
  handleSearch(value)
}, 500)

const handleSearchInput = () => {
  debouncedSearch(searchInput.value)
}

const clearSearch = () => {
  searchInput.value = ''
  handleSearch('')
}

const handleEditEvent = (id: string) => {
  router.push(`/events/edit/${id}`)
}

const handleDeleteClick = (id: string) => {
  eventToDelete.value = id
  showDeleteConfirm.value = true
  deleteError.value = ''
}

const confirmDelete = async () => {
  if (!eventToDelete.value) return
  
  try {
    const success = await deleteEvent(eventToDelete.value)
    if (success) {
      showDeleteConfirm.value = false
      eventToDelete.value = null
    } else {
      deleteError.value = 'Failed to delete event. Please try again.'
    }
  } catch (err) {
    console.error('Failed to delete event:', err)
    deleteError.value = 'Failed to delete event. Please try again.'
  }
}

const cancelDelete = () => {
  showDeleteConfirm.value = false
  eventToDelete.value = null
  deleteError.value = ''
}
</script>

<template>
  <div class="max-w-7xl mx-auto py-8 sm:px-6 lg:px-8">
    <!-- Delete Confirmation Modal -->
    <div
      v-if="showDeleteConfirm"
      class="fixed inset-0 bg-gray-500 bg-opacity-75 flex items-center justify-center z-50"
    >
      <div class="bg-white rounded-lg px-4 pt-5 pb-4 overflow-hidden shadow-xl transform transition-all sm:max-w-lg sm:w-full sm:p-6">
        <div class="sm:flex sm:items-start">
          <div class="mx-auto flex-shrink-0 flex items-center justify-center h-12 w-12 rounded-full bg-red-100 sm:mx-0 sm:h-10 sm:w-10">
            <svg
              class="h-6 w-6 text-red-600"
              xmlns="http://www.w3.org/2000/svg"
              fill="none"
              viewBox="0 0 24 24"
              stroke="currentColor"
            >
              <path
                stroke-linecap="round"
                stroke-linejoin="round"
                stroke-width="2"
                d="M12 9v2m0 4h.01m-6.938 4h13.856c1.54 0 2.502-1.667 1.732-3L13.732 4c-.77-1.333-2.694-1.333-3.464 0L3.34 16c-.77 1.333.192 3 1.732 3z"
              />
            </svg>
          </div>
          <div class="mt-3 text-center sm:mt-0 sm:ml-4 sm:text-left">
            <h3 class="text-lg leading-6 font-medium text-gray-900">
              Delete Event
            </h3>
            <div class="mt-2">
              <p class="text-sm text-gray-500">
                Are you sure you want to delete this event? This action cannot be undone.
              </p>
              <p
                v-if="deleteError"
                class="mt-2 text-sm text-red-600"
              >
                {{ deleteError }}
              </p>
            </div>
          </div>
        </div>
        <div class="mt-5 sm:mt-4 sm:flex sm:flex-row-reverse">
          <button
            type="button"
            class="w-full inline-flex justify-center rounded-md border border-transparent shadow-sm px-4 py-2 bg-red-600 text-base font-medium text-white hover:bg-red-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-red-500 sm:ml-3 sm:w-auto sm:text-sm"
            @click="confirmDelete"
          >
            Delete
          </button>
          <button
            type="button"
            class="mt-3 w-full inline-flex justify-center rounded-md border border-gray-300 shadow-sm px-4 py-2 bg-white text-base font-medium text-gray-700 hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500 sm:mt-0 sm:w-auto sm:text-sm"
            @click="cancelDelete"
          >
            Cancel
          </button>
        </div>
      </div>
    </div>

    <div class="px-4 sm:px-0 space-y-8">
      <!-- Header Section -->
      <div class="sm:flex sm:items-center sm:justify-between">
        <div class="sm:flex-auto">
          <h1 class="text-3xl font-semibold text-gray-900">
            Events
          </h1>
          <p class="mt-2 text-lg text-gray-700">
            A list of all upcoming events including venue and availability details.
          </p>
        </div>
        <div class="mt-4 sm:mt-0 sm:ml-16 sm:flex-none">
          <router-link
            to="/events/create"
            class="inline-flex items-center justify-center px-6 py-3 border border-transparent text-base font-medium rounded-md text-white bg-blue-600 hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500 transition-colors duration-200"
          >
            Add Event
          </router-link>
        </div>
      </div>

      <!-- Search Section -->
      <div class="max-w-2xl mx-auto">
        <div class="relative">
          <!-- Search Label (visually hidden) -->
          <label
            for="search"
            class="sr-only"
          >Search events</label>
          
          <!-- Search Container -->
          <div 
            class="relative group rounded-xl"
            :class="{
              'ring-2 ring-blue-500 ring-opacity-50': isSearchFocused
            }"
          >
            <!-- Search Icon -->
            <div class="absolute inset-y-0 left-0 pl-4 flex items-center pointer-events-none">
              <svg 
                class="h-5 w-5 text-gray-400 group-hover:text-gray-500" 
                xmlns="http://www.w3.org/2000/svg" 
                viewBox="0 0 20 20" 
                fill="currentColor"
                aria-hidden="true"
              >
                <path
                  fill-rule="evenodd"
                  d="M8 4a4 4 0 100 8 4 4 0 000-8zM2 8a6 6 0 1110.89 3.476l4.817 4.817a1 1 0 01-1.414 1.414l-4.816-4.816A6 6 0 012 8z"
                  clip-rule="evenodd"
                />
              </svg>
            </div>

            <!-- Search Input -->
            <input
              id="search"
              v-model="searchInput"
              type="search"
              class="block w-full bg-white pl-11 pr-12 py-3 border border-gray-300 rounded-xl text-base text-gray-900 placeholder-gray-500 focus:outline-none focus:border-blue-500 focus:ring-1 focus:ring-blue-500 hover:border-gray-400 transition-all duration-200 appearance-none"
              :class="{
                'pr-12': searchInput,
                'shadow-sm': !isSearchFocused,
                'shadow-md': isSearchFocused
              }"
              placeholder="Search events by title, venue, or description..."
              autocomplete="off"
              spellcheck="false"
              @input="handleSearchInput"
              @focus="isSearchFocused = true"
              @blur="isSearchFocused = false"
            >

            <!-- Clear Button -->
            <div 
              v-if="searchInput" 
              class="absolute inset-y-0 right-0 flex items-center pr-4"
            >
              <button
                type="button"
                class="rounded-full p-1 text-gray-400 hover:text-gray-500 hover:bg-gray-100 focus:outline-none focus:ring-2 focus:ring-offset-1 focus:ring-blue-500"
                @click="clearSearch"
              >
                <span class="sr-only">Clear search</span>
                <svg
                  class="h-5 w-5"
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
              </button>
            </div>
          </div>

          <!-- Search Status -->
          <div 
            v-if="loading && searchInput" 
            class="absolute mt-2 text-sm text-gray-500"
          >
            Searching...
          </div>
        </div>
      </div>

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
        class="rounded-lg bg-red-50 p-6"
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
              Error Loading Events
            </h3>
            <p class="mt-2 text-sm text-red-700">
              {{ error }}
            </p>
          </div>
        </div>
      </div>

      <!-- Events List -->
      <div
        v-else
        class="bg-white shadow rounded-lg overflow-hidden"
      >
        <ul
          role="list"
          class="divide-y divide-gray-200"
        >
          <li
            v-for="event in events"
            :key="event.id"
            class="space-y-4 mb-4"
          >
            <EventCard
              :event="event"
              :get-availability-color="getAvailabilityColor"
              @edit="handleEditEvent"
              @delete="handleDeleteClick"
            />
          </li>
        </ul>

        <!-- Pagination -->
        <div class="bg-gray-50 px-6 py-4 flex items-center justify-between border-t border-gray-200">
          <div class="flex-1 flex items-center justify-center">
            <nav class="relative z-0 inline-flex rounded-md shadow-sm -space-x-px">
              <button
                v-if="hasPreviousPage"
                class="relative inline-flex items-center px-3 py-2 rounded-l-md border border-gray-300 bg-white text-sm font-medium text-gray-500 hover:bg-gray-50 transition-colors duration-150"
                @click="handlePageChange(currentPage - 1)"
              >
                Previous
              </button>
              <button
                v-for="page in totalPages"
                :key="page"
                :class="[
                  currentPage === page
                    ? 'z-10 bg-blue-50 border-blue-500 text-blue-600'
                    : 'bg-white border-gray-300 text-gray-500 hover:bg-gray-50',
                  'relative inline-flex items-center px-4 py-2 border text-sm font-medium transition-colors duration-150'
                ]"
                @click="handlePageChange(page)"
              >
                {{ page }}
              </button>
              <button
                v-if="hasNextPage"
                class="relative inline-flex items-center px-3 py-2 rounded-r-md border border-gray-300 bg-white text-sm font-medium text-gray-500 hover:bg-gray-50 transition-colors duration-150"
                @click="handlePageChange(currentPage + 1)"
              >
                Next
              </button>
            </nav>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>