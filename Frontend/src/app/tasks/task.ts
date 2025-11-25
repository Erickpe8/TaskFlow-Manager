export interface TaskDto {
  Id: number;
  Title: string;
  Description: string;
  ColumnId: number;
  Priority: number;
  DueDate: string | null;
  Order: number;
}

export interface ColumnDto {
  Id: number;
  Name: string;
  Order: number;
  Tasks: TaskDto[];
}
