export interface Task {
  id: number;
  title: string;
  description?: string;
  columnId: number;
  priority?: number;
  dueDate?: string;
  order: number;
}
