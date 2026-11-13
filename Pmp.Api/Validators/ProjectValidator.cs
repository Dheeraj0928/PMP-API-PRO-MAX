namespace Pmp.Api.Validators;
public class ProjectValidator
{
    public bool IsValidName(string name)
    {
        return !string.IsNullOrWhiteSpace(name);
    }
}