<script setup lang="ts">
import { ref, onMounted, computed } from 'vue'
import { getSuggestionViewModels } from '@/services/suggestionService'
import type { SuggestionViewModel } from '@/types/suggestion'
import SuggestionTable from '@/components/SuggestionTable.vue'

const suggestions = ref<SuggestionViewModel[]>([])
const error = ref<string | null>(null)
const loading = ref(true)

onMounted(async () => {
  try {
    suggestions.value = await getSuggestionViewModels()
  } catch {
    error.value = 'Failed to load suggestions.'
  } finally {
    loading.value = false
  }
})

const total = computed(() => suggestions.value.length)
</script>

<template>
  <main>
    <div class="page-header">
      <div class="page-header__text">
        <h1>Wellness Suggestions</h1>
        <p class="subtitle">Employee health & exercise recommendations</p>
      </div>
      <div v-if="suggestions.length" class="page-header__count">
        <span class="count">{{ total }}</span>
        <span class="count-label">total</span>
      </div>
    </div>

    <div v-if="loading" class="loading-state">Loading suggestions...</div>

    <div v-else-if="error" class="error-state">
      <span class="error-state__icon">!</span>
      <p>{{ error }}</p>
    </div>

    <SuggestionTable v-else-if="suggestions.length" :suggestions="suggestions" />


    <div v-else class="empty-state">
      <p>No suggestions found.</p>
    </div>
  </main>
</template>

<style scoped>
main {
  font-family: 'Plus Jakarta Sans', sans-serif;
  padding: 2.5rem 3rem;
  min-height: 100vh;
  background: var(--color-page-bg);
}

.page-header {
  display: flex;
  align-items: flex-end;
  justify-content: space-between;
  margin-bottom: 2rem;
}

h1 {
  font-size: 1.75rem;
  font-weight: 700;
  color: var(--color-text-primary);
  letter-spacing: -0.02em;
  line-height: 1.2;
}

.subtitle {
  margin-top: 0.25rem;
  font-size: 0.875rem;
  color: var(--color-text-muted);
  font-weight: 400;
}

.page-header__count {
  display: flex;
  flex-direction: column;
  align-items: flex-end;
}

.count {
  font-size: 2rem;
  font-weight: 700;
  color: var(--color-text-primary);
  line-height: 1;
  letter-spacing: -0.03em;
}

.count-label {
  font-size: 0.75rem;
  color: var(--color-text-muted);
  text-transform: uppercase;
  letter-spacing: 0.06em;
}

.error-state {
  display: flex;
  align-items: center;
  gap: 0.75rem;
  padding: 1rem 1.25rem;
  background: var(--color-status-overdue-bg);
  border-radius: 8px;
  color: var(--color-status-overdue-text);
  font-size: 0.875rem;
  font-weight: 500;
}

.error-state__icon {
  display: flex;
  align-items: center;
  justify-content: center;
  width: 20px;
  height: 20px;
  border-radius: 50%;
  background: var(--color-status-overdue-text);
  color: white;
  font-size: 0.75rem;
  font-weight: 700;
  flex-shrink: 0;
}

.loading-state {
  text-align: center;
  padding: 4rem;
  color: var(--color-text-muted);
  font-size: 0.9rem;
}

.empty-state {
  text-align: center;
  padding: 4rem;
  color: var(--color-text-muted);
  font-size: 0.9rem;
}
</style>
