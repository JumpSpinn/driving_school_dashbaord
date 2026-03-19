import type {IStudent} from "@/interfaces/IStudent";
import type {IInstructor} from "@/interfaces/IInstructor";

export interface IDrivingSchoolBase {
  name: string;
  ownerId?: number;
}

export type IDrivingSchoolUpdate = IDrivingSchoolBase & { id: number };

export interface IDrivingSchool extends IDrivingSchoolBase {
  id: number;
  isDeleted: boolean;

  // Navigation properties
  students: IStudent[];
  owner?: IInstructor;
}
