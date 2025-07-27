<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { z } from 'zod'
import { useForm, useField } from 'vee-validate'
import { toTypedSchema } from '@vee-validate/zod'
import { useRouter } from 'vue-router'
import { useEvents } from '../../../composable/useEvents'
import type { EventFormData } from '../types/index'

interface Props {
  id?: string
}

const props = defineProps<Props>()
const router = useRouter()
const { getEventById, createEvent, updateEvent } = useEvents()
const isSubmitting = ref(false)
const submitError = ref('')

const goBack = () => {
  router.back()
}

const eventSchema = z.object({
  title: z.string()
    .min(3, 'Title must be at least 3 characters')
    .max(100, 'Title must be less than 100 characters'),
  description: z.string()
    .min(10, 'Description must be at least 10 characters')
    .max(500, 'Description must be less than 500 characters'),
  startDate: z.string()
    .refine(date => new Date(date) > new Date(), 'Start date must be in the future'),
  endDate: z.string()
    .refine(date => new Date(date) > new Date(), 'End date must be in the future'),
  venueName: z.string()
    .min(3, 'Venue name must be at least 3 characters'),
  totalSeats: z.number()
    .int()
    .positive('Total seats must be positive'),
  availableSeats: z.number()
    .int()
    .min(0, 'Available seats cannot be negative'),
  price: z.number()
    .positive('Price must be positive')
})

const { handleSubmit, meta, setValues } = useForm<EventFormData>({
  validationSchema: toTypedSchema(eventSchema)
})

const { value: title, errorMessage: titleError, handleBlur: titleBlur } = useField<string>('title')
const { value: description, errorMessage: descriptionError, handleBlur: descriptionBlur } = useField<string>('description')
const { value: startDate, errorMessage: startDateError, handleBlur: startDateBlur } = useField<string>('startDate')
const { value: endDate, errorMessage: endDateError, handleBlur: endDateBlur } = useField<string>('endDate')
const { value: venueName, errorMessage: venueNameError, handleBlur: venueNameBlur } = useField<string>('venueName')
const { value: totalSeats, errorMessage: totalSeatsError, handleBlur: totalSeatsBlur } = useField<number>('totalSeats')
const { value: availableSeats, errorMessage: availableSeatsError, handleBlur: availableSeatsBlur } = useField<number>('availableSeats')
const { value: price, errorMessage: priceError, handleBlur: priceBlur } = useField<number>('price')

const isFormValid = computed(() => meta.value.valid)
const isEditMode = computed(() => !!props.id)
const pageTitle = computed(() => isEditMode.value ? 'Edit Event' : 'Create Event')
const submitButtonText = computed(() => {
  if (isSubmitting.value) {
    return isEditMode.value ? 'Updating Event...' : 'Creating Event...'
  }
  return isEditMode.value ? 'Update Event' : 'Create Event'
})

onMounted(async () => {
  if (props.id) {
    const event = await getEventById(props.id)
    if (event) {
      // Format dates for datetime-local input
      const formattedStartDate = new Date(event.startDate).toISOString().slice(0, 16)
      const formattedEndDate = new Date(event.endDate).toISOString().slice(0, 16)
      
      setValues({
        title: event.title,
        description: event.description,
        startDate: formattedStartDate,
        endDate: formattedEndDate,
        venueName: event.venueName,
        totalSeats: event.totalSeats,
        availableSeats: event.availableSeats,
        price: event.price
      })
    } else {
      // Handle invalid ID
      submitError.value = 'Event not found'
      router.push('/events')
    }
  }
})

const onSubmit = handleSubmit(async (values) => {
  submitError.value = ''
  isSubmitting.value = true
  
  try {
    if (isEditMode.value && props.id) {
      await updateEvent(props.id, values)
    } else {
      await createEvent(values)
    }
    router.push('/events')
  } catch (error) {
    submitError.value = error instanceof Error ? error.message : 'An error occurred while saving the event'
  } finally {
    isSubmitting.value = false
  }
})
</script>

<template>
  <div class="min-h-screen bg-gray-50 py-8">
    <div class="max-w-3xl mx-auto px-4 sm:px-6 lg:px-8">
      <!-- Header -->
      <div class="mb-8 flex items-center justify-between">
        <div>
          <h1 class="text-2xl font-bold text-gray-900">
            {{ pageTitle }}
          </h1>
          <p class="mt-2 text-sm text-gray-600">
            {{ isEditMode ? 'Update the event details below.' : 'Fill in the details to create a new event.' }}
          </p>
        </div>
        <button
          class="inline-flex items-center px-4 py-2 border border-gray-300 rounded-md shadow-sm text-sm font-medium text-gray-700 bg-white hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500"
          @click="goBack"
        >
          <svg
            xmlns="http://www.w3.org/2000/svg"
            class="h-5 w-5 mr-2"
            fill="none"
            viewBox="0 0 24 24"
            stroke="currentColor"
          >
            <path
              stroke-linecap="round"
              stroke-linejoin="round"
              stroke-width="2"
              d="M10 19l-7-7m0 0l7-7m-7 7h18"
            />
          </svg>
          Go Back
        </button>
      </div>

      <!-- Form Card -->
      <div class="bg-white rounded-lg shadow-sm border border-gray-200">
        <form
          class="space-y-6 p-6"
          @submit="onSubmit"
        >
          <!-- Error Message -->
          <div
            v-if="submitError"
            class="rounded-md bg-red-50 p-4"
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
                <p class="text-sm font-medium text-red-800">
                  {{ submitError }}
                </p>
              </div>
            </div>
          </div>

          <!-- Title -->
          <div class="space-y-1">
            <label
              for="title"
              class="block text-sm font-medium text-gray-700"
            >Title</label>
            <input
              id="title"
              v-model="title"
              type="text"
              class="w-full px-4 py-2 border border-gray-300 rounded-md focus:ring-2 focus:ring-blue-500 focus:border-transparent transition"
              :class="{ 'border-red-300': titleError }"
              placeholder="Enter event title"
              @blur="titleBlur"
            >
            <p
              v-if="titleError"
              class="text-sm text-red-600"
            >
              {{ titleError }}
            </p>
          </div>

          <!-- Description -->
          <div class="space-y-1">
            <label
              for="description"
              class="block text-sm font-medium text-gray-700"
            >Description</label>
            <textarea
              id="description"
              v-model="description"
              rows="4"
              class="w-full px-4 py-2 border border-gray-300 rounded-md focus:ring-2 focus:ring-blue-500 focus:border-transparent transition"
              :class="{ 'border-red-300': descriptionError }"
              placeholder="Describe your event"
              @blur="descriptionBlur"
            />
            <p
              v-if="descriptionError"
              class="text-sm text-red-600"
            >
              {{ descriptionError }}
            </p>
          </div>

          <!-- Date Range -->
          <div class="grid grid-cols-1 gap-6 sm:grid-cols-2">
            <div class="space-y-1">
              <label
                for="startDate"
                class="block text-sm font-medium text-gray-700"
              >Start Date</label>
              <input
                id="startDate"
                v-model="startDate"
                type="datetime-local"
                class="w-full px-4 py-2 border border-gray-300 rounded-md focus:ring-2 focus:ring-blue-500 focus:border-transparent transition"
                :class="{ 'border-red-300': startDateError }"
                @blur="startDateBlur"
              >
              <p
                v-if="startDateError"
                class="text-sm text-red-600"
              >
                {{ startDateError }}
              </p>
            </div>

            <div class="space-y-1">
              <label
                for="endDate"
                class="block text-sm font-medium text-gray-700"
              >End Date</label>
              <input
                id="endDate"
                v-model="endDate"
                type="datetime-local"
                class="w-full px-4 py-2 border border-gray-300 rounded-md focus:ring-2 focus:ring-blue-500 focus:border-transparent transition"
                :class="{ 'border-red-300': endDateError }"
                @blur="endDateBlur"
              >
              <p
                v-if="endDateError"
                class="text-sm text-red-600"
              >
                {{ endDateError }}
              </p>
            </div>
          </div>

          <!-- Venue -->
          <div class="space-y-1">
            <label
              for="venueName"
              class="block text-sm font-medium text-gray-700"
            >Venue Name</label>
            <input
              id="venueName"
              v-model="venueName"
              type="text"
              class="w-full px-4 py-2 border border-gray-300 rounded-md focus:ring-2 focus:ring-blue-500 focus:border-transparent transition"
              :class="{ 'border-red-300': venueNameError }"
              placeholder="Enter venue name"
              @blur="venueNameBlur"
            >
            <p
              v-if="venueNameError"
              class="text-sm text-red-600"
            >
              {{ venueNameError }}
            </p>
          </div>

          <!-- Seats and Price -->
          <div class="grid grid-cols-1 gap-6 sm:grid-cols-3">
            <div class="space-y-1">
              <label
                for="totalSeats"
                class="block text-sm font-medium text-gray-700"
              >Total Seats</label>
              <input
                id="totalSeats"
                v-model.number="totalSeats"
                type="number"
                min="1"
                class="w-full px-4 py-2 border border-gray-300 rounded-md focus:ring-2 focus:ring-blue-500 focus:border-transparent transition"
                :class="{ 'border-red-300': totalSeatsError }"
                @blur="totalSeatsBlur"
              >
              <p
                v-if="totalSeatsError"
                class="text-sm text-red-600"
              >
                {{ totalSeatsError }}
              </p>
            </div>

            <div class="space-y-1">
              <label
                for="availableSeats"
                class="block text-sm font-medium text-gray-700"
              >Available Seats</label>
              <input
                id="availableSeats"
                v-model.number="availableSeats"
                type="number"
                min="0"
                class="w-full px-4 py-2 border border-gray-300 rounded-md focus:ring-2 focus:ring-blue-500 focus:border-transparent transition"
                :class="{ 'border-red-300': availableSeatsError }"
                @blur="availableSeatsBlur"
              >
              <p
                v-if="availableSeatsError"
                class="text-sm text-red-600"
              >
                {{ availableSeatsError }}
              </p>
            </div>

            <div class="space-y-1">
              <label
                for="price"
                class="block text-sm font-medium text-gray-700"
              >Price</label>
              <div class="relative">
                <div class="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
                  <span class="text-gray-500 sm:text-sm">$</span>
                </div>
                <input
                  id="price"
                  v-model.number="price"
                  type="number"
                  min="0"
                  step="0.01"
                  class="w-full pl-7 pr-4 py-2 border border-gray-300 rounded-md focus:ring-2 focus:ring-blue-500 focus:border-transparent transition"
                  :class="{ 'border-red-300': priceError }"
                  @blur="priceBlur"
                >
              </div>
              <p
                v-if="priceError"
                class="text-sm text-red-600"
              >
                {{ priceError }}
              </p>
            </div>
          </div>

          <!-- Submit Button -->
          <div class="pt-4">
            <button
              type="submit"
              :disabled="!isFormValid || isSubmitting"
              class="w-full flex justify-center py-3 px-4 border border-transparent rounded-md shadow-sm text-sm font-medium text-white bg-blue-600 hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500 transition disabled:opacity-50 disabled:cursor-not-allowed"
            >
              <span v-if="isSubmitting">
                <svg
                  class="animate-spin -ml-1 mr-3 h-5 w-5 text-white"
                  xmlns="http://www.w3.org/2000/svg"
                  fill="none"
                  viewBox="0 0 24 24"
                >
                  <circle
                    class="opacity-25"
                    cx="12"
                    cy="12"
                    r="10"
                    stroke="currentColor"
                    stroke-width="4"
                  />
                  <path
                    class="opacity-75"
                    fill="currentColor"
                    d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"
                  />
                </svg>
                {{ submitButtonText }}
              </span>
              <span v-else>{{ submitButtonText }}</span>
            </button>
          </div>
        </form>
      </div>
    </div>
  </div>
</template>