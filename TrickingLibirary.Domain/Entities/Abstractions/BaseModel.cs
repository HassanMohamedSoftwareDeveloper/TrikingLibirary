﻿namespace TrickingLibirary.Domain.Entities.Abstractions;

public abstract class BaseModel<TKey>
{
    public TKey Id { get; set; }
    public bool Deleted { get; set; }
}
