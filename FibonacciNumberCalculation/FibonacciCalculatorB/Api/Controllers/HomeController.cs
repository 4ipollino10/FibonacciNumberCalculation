using Common;
using Common.Models;
using FibonacciCalculatorB.Application.Publishers;
using Microsoft.AspNetCore.Mvc;

namespace FibonacciCalculatorB.Api.Controllers;

[ApiController, Route($"{RouteConstants.FibonacciCalculatorB.FibonacciCalculatorBControllerRoute}")]
public class HomeController(NextFibonacciSequenceNumberCalculatedPublisher publisher, FibonacciSequenceNumberCalculator calculator)
{
    [HttpPost, Route($"{RouteConstants.FibonacciCalculatorB.CalculateNextFibonacciSequenceRoute}")]
    public async Task CalculateNextFibonacciSequenceNumber(FibonacciSequenceNumber currentNumber)
    {
        try
        {
            var nextCalculatedFibonacciNumber = calculator.CalculateNextFibonacciNumber(currentNumber);
            await publisher.SendCalculatedNumber(nextCalculatedFibonacciNumber);
        }
        catch (Exception)
        {
            // ignored
        }
    }
}