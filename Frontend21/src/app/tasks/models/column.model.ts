import { Task } from './task.model';

export interface Column {
  id: number;
  name: string;
  order: number;
  tasks: Task[];
}
