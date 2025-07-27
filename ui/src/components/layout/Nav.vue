<script setup lang="ts">
import { ref } from 'vue'
import { useRoute } from 'vue-router'
import { NAVIGATION_ITEMS } from '../../constants/navigation'

const isOpen = ref(false)
const route = useRoute()

const isActive = (path: string) => {
  return route.path === path
}

const toggleMenu = () => {
  isOpen.value = !isOpen.value
}
</script>

<template>
  <div class="w-full flex items-center">
    <!-- Desktop Navigation -->
    <div class="hidden md:flex space-x-4">
      <router-link
        v-for="item in NAVIGATION_ITEMS"
        :key="item.path"
        :to="item.path"
        :class="[
          isActive(item.path)
            ? 'bg-gray-100 text-gray-900'
            : 'text-gray-600 hover:bg-gray-50 hover:text-gray-900',
          'px-3 py-2 rounded-md text-sm font-medium'
        ]"
      >
        <div class="flex items-center space-x-2">
          <svg
            class="h-5 w-5"
            xmlns="http://www.w3.org/2000/svg"
            fill="none"
            viewBox="0 0 24 24"
            stroke="currentColor"
          >
            <path
              stroke-linecap="round"
              stroke-linejoin="round"
              stroke-width="2"
              :d="item.icon"
            />
          </svg>
          <span>{{ item.name }}</span>
        </div>
      </router-link>
    </div>

    <!-- Mobile menu button -->
    <div class="flex md:hidden ml-auto">
      <button
        class="inline-flex items-center justify-center p-2 rounded-md text-gray-500 hover:text-gray-900 hover:bg-gray-100 focus:outline-none focus:ring-2 focus:ring-inset focus:ring-gray-500"
        @click="toggleMenu"
      >
        <span class="sr-only">Open main menu</span>
        <svg
          class="h-6 w-6"
          xmlns="http://www.w3.org/2000/svg"
          fill="none"
          viewBox="0 0 24 24"
          stroke="currentColor"
        >
          <path
            stroke-linecap="round"
            stroke-linejoin="round"
            stroke-width="2"
            d="M4 6h16M4 12h16M4 18h16"
          />
        </svg>
      </button>
    </div>

    <!-- Mobile menu -->
    <div
      v-if="isOpen"
      class="md:hidden fixed inset-0 z-50"
    >
      <div class="fixed inset-0 bg-black bg-opacity-25" @click="isOpen = false"></div>
      <nav class="fixed top-0 right-0 bottom-0 w-64 bg-white shadow-lg">
        <div class="px-4 pt-4 pb-3 border-b border-gray-200">
          <div class="flex items-center justify-between">
            <span class="text-lg font-medium text-gray-900">Menu</span>
            <button
              class="rounded-md p-2 text-gray-500 hover:text-gray-900 hover:bg-gray-100 focus:outline-none focus:ring-2 focus:ring-inset focus:ring-gray-500"
              @click="isOpen = false"
            >
              <span class="sr-only">Close menu</span>
              <svg
                class="h-6 w-6"
                xmlns="http://www.w3.org/2000/svg"
                fill="none"
                viewBox="0 0 24 24"
                stroke="currentColor"
              >
                <path
                  stroke-linecap="round"
                  stroke-linejoin="round"
                  stroke-width="2"
                  d="M6 18L18 6M6 6l12 12"
                />
              </svg>
            </button>
          </div>
        </div>
        <div class="px-2 pt-2 pb-3 space-y-1">
          <router-link
            v-for="item in NAVIGATION_ITEMS"
            :key="item.path"
            :to="item.path"
            :class="[
              isActive(item.path)
                ? 'bg-gray-100 text-gray-900'
                : 'text-gray-600 hover:bg-gray-50 hover:text-gray-900',
              'block px-3 py-2 rounded-md text-base font-medium'
            ]"
            @click="isOpen = false"
          >
            <div class="flex items-center space-x-2">
              <svg
                class="h-5 w-5"
                xmlns="http://www.w3.org/2000/svg"
                fill="none"
                viewBox="0 0 24 24"
                stroke="currentColor"
              >
                <path
                  stroke-linecap="round"
                  stroke-linejoin="round"
                  stroke-width="2"
                  :d="item.icon"
                />
              </svg>
              <span>{{ item.name }}</span>
            </div>
          </router-link>
        </div>
      </nav>
    </div>
  </div>
</template>
