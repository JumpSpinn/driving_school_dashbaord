namespace DrivingSchoolDashboard.Web.Helpers;

public static class LicenseHelper
{
    public static string GetName(LicenseClass? license) => license switch
    {
        LicenseClass.AM => "AM",
        LicenseClass.A1 => "A1",
        LicenseClass.A2 => "A2",
        LicenseClass.A => "A",
        LicenseClass.B => "B",
        LicenseClass.BE => "BE",
        LicenseClass.C => "C",
        LicenseClass.T => "T",
        _ => "-"
    };
    
    public static string GetDescription(LicenseClass? license) => license switch
    {
        LicenseClass.AM => "Moped & Roller (bis 45 km/h)",
        LicenseClass.A1 => "Leichtkraftrad (bis 125 ccm)",
        LicenseClass.A2 => "Kraftrad (bis 35 kW)",
        LicenseClass.A => "Schweres Kraftrad (unbegrenzt)",
        LicenseClass.B => "Pkw (bis 3,5t)",
        LicenseClass.BE => "Pkw mit schwerem Anhänger",
        LicenseClass.C => "Lkw (über 7,5t)",
        LicenseClass.T => "Traktor & Zugmaschinen",
        _ => "Bisher keine Angabe / Unbekannt"
    };
}