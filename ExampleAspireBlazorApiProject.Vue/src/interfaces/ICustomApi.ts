import type {ICourseBookingUpdate, ICourseBookingBase} from "@/interfaces/ICourseBooking.ts";
import type {IDrivingSchoolBase, IDrivingSchoolUpdate} from "@/interfaces/IDrivingSchool.ts";
import type {IInstructorBase, IInstructorUpdate} from "@/interfaces/IInstructor.ts";
import type {IStudentBase, IStudentUpdate} from "@/interfaces/IStudent.ts";
import type {ITheoryLessonBase, ITheoryLessonUpdate} from "@/interfaces/ITheoryLesson.ts";

export interface ICustomApi {
  getAll() : Promise<any>;
  create(data: ICourseBookingBase | IDrivingSchoolBase | IInstructorBase | IStudentBase | ITheoryLessonBase) : Promise<any>;
  update(data: ICourseBookingUpdate | IDrivingSchoolUpdate | IInstructorUpdate | IStudentUpdate | ITheoryLessonUpdate) : Promise<any>;
  delete(id: number) : Promise<boolean>;
}
