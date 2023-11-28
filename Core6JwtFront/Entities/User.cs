using System.Text.Json.Serialization;

namespace Core6JwtFront.Entities;

public class User
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; }

}

public class UserResponse {
    public int? id { get; set; }
    public string? firstName { get; set; }
    public string? lastName { get; set; }
    public string? username { get; set; }
    public string? jwtToken { get; set; }

}