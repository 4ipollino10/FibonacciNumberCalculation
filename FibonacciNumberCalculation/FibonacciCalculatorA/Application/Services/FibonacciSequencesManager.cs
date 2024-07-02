using Common;
using Common.Models;
using FibonacciCalculatorA.Infrastructure.ExternalServices;

namespace FibonacciCalculatorA.Application.Services;

/// <summary>
/// Бекграундовый сервис для планирования задач по вычислению последовательностей Фибоначчи
/// </summary>
/// <param name="calculator"></param>
public class FibonacciSequencesManager(FibonacciSequenceNumberCalculator calculator, IRefitFibonacciCalculatorB fibonacciBCalculator)
{
    /// <summary>
    /// Создаст задачи на вычисление чисел Фибоначчи
    /// </summary>
    /// <param name="amountOfSequences"></param>
    public List<Guid> StartCalculatingFibonacciSequencesTasks(int amountOfSequences)
    {
        var sequenceIds = new List<Guid>();
        
        for (var i = 0; i < amountOfSequences; ++i)
        {
            var sequenceId = Guid.NewGuid();

            var fibonacciSequenceNumber = new FibonacciSequenceNumber()
            {
                SequenceId = sequenceId,
                Number = FibonacciSequenceNumberCalculator.FirstSignificantFibonacciSequenceNumber.ToString()
            };
            
            calculator.RecordLastSequenceNumber(fibonacciSequenceNumber);
            
            sequenceIds.Add(sequenceId);
        }

        Parallel.ForEach(sequenceIds,(id) =>
        {
            fibonacciBCalculator.CalculateNexFibonacciSequenceNumber(new FibonacciSequenceNumber()
            {
                SequenceId = id,
                Number = FibonacciSequenceNumberCalculator.FirstSignificantFibonacciSequenceNumber.ToString()
            });
        });
        
        return sequenceIds;
    }

    /// <summary>
    /// Вычислит следующее число Фибоначчи
    /// </summary>
    /// <param name="currentNumber"></param>
    public FibonacciSequenceNumber CalculateNextNumber(FibonacciSequenceNumber currentNumber)
    {
        var calculatedFibonacciSequenceNumber = calculator.CalculateNextFibonacciNumber(currentNumber);

        return calculatedFibonacciSequenceNumber;
    }
}