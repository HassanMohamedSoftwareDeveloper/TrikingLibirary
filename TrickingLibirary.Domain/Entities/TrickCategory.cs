﻿namespace TrickingLibirary.Domain.Entities;

public class TrickCategory
{
    public int TrickId { get; set; }
    public Trick Trick { get; set; }
    public string CategoryId { get; set; }
    public Category Category { get; set; }
}
