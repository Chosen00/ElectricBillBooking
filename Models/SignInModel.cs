using System.ComponentModel.DataAnnotations;

public class SignInModel
{
    public string Username { get; set; }

    public string Password { get; set; }

    public SignInModel()
    {
        Username = "";
        Password = "";
    }
}
