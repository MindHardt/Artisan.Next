namespace Arklens.Tests;

public class AlidTests
{
    [Theory]
    [InlineData("spell:wizard:fireball")]
    [InlineData("trait:expert+swimming")]
    [InlineData("weapon:rapier+well_made+flexible")]
    public void TestParse(string alidText)
    {
        var alid = Alid.Alid.Parse(alidText);
        
        Assert.Equal(alidText, alid.Text);
    }
}