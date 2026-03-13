import type {IStudent} from "@/interfaces/IStudent";
import type {IInstructor} from "@/interfaces/IInstructor";

export interface IDrivingSchool {
  Id: number;
  Name: string;
  IsDeleted: boolean;

  // Navigation properties
  Students: IStudent[];
  OwnerId?: number;
  Owner?: IInstructor;
}
