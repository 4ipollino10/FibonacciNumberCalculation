using Common;
using Common.Models;
using Refit;

namespace FibonacciCalculatorA.Infrastructure.ExternalServices;

public interface IRefitFibonacciCalculatorB
{
    [Post($"{RouteConstants.FibonacciCalculatorB.FibonacciCalculatorBControllerRoute}/{RouteConstants.FibonacciCalculatorB.CalculateNextFibonacciSequenceRoute}")]
    Task CalculateNexFibonacciSequenceNumber(FibonacciSequenceNumber currentNumber);
}