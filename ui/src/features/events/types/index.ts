export interface Event {
  id: string
  title: string
  description: string
  startDate: string
  endDate: string
  venueName: string
  totalSeats: number
  availableSeats: number
  price: number
  createdAt: string
}

export type EventFormData = Omit<Event, 'id' | 'createdAt'>

export interface PaginatedResponse<T> {
  items: T[]
  pageNumber: number
  pageSize: number
  totalPages: number
  totalCount: number
  hasPreviousPage: boolean
  hasNextPage: boolean
}