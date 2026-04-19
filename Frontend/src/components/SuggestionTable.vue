<script setup lang="ts">
import type { SuggestionViewModel } from '@/types/suggestion'
import SuggestionStatusBadge from './SuggestionStatusBadge.vue'
import SuggestionPriorityIndicator from './SuggestionPriorityIndicator.vue'

defineProps<{ suggestions: SuggestionViewModel[] }>()

function formatDate(date: string): string {
  return new Date(date).toLocaleDateString('en-GB', { day: 'numeric', month: 'short', year: 'numeric' })
}
</script>

<template>
  <div class="table-card">
    <table>
      <thead>
        <tr>
          <th>Employee</th>
          <th>Type</th>
          <th>Description</th>
          <th>Status</th>
          <th>Priority</th>
          <th>Source</th>
          <th>Date</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="suggestion in suggestions" :key="suggestion.id">
          <td class="td--employee">{{ suggestion.employeeName }}</td>
          <td class="td--type">{{ suggestion.type }}</td>
          <td class="td--description">{{ suggestion.description }}</td>
          <td><SuggestionStatusBadge :status="suggestion.status" /></td>
          <td><SuggestionPriorityIndicator :priority="suggestion.priority" /></td>
          <td class="td--muted">{{ suggestion.source }}</td>
          <td class="td--muted">{{ formatDate(suggestion.dateCreated) }}</td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<style scoped>
.table-card {
  background: var(--color-surface);
  border-radius: 12px;
  box-shadow: 0 1px 3px rgba(0, 0, 0, 0.06), 0 4px 16px rgba(0, 0, 0, 0.04);
  overflow: hidden;
}

table {
  width: 100%;
  border-collapse: collapse;
}

thead {
  background: var(--color-surface-subtle);
  border-bottom: 1px solid var(--color-border-table);
}

th {
  text-align: left;
  padding: 0.75rem 1.25rem;
  font-size: 0.7rem;
  font-weight: 600;
  text-transform: uppercase;
  letter-spacing: 0.07em;
  color: var(--color-text-muted);
}

td {
  padding: 1rem 1.25rem;
  font-size: 0.875rem;
  color: var(--color-text-secondary);
  border-bottom: 1px solid var(--color-border-row);
  vertical-align: middle;
}

tbody tr {
  transition: background 0.15s ease;
}

tbody tr:last-child td {
  border-bottom: none;
}

tbody tr:hover {
  background: var(--color-surface-subtle);
}

.td--employee {
  font-weight: 600;
  color: var(--color-text-primary);
}

.td--type {
  text-transform: capitalize;
  color: var(--color-text-tertiary);
}

.td--description {
  max-width: 280px;
  color: var(--color-text-tertiary);
  line-height: 1.5;
}

.td--muted {
  color: var(--color-text-faint);
  text-transform: capitalize;
  font-size: 0.8125rem;
}
</style>
