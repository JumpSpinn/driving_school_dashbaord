import type {IInstructor} from "@/interfaces/IInstructor";

export interface ITheoryLesson {
  Id: number;
  Name: string;
  Topic: string;
  DayOfWeek: number;
  StartTime: Date | string | null;
  EndTime: Date | string | null;
  DurationMinutes: number;
  MaxStudents: number;
  Price: number;
  IsBasic: boolean;
  IsDeleted: boolean;

  // Navigation properties
  InstructorId?: number;
  Instructor?: IInstructor;
}
