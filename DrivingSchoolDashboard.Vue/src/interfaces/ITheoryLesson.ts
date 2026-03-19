import type {IInstructor} from "@/interfaces/IInstructor";

export interface ITheoryLessonBase {
  name: string;
  topic: string;
  dayOfWeek: number;
  startTime: Date | string | null;
  endTime: Date | string | null;
  durationMinutes: number;
  maxStudents: number;
  price: number;
  isBasic: boolean;
}

export type ITheoryLessonUpdate = ITheoryLessonBase & { id: number };

export interface ITheoryLesson extends ITheoryLessonBase {
  id: number;
  isDeleted: boolean;

  // Navigation properties
  instructorId?: number;
  instructor?: IInstructor;
}
