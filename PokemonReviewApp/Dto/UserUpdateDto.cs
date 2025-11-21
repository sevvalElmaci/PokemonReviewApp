public class UserUpdateDto
{
    public int Id { get; set; }    // REQUIRED for ID check
    public string? Username { get; set; }
    public string? Password { get; set; }
    public int RoleId { get; set; }
}
