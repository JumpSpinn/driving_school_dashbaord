import {LicenseClass} from "@/enums/LicenseClass";
import type {IDrivingSchool} from "@/interfaces/IDrivingSchool";
import type {ICourseBooking} from "@/interfaces/ICourseBooking";

export interface IStudentBase {
  firstName: string;
  lastName: string;
  mail: string;
  phone?: string;
  birthday?: Date | string | null;
  license?: LicenseClass;
  examDate?: Date | string | null;
  hasPassed: boolean;
  drivingSchoolId?: number;
}

export type IStudentUpdate = IStudentBase & { id: number };

export interface IStudent extends IStudentBase {
  id: number;
  enrollmentDate?: Date | string | null;
  isDeleted: boolean;

  // Navigation properties
  drivingSchool?: IDrivingSchool;
  courseBookings?: ICourseBooking[];
}
