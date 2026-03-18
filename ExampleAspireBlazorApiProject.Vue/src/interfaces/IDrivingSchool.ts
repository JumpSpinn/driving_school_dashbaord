import type {IStudent} from "@/interfaces/IStudent";
import type {IInstructor} from "@/interfaces/IInstructor";

export interface IDrivingSchoolBase {
  name: string;
}

export type IDrivingSchoolUpdate = IDrivingSchoolBase & { id: number };

export interface IDrivingSchool extends IDrivingSchoolBase {
  id: number;
  isDeleted: boolean;

  // Navigation properties
  students: IStudent[];
  ownerId?: number;
  owner?: IInstructor;
}
