import type {ITheoryLesson} from "@/interfaces/ITheoryLesson";

export interface IInstructorBase {
  firstName: string;
  lastName: string;
  mail: string;
  phone?: string;
}

export type IInstructorUpdate = IInstructorBase & { id: number };

export interface IInstructor extends IInstructorBase {
  id: number;
  isDeleted: boolean;

  // Navigation properties
  theoryLessons?: ITheoryLesson[];
}
