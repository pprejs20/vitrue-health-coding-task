import type { Suggestion, SuggestionViewModel } from '@/types/suggestion'

const BASE_URL = 'http://localhost:5156/suggestion'

export async function getSuggestions(): Promise<Suggestion[]> {
  const response = await fetch(BASE_URL)
  if (!response.ok) throw new Error('Failed to fetch suggestions')
  return response.json()
}

export async function getSuggestionViewModels(): Promise<SuggestionViewModel[]> {
  const response = await fetch(`${BASE_URL}/view`)
  if (!response.ok) throw new Error('Failed to fetch suggestions')
  return response.json()
}

export async function updateSuggestion(suggestion: Suggestion): Promise<void> {
  const response = await fetch(`${BASE_URL}/${suggestion.id}`, {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(suggestion),
  })
  if (!response.ok) throw new Error('Failed to update suggestion')
}
