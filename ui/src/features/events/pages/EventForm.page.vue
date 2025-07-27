<script setup lang="ts">
import EventCreateUpdate from '../components/EventCreateUpdate.vue'
import { useRoute } from 'vue-router'

const route = useRoute()
const id = route.params.id as string
</script>

<template>
  <EventCreateUpdate :id="id" />
</template> 