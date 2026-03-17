import {LicenseClass} from "@/enums/LicenseClass";
import type {IDrivingSchool} from "@/interfaces/IDrivingSchool";
import type {ICourseBooking} from "@/interfaces/ICourseBooking";

export interface IStudent {
  id: number;
  firstName: string;
  lastName: string;
  mail: string;
  phone?: string;
  birthday?: Date | string | null;
  license?: LicenseClass;
  enrollmentDate?: Date | string | null;
  examDate?: Date | string | null;
  hasPassed: boolean;
  isDeleted: boolean;

  // Navigation properties
  drivingSchoolId?: number;
  drivingSchool?: IDrivingSchool;
  courseBookings?: ICourseBooking[];
}
