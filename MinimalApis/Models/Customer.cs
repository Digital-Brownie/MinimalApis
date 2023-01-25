using JetBrains.Annotations;
using MinimalApis.Interfaces;

namespace MinimalApis.Models;

[UsedImplicitly]
public record Customer(Guid Id, string Username, string Email, DateOnly DateOfBirth) : IIdentifiable<Guid>
{
    public Guid Id { get; set; } = Id;
}
