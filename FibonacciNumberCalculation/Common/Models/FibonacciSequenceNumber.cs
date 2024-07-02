using System.Numerics;

namespace Common.Models;

/// <summary>
/// Модель элемента последовательности Фибоначчи
/// </summary>
public class FibonacciSequenceNumber
{
    /// <summary>
    /// Идентификатор последовательности
    /// </summary>
    public Guid SequenceId { get; set; }

    /// <summary>
    /// Вычисленное число
    /// </summary>
    public string Number { get; set; }
}