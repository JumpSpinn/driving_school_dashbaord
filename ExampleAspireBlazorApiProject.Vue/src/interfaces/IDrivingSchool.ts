import type {IStudent} from "@/interfaces/IStudent";
import type {IInstructor} from "@/interfaces/IInstructor";

export interface IDrivingSchool {
  id: number;
  name: string;
  isDeleted: boolean;

  // Navigation properties
  students: IStudent[];
  ownerId?: number;
  owner?: IInstructor;
}
