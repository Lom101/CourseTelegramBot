﻿namespace Core.Entity;

public class Course
{
    public int Id { get; set; }
    
    public string Title { get; set; }
    public string Description { get; set; }
    
    public List<Topic> Topics { get; set; } = new();
}