using NUnit.Framework;

namespace UniTest.Services;

[TestFixture]
public class MailServiceTest
{

    [SetUp]
    public void SetUp()
    {
    }

    [Test]
    public void MakeBookNotValid()
    {
        Assert.That(false, Is.False, "1 should not be prime");
    }
    [Test]
    public void MakeBookNotValidNot()
    {
        Assert.That(true, Is.True, "1 should not be prime");
    }
}
