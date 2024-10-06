using SpaceDash.Models;
using System.ComponentModel.DataAnnotations;

public class Challenge
{
    public int Id { get; set; }
    [Required]
    public string Type { get; set; }
    public int Order { get; set; }
    [Required]
    public string Name { get; set; }
    public string Description { get; set; }
    public int DeviceId { get; set; }
    public Device Device { get; set; }
    public int GameSessionId { get; set; }
    public GameSession GameSession { get; set; }
    public List<Question> Questions { get; set; }
    public int? RequiredHighFives { get; set; }
    public bool IsCompleted { get; set; }

    // Properties for Sudoku challenges
    public string? SudokuPuzzle { get; set; }
    public string? Solution { get; set; }
    public int TimeLimit { get; set; }

}