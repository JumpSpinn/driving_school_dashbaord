namespace DrivingSchoolDashboard.Tests.Helpers;

[TestFixture]
public sealed class LicenseHelperTests
{
    // ---------------------------------------------------------------------------
    // GetName
    // ---------------------------------------------------------------------------

    [Test]
    public void GetName_ReturnsCorrectNameForLicenseAM()
    {
        var result = LicenseHelper.GetName(LicenseClass.AM);
        Assert.That(result, Is.EqualTo("AM"));
    }

    [Test]
    public void GetName_ReturnsCorrectNameForLicenseA1()
    {
        var result = LicenseHelper.GetName(LicenseClass.A1);
        Assert.That(result, Is.EqualTo("A1"));
    }

    [Test]
    public void GetName_ReturnsCorrectNameForLicenseA2()
    {
        var result = LicenseHelper.GetName(LicenseClass.A2);
        Assert.That(result, Is.EqualTo("A2"));
    }

    [Test]
    public void GetName_ReturnsCorrectNameForLicenseA()
    {
        var result = LicenseHelper.GetName(LicenseClass.A);
        Assert.That(result, Is.EqualTo("A"));
    }

    [Test]
    public void GetName_ReturnsCorrectNameForLicenseB()
    {
        var result = LicenseHelper.GetName(LicenseClass.B);
        Assert.That(result, Is.EqualTo("B"));
    }

    [Test]
    public void GetName_ReturnsCorrectNameForLicenseBE()
    {
        var result = LicenseHelper.GetName(LicenseClass.BE);
        Assert.That(result, Is.EqualTo("BE"));
    }

    [Test]
    public void GetName_ReturnsCorrectNameForLicenseC()
    {
        var result = LicenseHelper.GetName(LicenseClass.C);
        Assert.That(result, Is.EqualTo("C"));
    }

    [Test]
    public void GetName_ReturnsCorrectNameForLicenseT()
    {
        var result = LicenseHelper.GetName(LicenseClass.T);
        Assert.That(result, Is.EqualTo("T"));
    }

    [Test]
    public void GetName_ReturnsDashForNull()
    {
        var result = LicenseHelper.GetName(null);
        Assert.That(result, Is.EqualTo("-"));
    }

    // ---------------------------------------------------------------------------
    // GetDescription
    // ---------------------------------------------------------------------------

    [Test]
    public void GetDescription_ReturnsCorrectDescriptionForAM()
    {
        var result = LicenseHelper.GetDescription(LicenseClass.AM);
        Assert.That(result, Is.EqualTo("Moped & Roller (bis 45 km/h)"));
    }

    [Test]
    public void GetDescription_ReturnsCorrectDescriptionForA1()
    {
        var result = LicenseHelper.GetDescription(LicenseClass.A1);
        Assert.That(result, Is.EqualTo("Leichtkraftrad (bis 125 ccm)"));
    }

    [Test]
    public void GetDescription_ReturnsCorrectDescriptionForA2()
    {
        var result = LicenseHelper.GetDescription(LicenseClass.A2);
        Assert.That(result, Is.EqualTo("Kraftrad (bis 35 kW)"));
    }

    [Test]
    public void GetDescription_ReturnsCorrectDescriptionForA()
    {
        var result = LicenseHelper.GetDescription(LicenseClass.A);
        Assert.That(result, Is.EqualTo("Schweres Kraftrad (unbegrenzt)"));
    }

    [Test]
    public void GetDescription_ReturnsCorrectDescriptionForB()
    {
        var result = LicenseHelper.GetDescription(LicenseClass.B);
        Assert.That(result, Is.EqualTo("Pkw (bis 3,5t)"));
    }

    [Test]
    public void GetDescription_ReturnsCorrectDescriptionForBE()
    {
        var result = LicenseHelper.GetDescription(LicenseClass.BE);
        Assert.That(result, Is.EqualTo("Pkw mit schwerem Anhänger"));
    }

    [Test]
    public void GetDescription_ReturnsCorrectDescriptionForC()
    {
        var result = LicenseHelper.GetDescription(LicenseClass.C);
        Assert.That(result, Is.EqualTo("Lkw (über 7,5t)"));
    }

    [Test]
    public void GetDescription_ReturnsCorrectDescriptionForT()
    {
        var result = LicenseHelper.GetDescription(LicenseClass.T);
        Assert.That(result, Is.EqualTo("Traktor & Zugmaschinen"));
    }

    [Test]
    public void GetDescription_ReturnsDefaultMessageForNull()
    {
        var result = LicenseHelper.GetDescription(null);
        Assert.That(result, Is.EqualTo("Bisher keine Angabe / Unbekannt"));
    }
}
