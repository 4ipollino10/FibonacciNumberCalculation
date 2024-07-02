namespace Common;

public class RouteConstants
{
    public class FibonacciCalculatorA
    {
        public const string FibonacciCalculatorAControllerRoute = "/api/fibonacciCalculatorA";

        public const string StartCalculatingFibonacciSequencesRoute = "calculate-sequences/{amountOfSequences}";

        public const string FibonacciSequenceDownloadRoute = "fibonacci-sequence-download/{sequenceId}";
    }
    
    public class FibonacciCalculatorB
    {
        public const string FibonacciCalculatorBControllerRoute = "/api/fibonacciCalculatorB";

        public const string CalculateNextFibonacciSequenceRoute = "calculate-next-number";
    }
    
}