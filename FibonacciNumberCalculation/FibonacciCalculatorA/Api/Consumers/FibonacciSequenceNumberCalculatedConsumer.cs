using Common.Models;
using FibonacciCalculatorA.Application.Services;
using FibonacciCalculatorA.Infrastructure.ExternalServices;
using MassTransit;

namespace FibonacciCalculatorA.Api.Consumers;

/// <summary>
/// Консьюмер сообщений из второго приложения
/// </summary>
/// <param name="manager"></param>
public class FibonacciSequenceNumberCalculatedConsumer(FibonacciSequencesManager manager, IRefitFibonacciCalculatorB fibonacciCalculatorB) : IConsumer<FibonacciSequenceNumber>
{
    public async Task Consume(ConsumeContext<FibonacciSequenceNumber> context)
    {
        try
        {
            var result = manager.CalculateNextNumber(context.Message);
            await fibonacciCalculatorB.CalculateNexFibonacciSequenceNumber(result);
        }
        catch (Exception)
        {
            // ignored
        }
    }
}