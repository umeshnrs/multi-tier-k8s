<script setup lang="ts">
import type { Event } from '../types'
import { formatDate, formatPrice } from '../../../utils/formatters'

interface Props {
  event: Event
  getAvailabilityColor: (total: number, available: number) => string
}

const props = defineProps<Props>()
const emit = defineEmits<{
  (e: 'edit', id: string): void
  (e: 'delete', id: string): void
}>()

const handleEdit = () => {
  if (props.event.id) {
    emit('edit', props.event.id)
  }
}

const handleDelete = () => {
  if (props.event.id) {
    emit('delete', props.event.id)
  }
}
</script>

<template>
  <div class="p-6 hover:bg-gray-50 transition-colors duration-150">
    <div class="flex items-center justify-between">
      <div class="flex-1 min-w-0 space-y-3">
        <div class="flex items-center justify-between">
          <h3 class="text-xl font-semibold text-gray-900 truncate">
            {{ event.title }}
          </h3>
          <div class="flex items-center space-x-4">
            <span
              :class="[
                getAvailabilityColor(event.totalSeats, event.availableSeats),
                'px-3 py-1.5 text-sm font-medium rounded-full'
              ]"
            >
              {{ event.availableSeats }} seats left
            </span>
            <div class="flex items-center space-x-2">
              <button
                class="p-2 text-gray-400 hover:text-blue-500 hover:bg-blue-50 rounded-full transition-colors duration-200"
                title="Edit event"
                @click="handleEdit"
              >
                <svg
                  xmlns="http://www.w3.org/2000/svg"
                  class="h-5 w-5"
                  viewBox="0 0 20 20"
                  fill="currentColor"
                >
                  <path d="M13.586 3.586a2 2 0 112.828 2.828l-.793.793-2.828-2.828.793-.793zM11.379 5.793L3 14.172V17h2.828l8.38-8.379-2.83-2.828z" />
                </svg>
              </button>
              <button
                class="p-2 text-gray-400 hover:text-red-500 hover:bg-red-50 rounded-full transition-colors duration-200"
                title="Delete event"
                @click="handleDelete"
              >
                <svg
                  xmlns="http://www.w3.org/2000/svg"
                  class="h-5 w-5"
                  viewBox="0 0 20 20"
                  fill="currentColor"
                >
                  <path
                    fill-rule="evenodd"
                    d="M9 2a1 1 0 00-.894.553L7.382 4H4a1 1 0 000 2v10a2 2 0 002 2h8a2 2 0 002-2V6a1 1 0 100-2h-3.382l-.724-1.447A1 1 0 0011 2H9zM7 8a1 1 0 012 0v6a1 1 0 11-2 0V8zm5-1a1 1 0 00-1 1v6a1 1 0 102 0V8a1 1 0 00-1-1z"
                    clip-rule="evenodd"
                  />
                </svg>
              </button>
            </div>
          </div>
        </div>
        <div>
          <p class="text-base text-gray-600">
            {{ event.description }}
          </p>
        </div>
        <div class="flex items-center text-sm text-gray-500 space-x-6">
          <div>
            <span class="font-medium text-gray-700">Venue:</span> {{ event.venueName }}
          </div>
          <div>
            <span class="font-medium text-gray-700">Price:</span> {{ formatPrice(event.price) }}
          </div>
        </div>
        <div class="flex items-center text-sm text-gray-500 space-x-6">
          <div>
            <span class="font-medium text-gray-700">Starts:</span> {{ formatDate(event.startDate) }}
          </div>
          <div>
            <span class="font-medium text-gray-700">Ends:</span> {{ formatDate(event.endDate) }}
          </div>
        </div>
      </div>
    </div>
  </div>
</template> 