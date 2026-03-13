import type {IStudent} from "@/interfaces/IStudent";
import type {ITheoryLesson} from "@/interfaces/ITheoryLesson";

export interface ICourseBooking {
  Id: number;
  StudentId?: number;
  TheoryLessonId?: number;
  BookingDate?: Date | string | null;

  // Navigation properties
  Student?: IStudent;
  TheoryLesson?: ITheoryLesson;
}
