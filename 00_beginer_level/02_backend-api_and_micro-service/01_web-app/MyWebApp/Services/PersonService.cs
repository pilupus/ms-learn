using MyWebApp.Interfaces;

namespace MyWebApp.Services;

public class PersonService: IPersonService
{
    public string GetPersonName()
    {
        return "John Doe";
    }
}