using Common;
using FibonacciCalculatorA.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace FibonacciCalculatorA.Api.Controllers;

/// <summary>
/// Контроллер для пользователя
/// </summary>
[ApiController, Route($"{RouteConstants.FibonacciCalculatorA.FibonacciCalculatorAControllerRoute}")]
public class HomeController(FibonacciSequencesManager fibonacciSequencesManager) : ControllerBase
{
    /// <summary>
    /// Начать вычисление последовательностей чисел Фибоначчи
    /// </summary>
    /// <param name="amountOfSequences"></param>
    /// <returns> Список идентификаторов, по которым можно следить за состоянием последовательностей </returns>
    [HttpPost, Route($"{RouteConstants.FibonacciCalculatorA.StartCalculatingFibonacciSequencesRoute}")]
    public List<Guid> StartCalculatingFibonacciSequences(int amountOfSequences)
    {
        return fibonacciSequencesManager.StartCalculatingFibonacciSequencesTasks(amountOfSequences);
    }
    
    /// <summary>
    /// Скачает текущее состояние последовательности чисел Фибоначчи
    /// </summary>
    /// <param name="sequenceId"></param>
    [HttpGet, Route(RouteConstants.FibonacciCalculatorA.FibonacciSequenceDownloadRoute)]
    public IActionResult GenerateAndDownloadPassingResultsReportOrThrow(Guid sequenceId)
    {
        var directoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, FibonacciSequenceNumberCalculator.FibonacciFilesDirectoryPath);
        var filePath = Path.Combine(directoryPath, $"{sequenceId}.txt");
        if (!System.IO.File.Exists(filePath)) return NotFound();
        var fileBytes = System.IO.File.ReadAllBytes(filePath);
        return File(fileBytes, "application/octet-stream", Path.GetFileName(filePath));
    }
}