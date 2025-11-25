// tasks/task.ts
export interface PaginatedResult<T> {
  Items: T[];
  TotalCount: number;
  Page: number;
  PageSize: number;
  TotalPages: number;
}

export interface TaskFilter {
  search?: string;
  columnId?: number;
  priority?: number;
  sortBy?: string;
  sortDesc?: boolean;
  page: number;
  pageSize: number;
}

export interface TaskDto {
  Id: number;
  Title: string;
  Description?: string;
  ColumnId: number;
  ColumnName: string;
  Priority?: number;
  DueDate?: string;
  Order: number;
}

export interface ColumnDto {
  Id: number;
  Name: string;
  Order: number;
  Tasks: TaskDto[];
}

export interface CreateTaskDto {
  Title: string;
  Description?: string;
  ColumnId: number;
  Priority?: number;
  DueDate?: string;
}

export interface UpdateTaskDto {
  Title: string;
  Description?: string;
  ColumnId: number;
  Priority?: number;
  DueDate?: string;
  Order: number;
}
