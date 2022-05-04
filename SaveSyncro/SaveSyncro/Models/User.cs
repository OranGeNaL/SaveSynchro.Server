using System.Text.Json.Serialization;

namespace SaveSyncro.Models;

public class User
{
    public string Login { get; set; }
    public string Password { get; set; }
    public string UserID { get; set; } = "";

    public User()
    { }
    
    public User(string login, string password)
    {
        Login = login;
        Password = password;
    }
    public User(string login, string password, string userId)
    {
        Login = login;
        Password = password;
        UserID = userId;
    }
}