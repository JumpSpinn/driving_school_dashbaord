import type {IStudent} from "@/interfaces/IStudent";
import type {ITheoryLesson} from "@/interfaces/ITheoryLesson";

export interface ICourseBooking {
  id: number;
  studentId?: number;
  theoryLessonId?: number;
  bookingDate?: Date | string | null;

  // Navigation properties
  student?: IStudent;
  theoryLesson?: ITheoryLesson;
}
