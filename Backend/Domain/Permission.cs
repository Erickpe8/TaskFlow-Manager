namespace TaskFlow.Api.Domain;

public class Permission
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}
