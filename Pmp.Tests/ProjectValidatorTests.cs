using Xunit;
using Pmp.Api.Validators;
namespace Pmp.Tests;
public class ProjectValidatorTests
{
    [Fact]
    public void EmptyProjectName_ShouldBeInvalid()
    {
        var validator = new ProjectValidator();
        bool result=validator.IsValidName("");
        Assert.False(result);
    }
}
