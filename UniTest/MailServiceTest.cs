using NUnit.Framework;

namespace UniTest;

[TestFixture]
public class MailServiceTest
{
    private static string mailAddres = "asdftest1234@gmail.com";

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
