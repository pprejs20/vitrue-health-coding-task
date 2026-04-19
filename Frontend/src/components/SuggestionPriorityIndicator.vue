<script setup lang="ts">
import { computed } from 'vue'
import { SuggestionPriority } from '@/types/suggestion'

const props = defineProps<{ priority: SuggestionPriority }>()

const label: Record<SuggestionPriority, string> = {
  [SuggestionPriority.Low]: 'Low',
  [SuggestionPriority.Medium]: 'Medium',
  [SuggestionPriority.High]: 'High',
}

const priorityClass = computed(() => `priority priority--${props.priority}`)
</script>

<template>
  <span :class="priorityClass">
    <span class="priority__dot"></span>
    {{ label[priority] }}
  </span>
</template>

<style scoped>
.priority {
  display: inline-flex;
  align-items: center;
  gap: 0.4rem;
  font-size: 0.8125rem;
  font-weight: 500;
}

.priority__dot {
  width: 7px;
  height: 7px;
  border-radius: 50%;
  flex-shrink: 0;
}

.priority--low .priority__dot    { background: var(--color-priority-low-dot); }
.priority--medium .priority__dot { background: var(--color-priority-medium-dot); }
.priority--high .priority__dot   { background: var(--color-priority-high-dot); }

.priority--low    { color: var(--color-priority-low-text); }
.priority--medium { color: var(--color-priority-medium-text); }
.priority--high   { color: var(--color-priority-high-text); }
</style>
