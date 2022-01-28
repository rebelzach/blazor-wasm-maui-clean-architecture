using System.Collections.Generic;

namespace Thing.SharedKernel;

/// <summary>
/// Base types for all Entities which track state using a given Id.
/// </summary>
public abstract class BaseEntity<TId>
{
    private TId? _id;

    public TId Id { get => _id!; set => _id = value; }
}
