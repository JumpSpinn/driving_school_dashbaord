import type {ITheoryLesson} from "@/interfaces/ITheoryLesson";

export interface IInstructor {
  id: number;
  firstName: string;
  lastName: string;
  mail: string;
  phone?: string;
  isDeleted: boolean;

  // Navigation properties
  theoryLessons?: ITheoryLesson[];
}
