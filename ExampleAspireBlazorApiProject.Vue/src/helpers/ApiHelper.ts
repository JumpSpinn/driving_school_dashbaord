import type {ICustomApi} from "@/interfaces/ICustomApi.ts";
import type {ICourseBookingBase, ICourseBookingUpdate} from "@/interfaces/ICourseBooking.ts";
import type {IDrivingSchoolBase, IDrivingSchoolUpdate} from "@/interfaces/IDrivingSchool.ts";
import type {IInstructorBase, IInstructorUpdate} from "@/interfaces/IInstructor.ts";
import type {IStudentBase, IStudentUpdate} from "@/interfaces/IStudent.ts";
import type {ITheoryLessonBase, ITheoryLessonUpdate} from "@/interfaces/ITheoryLesson.ts";

export class ApiHelper {
  public static async getAll(api: ICustomApi) {
    const resp = await api.getAll();
    return resp ?? [];
  }

  public static async create(data: ICourseBookingBase | IDrivingSchoolBase | IInstructorBase | IStudentBase | ITheoryLessonBase, api: ICustomApi) {
    return await api.create(data);
  }

  public static async update(data: ICourseBookingUpdate | IDrivingSchoolUpdate | IInstructorUpdate | IStudentUpdate | ITheoryLessonUpdate, api: ICustomApi) {
    return await api.update(data);
  }

  public static async delete(id: number, api: ICustomApi) {
    return await api.delete(id);
  }
}
