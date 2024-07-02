using Common.Models;
using MassTransit;

namespace FibonacciCalculatorB.Application.Publishers;

public class NextFibonacciSequenceNumberCalculatedPublisher(IBus bus)
{
    public async Task SendCalculatedNumber(FibonacciSequenceNumber currentNumber)
    {
        await bus.Publish(new FibonacciSequenceNumber()
        {
            SequenceId = currentNumber.SequenceId,
            Number = currentNumber.Number
        });
    }
}