import type {IStudent} from "@/interfaces/IStudent.ts";

export class StudentHelper {
  public static getFullName(student?: IStudent) : string {
    if(student == null) return "Nicht zugewiesen";
    return `${student.firstName} ${student.lastName}`;
  }

  public static getPhone(student?: IStudent) : string {
    if(student == null || student.phone == null) return "Keine Angabe";
    return student.phone;
  }

  public static getMail(student?: IStudent) : string {
    if(student == null || student.mail == null) return "Keine Angabe";
    return student.mail;
  }

  public static getDrivingSchoolName(student?: IStudent) : string {
    if(student == null || student.drivingSchool == null) return "Keine Zuweisung";
    return student.drivingSchool.name;
  }
}
