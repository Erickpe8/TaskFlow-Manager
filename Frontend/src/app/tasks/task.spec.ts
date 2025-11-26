// tasks/task.spec.ts
import {
  TaskDto,
  ColumnDto,
  CreateTaskDto,
  UpdateTaskDto,
  PaginatedResult,
  TaskFilter
} from './task';

describe('Task Interfaces', () => {
  it('should create a valid TaskDto', () => {
    const task: TaskDto = {
      Id: 1,
      Title: 'Test Task',
      Description: 'Test Description',
      ColumnId: 1,
      ColumnName: 'To Do',
      Priority: 2,
      DueDate: '2025-12-31',
      Order: 1
    };

    expect(task.Id).toBe(1);
    expect(task.Title).toBe('Test Task');
    expect(task.Priority).toBe(2);
  });

  it('should create a valid ColumnDto', () => {
    const column: ColumnDto = {
      Id: 1,
      Name: 'To Do',
      Order: 1,
      Tasks: []
    };

    expect(column.Id).toBe(1);
    expect(column.Name).toBe('To Do');
    expect(column.Tasks).toEqual([]);
  });

  it('should create a valid CreateTaskDto', () => {
    const createDto: CreateTaskDto = {
      Title: 'New Task',
      Description: 'New Description',
      ColumnId: 1,
      Priority: 3,
      DueDate: '2025-12-31'
    };

    expect(createDto.Title).toBe('New Task');
    expect(createDto.ColumnId).toBe(1);
  });

  it('should create a valid UpdateTaskDto', () => {
    const updateDto: UpdateTaskDto = {
      Title: 'Updated Task',
      Description: 'Updated Description',
      ColumnId: 2,
      Priority: 1,
      DueDate: '2025-12-31',
      Order: 5
    };

    expect(updateDto.Title).toBe('Updated Task');
    expect(updateDto.Order).toBe(5);
  });

  it('should create a valid PaginatedResult', () => {
    const result: PaginatedResult<TaskDto> = {
      Items: [],
      TotalCount: 0,
      Page: 1,
      PageSize: 10,
      TotalPages: 0
    };

    expect(result.Items).toEqual([]);
    expect(result.Page).toBe(1);
    expect(result.PageSize).toBe(10);
  });

  it('should create a valid TaskFilter', () => {
    const filter: TaskFilter = {
      search: 'test',
      columnId: 1,
      priority: 2,
      sortBy: 'title',
      sortDesc: false,
      page: 1,
      pageSize: 10
    };

    expect(filter.search).toBe('test');
    expect(filter.page).toBe(1);
    expect(filter.sortBy).toBe('title');
  });

  it('should handle optional fields in TaskDto', () => {
    const task: TaskDto = {
      Id: 1,
      Title: 'Minimal Task',
      ColumnId: 1,
      ColumnName: 'To Do',
      Order: 1
    };

    expect(task.Description).toBeUndefined();
    expect(task.Priority).toBeUndefined();
    expect(task.DueDate).toBeUndefined();
  });

  it('should calculate TotalPages correctly in PaginatedResult', () => {
    const result: PaginatedResult<TaskDto> = {
      Items: [],
      TotalCount: 25,
      Page: 1,
      PageSize: 10,
      TotalPages: Math.ceil(25 / 10)
    };

    expect(result.TotalPages).toBe(3);
  });
});
