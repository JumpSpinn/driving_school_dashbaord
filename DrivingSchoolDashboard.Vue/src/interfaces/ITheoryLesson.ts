import type {IInstructor} from "@/interfaces/IInstructor";
import type {DayOfWeek} from "@/enums/DayOfWeek.ts";

export interface ITheoryLessonBase {
  name: string;
  topic: string;
  dayOfWeek: number | DayOfWeek;
  startTime: Date | string | null;
  durationMinutes: number;
  maxStudents: number;
  price: number;
  isBasic: boolean;
  instructorId?: number;
}

export type ITheoryLessonUpdate = ITheoryLessonBase & { id: number };

export interface ITheoryLesson extends ITheoryLessonBase {
  id: number;
  isDeleted: boolean;

  // Navigation properties
  instructor?: IInstructor;
}
