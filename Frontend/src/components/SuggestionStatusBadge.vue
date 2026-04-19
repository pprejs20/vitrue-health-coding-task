<script setup lang="ts">
import { computed } from 'vue'
import { SuggestionStatus } from '@/types/suggestion'

const props = defineProps<{ status: SuggestionStatus }>()

const label: Record<SuggestionStatus, string> = {
  [SuggestionStatus.Pending]: 'Pending',
  [SuggestionStatus.InProgress]: 'In Progress',
  [SuggestionStatus.Completed]: 'Completed',
  [SuggestionStatus.Overdue]: 'Overdue',
}

const badgeClass = computed(() => `badge badge--${props.status.replace('_', '-')}`)
</script>

<template>
  <span :class="badgeClass">{{ label[status] }}</span>
</template>

<style scoped>
.badge {
  display: inline-block;
  padding: 0.25rem 0.65rem;
  border-radius: 99px;
  font-size: 0.75rem;
  font-weight: 600;
  letter-spacing: 0.01em;
  white-space: nowrap;
}

.badge--pending {
  background: var(--color-status-pending-bg);
  color: var(--color-status-pending-text);
}

.badge--in-progress {
  background: var(--color-status-in-progress-bg);
  color: var(--color-status-in-progress-text);
}

.badge--completed {
  background: var(--color-status-completed-bg);
  color: var(--color-status-completed-text);
}

.badge--overdue {
  background: var(--color-status-overdue-bg);
  color: var(--color-status-overdue-text);
}
</style>
