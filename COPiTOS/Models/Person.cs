﻿namespace COPiTOS.Models;

public class Person
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime BirthDate { get; set; }
    public string? Address { get; set; }
    public string? Index { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
}