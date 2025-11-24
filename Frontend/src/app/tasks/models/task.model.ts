export interface Task {
  id: number;
  title: string;
  description: string;
  columnId: number;
  priority: number | null;
  order: number;
  dueDate: string | null;
  createdAt: string;
  updatedAt: string;
}
