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
  enrollmentDate?: Date | string | null;
  examDate?: Date | string | null;
  hasPassed: boolean;
}

export type IStudentUpdate = IStudentBase & { id: number };

export interface IStudent extends IStudentBase {
  id: number;
  isDeleted: boolean;

  // Navigation properties
  drivingSchoolId?: number;
  drivingSchool?: IDrivingSchool;
  courseBookings?: ICourseBooking[];
}
