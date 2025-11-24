import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { Task } from '../models/task.model';

@Injectable({ providedIn: 'root' })
export class TaskStateService {

  private _tasks = new BehaviorSubject<Task[]>([]);
  public tasks$ = this._tasks.asObservable();

  /** Sobrescribir todas las tareas */
  setTasks(tasks: Task[]) {
    this._tasks.next(tasks);
  }

  /** Actualizar una sola tarea */
  updateTask(updated: Task) {
    const copy = this._tasks.value.map(t =>
      t.id === updated.id ? updated : t
    );
    this._tasks.next(copy);
  }

  /** Agregar nueva tarea */
  addTask(newTask: Task) {
    this._tasks.next([...this._tasks.value, newTask]);
  }

  /** Eliminar tarea */
  removeTask(id: number) {
    this._tasks.next(this._tasks.value.filter(t => t.id !== id));
  }
}
