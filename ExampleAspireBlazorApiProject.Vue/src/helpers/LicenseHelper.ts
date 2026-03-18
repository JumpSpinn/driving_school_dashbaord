import {LicenseClass} from "@/enums/LicenseClass.ts";

export class LicenseHelper {
  public static getName = (license?: LicenseClass) => {
    switch(license){
      case LicenseClass.AM: return "AM";
      case LicenseClass.A1: return "A1";
      case LicenseClass.A2: return "A2";
      case LicenseClass.A: return "A";
      case LicenseClass.B: return "B";
      case LicenseClass.BE: return "BE";
      case LicenseClass.C: return "C";
      case LicenseClass.T: return "T";
      default: return "-";
    }
  }

  public static getDescription = (license?: LicenseClass) => {
    switch(license){
      case LicenseClass.AM: return "Moped & Roller (bis 45 km/h)";
      case LicenseClass.A1: return "Leichtkraftrad (bis 125 ccm)";
      case LicenseClass.A2: return "Kraftrad (bis 35 kW)";
      case LicenseClass.A: return "Schweres Kraftrad (unbegrenzt)";
      case LicenseClass.B: return "Pkw (bis 3,5t)";
      case LicenseClass.BE: return "Pkw mit schwerem Anhänger";
      case LicenseClass.C: return "Lkw (über 7,5t)";
      case LicenseClass.T: return "Traktor & Zugmaschinen";
      default: return "-";
    }
  }
}
