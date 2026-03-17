import type {IInstructor} from "@/interfaces/IInstructor.ts";

export class InstructorHelper {
  public static getOwnerName(instructor?: IInstructor) : string {
    if(instructor == null) return "Kein Besitzer";
    return `${instructor.firstName} ${instructor.lastName}`;
  }
}
