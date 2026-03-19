import type {IStudent} from "@/interfaces/IStudent";
import type {ITheoryLesson} from "@/interfaces/ITheoryLesson";

export interface ICourseBookingBase {
  studentId?: number;
  theoryLessonId?: number;
  bookingDate?: Date | string | null;
}

export type ICourseBookingUpdate = ICourseBookingBase & { id: number }

export interface ICourseBooking extends ICourseBookingBase {
  id: number;

  // Navigation properties
  student?: IStudent;
  theoryLesson?: ITheoryLesson;
}
