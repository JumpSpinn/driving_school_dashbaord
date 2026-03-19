import type {IInstructor} from "@/interfaces/IInstructor.ts";

export class InstructorHelper {
  public static getFullName(instructor?: IInstructor) : string {
    if(instructor == null) return "Kein Besitzer";
    return `${instructor.firstName} ${instructor.lastName}`;
  }

  public static getPhone(instructor?: IInstructor) : string {
    if(instructor == null || instructor.phone == null || instructor.phone.trim().length == 0) return "Keine Angabe";
    return instructor.phone;
  }

  public static getMail(instructor?: IInstructor) : string {
    if(instructor == null || instructor.mail == null) return "Keine Angabe";
    return instructor.mail;
  }
}
