import { ref } from 'vue'
import type { Event, PaginatedResponse, EventFormData } from '../features/events/types/index'
import apiClient from '../api/client'

export function useEvents() {
  const events = ref<Event[]>([])
  const loading = ref(false)
  const error = ref<string | null>(null)
  const currentPage = ref(1)
  const pageSize = ref(10)
  const totalPages = ref(0)
  const totalCount = ref(0)
  const hasPreviousPage = ref(false)
  const hasNextPage = ref(false)
  const searchTerm = ref('')
  const currentEvent = ref<Event | null>(null)

  const getAvailabilityColor = (total: number, available: number) => {
    const percentage = (available / total) * 100
    if (percentage > 50) return 'bg-green-100 text-green-800'
    if (percentage > 20) return 'bg-yellow-100 text-yellow-800'
    return 'bg-red-100 text-red-800'
  }

  const fetchEvents = async () => {
    loading.value = true
    error.value = null
    try {
      const { data } = await apiClient.get<PaginatedResponse<Event>>('/events', {
        params: {
          searchTerm: searchTerm.value,
          pageNumber: currentPage.value,
          pageSize: pageSize.value
        }
      })
      events.value = data.items
      totalPages.value = data.totalPages
      totalCount.value = data.totalCount
      hasPreviousPage.value = data.hasPreviousPage
      hasNextPage.value = data.hasNextPage
    } catch (err) {
      error.value = err instanceof Error ? err.message : 'An error occurred while fetching events'
    } finally {
      loading.value = false
    }
  }

  const getEventById = async (id: string) => {
    loading.value = true
    error.value = null
    try {
      const { data } = await apiClient.get<Event>(`/events/${id}`)
      currentEvent.value = data
      return data
    } catch (err) {
      error.value = err instanceof Error ? err.message : 'An error occurred while fetching the event'
      return null
    } finally {
      loading.value = false
    }
  }

  const createEvent = async (event: EventFormData) => {
    loading.value = true
    error.value = null
    try {
      const { data } = await apiClient.post<Event>('/events', event)
      return data
    } catch (err) {
      error.value = err instanceof Error ? err.message : 'An error occurred while creating the event'
      throw error.value
    } finally {
      loading.value = false
    }
  }

  const updateEvent = async (id: string, event: EventFormData) => {
    loading.value = true
    error.value = null
    try {
      const eventWithId = { ...event, id }
      const { data } = await apiClient.put<Event>(`/events/${id}`, eventWithId)
      return data
    } catch (err) {
      error.value = err instanceof Error ? err.message : 'An error occurred while updating the event'
      throw error.value
    } finally {
      loading.value = false
    }
  }

  const deleteEvent = async (id: string) => {
    loading.value = true
    error.value = null
    try {
      await apiClient.delete(`/events/${id}`)
      await fetchEvents()
      return true
    } catch (err) {
      error.value = err instanceof Error ? err.message : 'An error occurred while deleting the event'
      return false
    } finally {
      loading.value = false
    }
  }

  const handlePageChange = async (page: number) => {
    currentPage.value = page
    await fetchEvents()
  }

  const handleSearch = async (term: string) => {
    searchTerm.value = term
    currentPage.value = 1
    await fetchEvents()
  }

  return {
    events,
    loading,
    error,
    currentPage,
    pageSize,
    totalPages,
    totalCount,
    hasPreviousPage,
    hasNextPage,
    searchTerm,
    currentEvent,
    getAvailabilityColor,
    handlePageChange,
    handleSearch,
    fetchEvents,
    getEventById,
    createEvent,
    updateEvent,
    deleteEvent
  }
}