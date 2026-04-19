export enum SuggestionType {
  Equipment = 'equipment',
  Exercise = 'exercise',
  Behavioural = 'behavioural',
  Lifestyle = 'lifestyle',
}

export enum SuggestionStatus {
  Pending = 'pending',
  InProgress = 'in_progress',
  Completed = 'completed',
  Overdue = 'overdue',
}

export enum SuggestionPriority {
  Low = 'low',
  Medium = 'medium',
  High = 'high',
}

export enum SuggestionSource {
  Vida = 'vida',
  Admin = 'admin',
}

export interface Suggestion {
  id: string
  employeeId: string
  type: SuggestionType
  description: string
  status: SuggestionStatus
  priority: SuggestionPriority
  source: SuggestionSource
  dateCreated: string
  dateUpdated: string
  notes: string
}

export interface SuggestionViewModel {
  id: string
  employeeName: string
  type: SuggestionType
  description: string
  status: SuggestionStatus
  priority: SuggestionPriority
  source: SuggestionSource
  dateCreated: string
  notes: string
}
