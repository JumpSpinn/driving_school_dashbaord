namespace DrivingSchoolDashboard.Tests.Data;

[TestFixture]
public sealed class DialogDataTests
{
    // ---------------------------------------------------------------------------
    // DialogDefaultOptions Tests
    // ---------------------------------------------------------------------------

    [Test]
    public void DialogDefaultOptions_HasCorrectMaxWidth()
    {
        Assert.That(DialogData.DialogDefaultOptions.MaxWidth, Is.EqualTo(MaxWidth.Small));
    }

    [Test]
    public void DialogDefaultOptions_HasFullWidth()
    {
        Assert.That(DialogData.DialogDefaultOptions.FullWidth, Is.True);
    }

    [Test]
    public void DialogDefaultOptions_BackdropClickIsFalse()
    {
        Assert.That(DialogData.DialogDefaultOptions.BackdropClick, Is.False);
    }

    [Test]
    public void DialogDefaultOptions_CloseOnEscapeKeyIsTrue()
    {
        Assert.That(DialogData.DialogDefaultOptions.CloseOnEscapeKey, Is.True);
    }

    [Test]
    public void DialogDefaultOptions_HasCorrectBackgroundClass()
    {
        Assert.That(DialogData.DialogDefaultOptions.BackgroundClass, Is.EqualTo("dialog_backdrop_blur"));
    }

    // ---------------------------------------------------------------------------
    // DialogInfoOptions Tests
    // ---------------------------------------------------------------------------

    [Test]
    public void DialogInfoOptions_HasCorrectMaxWidth()
    {
        Assert.That(DialogData.DialogInfoOptions.MaxWidth, Is.EqualTo(MaxWidth.Small));
    }

    [Test]
    public void DialogInfoOptions_HasFullWidth()
    {
        Assert.That(DialogData.DialogInfoOptions.FullWidth, Is.True);
    }

    [Test]
    public void DialogInfoOptions_BackdropClickIsTrue()
    {
        Assert.That(DialogData.DialogInfoOptions.BackdropClick, Is.True);
    }

    [Test]
    public void DialogInfoOptions_CloseOnEscapeKeyIsTrue()
    {
        Assert.That(DialogData.DialogInfoOptions.CloseOnEscapeKey, Is.True);
    }

    [Test]
    public void DialogInfoOptions_HasCorrectBackgroundClass()
    {
        Assert.That(DialogData.DialogInfoOptions.BackgroundClass, Is.EqualTo("dialog_backdrop_blur"));
    }

    // ---------------------------------------------------------------------------
    // Comparison Tests
    // ---------------------------------------------------------------------------

    [Test]
    public void DialogOptions_BackdropClickDifference()
    {
        // DialogDefaultOptions does not close on backdrop click (important modals)
        Assert.That(DialogData.DialogDefaultOptions.BackdropClick, Is.False);
        
        // DialogInfoOptions closes on backdrop click (informational dialogs)
        Assert.That(DialogData.DialogInfoOptions.BackdropClick, Is.True);
    }
}
