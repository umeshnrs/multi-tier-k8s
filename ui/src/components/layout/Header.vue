<script setup lang="ts">
import { RouterLink } from 'vue-router'
import Nav from './Nav.vue'
</script>

<template>
  <header class="bg-white shadow">
    <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
      <div class="flex items-center justify-between h-16">
        <!-- Logo -->
        <div class="flex-shrink-0 flex items-center">
          <RouterLink
            to="/"
            class="flex items-center space-x-2"
          >
            <img
              class="h-8 w-auto"
              src="../../assets/vue.svg"
              alt="Logo"
            >
            <span class="text-xl font-bold text-gray-900">Events App</span>
          </RouterLink>
        </div>

        <!-- Navigation -->
        <div class="flex flex-1 justify-start pl-8">
          <Nav />
        </div>
      </div>
    </div>
  </header>
</template>