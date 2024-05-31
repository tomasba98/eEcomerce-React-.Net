namespace eEcomerce.BackEnd.Entities;
/// <summary>
/// Represents a base entity.
/// </summary>
public abstract class EntityBase
{
    /// <summary>
    /// Gets or sets the ID of the entity.
    /// </summary>
    public Guid Id { get; set; }
}