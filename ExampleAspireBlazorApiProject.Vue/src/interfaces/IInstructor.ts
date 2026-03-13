import type {ITheoryLesson} from "@/interfaces/ITheoryLesson";

export interface IInstructor {
  Id: number;
  FirstName: string;
  LastName: string;
  Mail: string;
  Phone?: string;
  IsDeleted: boolean;

  // Navigation properties
  TheoryLessons?: ITheoryLesson[];
}
