import type {ITheoryLesson} from "@/interfaces/ITheoryLesson.ts";

export class TheoryLessonHelper {
  public static getName(theoryLesson?: ITheoryLesson) : string {
    if(theoryLesson == null) return "Nicht zugewiesen";
    return `${theoryLesson.name} (${theoryLesson.topic})`;
  }
}
