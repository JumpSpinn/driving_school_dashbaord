import {DayOfWeek} from "@/enums/DayOfWeek.ts";

export class TimeHelper {
  static convert(timestamp: string | Date | null | undefined, format: 'full' | 'date' | 'time' | 'datetime' = 'full'): string {
    if(timestamp == null) return "Keine Angabe";

    const date = typeof timestamp === 'string' ? new Date(timestamp) : timestamp;

    const options: Intl.DateTimeFormatOptions = {
      timeZone: 'Europe/Berlin'
    };

    switch (format) {
      case 'full':
        options.dateStyle = 'full';
        options.timeStyle = 'short';
        break;
      case 'date':
        options.year = 'numeric';
        options.month = '2-digit';
        options.day = '2-digit';
        break;
      case 'time':
        options.hour = '2-digit';
        options.minute = '2-digit';
        break;
      case 'datetime':
        options.year = 'numeric';
        options.month = '2-digit';
        options.day = '2-digit';
        options.hour = '2-digit';
        options.minute = '2-digit';
        break;
    }

    return date.toLocaleString('de-DE', options);
  }

  public static async wait(ms: number) {
    return new Promise(resolve => setTimeout(resolve, ms));
  }

  public static getDayOfWeekName = (dayOfWeek: DayOfWeek) => {
    switch(dayOfWeek){
      case DayOfWeek.Monday: return "Montag";
      case DayOfWeek.Tuesday: return "Dienstag";
      case DayOfWeek.Wednesday: return "Mittwoch";
      case DayOfWeek.Thursday: return "Donnerstag";
      case DayOfWeek.Friday: return "Freitag";
      case DayOfWeek.Saturday: return "Samstag";
      case DayOfWeek.Sunday: return "Sonntag";
    }
  }
}
