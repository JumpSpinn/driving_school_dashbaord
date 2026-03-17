import type {IInstructor} from "@/interfaces/IInstructor";

export interface ITheoryLesson {
  id: number;
  name: string;
  topic: string;
  dayOfWeek: number;
  startTime: Date | string | null;
  endTime: Date | string | null;
  durationMinutes: number;
  maxStudents: number;
  price: number;
  isBasic: boolean;
  isDeleted: boolean;

  // Navigation properties
  instructorId?: number;
  instructor?: IInstructor;
}
