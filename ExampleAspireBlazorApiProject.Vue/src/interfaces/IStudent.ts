import {LicenseClass} from "@/enums/LicenseClass";
import type {IDrivingSchool} from "@/interfaces/IDrivingSchool";
import type {ICourseBooking} from "@/interfaces/ICourseBooking";

export interface IStudent {
  Id: number;
  FirstName: string;
  LastName: string;
  Mail: string;
  Phone?: string;
  Birthday?: Date | string | null;
  License?: LicenseClass;
  EnrollmentDate?: Date | string | null;
  ExamDate?: Date | string | null;
  HasPassed: boolean;
  IsDeleted: boolean;

  // Navigation properties
  DrivingSchoolId?: number;
  DrivingSchool?: IDrivingSchool;
  CourseBookings?: ICourseBooking[];
}
