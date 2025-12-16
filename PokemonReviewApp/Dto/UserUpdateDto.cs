public class UserUpdateDto
{
    public int Id { get; set; }    // REQUIRED for ID check
    public string? Username { get; set; }

    public int RoleId { get; set; }
    public string? OldPassword { get; set; }
    public string? NewPassword { get; set; }
}
