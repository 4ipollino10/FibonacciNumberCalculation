using System.Collections.Concurrent;
using System.Numerics;
using Common.Exceptions;
using Common.Models;

namespace Common;

/// <summary>
/// Сервис по работе с последовательностями Фибоначчи
/// </summary>
public class FibonacciSequenceNumberCalculator
{
    public const string FibonacciFilesDirectoryPath = "..\\..\\..\\..\\FibonacciSequences";
    private readonly ConcurrentDictionary<Guid, BigInteger> _lastFibonacciSequenceNumbers = new();
    public static readonly BigInteger FirstSignificantFibonacciSequenceNumber = 1;

    /// <summary>
    /// Вычислит следующее число последовательности Фибоначчи
    /// </summary>
    /// <param name="currentNumber"></param>
    /// <returns></returns>
    public FibonacciSequenceNumber CalculateNextFibonacciNumber(FibonacciSequenceNumber currentNumber)
    {
        var number = DeserializeBigInteger(currentNumber.Number);
        
        var lastFibonacciNumber = GetLastFibonacciSequenceNumber(currentNumber.SequenceId);
        var nextFibonacciNumber = lastFibonacciNumber + number;

        //Console.WriteLine($"New number {nextFibonacciNumber} calculated with id {currentNumber.SequenceId}");
        
        if (nextFibonacciNumber < 0)
        {
            throw new Exception("Long overflow!");
        }
        
        currentNumber.Number = nextFibonacciNumber.ToString();
        
        RecordLastSequenceNumber(currentNumber);
        return currentNumber;
    }
    
    /// <summary>
    /// Вернет последнее число последовательности Фибоначчи
    /// </summary>
    /// <param name="sequenceId"></param>
    /// <returns></returns>
    /// <exception cref="FibonacciSequenceNotFoundException"></exception>
    private BigInteger GetLastFibonacciSequenceNumber(Guid sequenceId)
    {
        if (_lastFibonacciSequenceNumbers.TryGetValue(sequenceId, out var lastSequenceNumber) == default)
        {
            _lastFibonacciSequenceNumbers.TryAdd(sequenceId, FirstSignificantFibonacciSequenceNumber);

            lastSequenceNumber = _lastFibonacciSequenceNumbers[sequenceId];
        }

        return lastSequenceNumber;
    }
    
    /// <summary>
    /// Запишет последнее число последовательности Фибоначчи
    /// </summary>
    /// <param name="currentNumber"></param>
    public void RecordLastSequenceNumber(FibonacciSequenceNumber currentNumber)
    {
        var number = BigInteger.Parse(currentNumber.Number);
        
        if (_lastFibonacciSequenceNumbers.GetValueOrDefault(currentNumber.SequenceId) == default)
        {
            _lastFibonacciSequenceNumbers.TryAdd(currentNumber.SequenceId, number);
        }

        _lastFibonacciSequenceNumbers[currentNumber.SequenceId] = number;
        
        //Console.WriteLine($"Last number {_lastFibonacciSequenceNumbers[currentNumber.SequenceId]} recorded with id {currentNumber.SequenceId}");
        
        WriteLastSequenceNumberToFile(currentNumber.SequenceId);
    }
    
    /// <summary>
    /// Создаст файл для последовательности Фибоначчи при отсутствии и запишет вычисленное число в существующий файл
    /// </summary>
    /// <param name="sequenceId"></param>
    private void WriteLastSequenceNumberToFile(Guid sequenceId)
    {
        var directoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, FibonacciFilesDirectoryPath);
        var filePath = Path.Combine(directoryPath, $"{sequenceId}.txt");

        if (File.Exists(filePath))
        {
            try
            {
                using var sw = File.AppendText(filePath);
                sw.WriteLine(GetLastFibonacciSequenceNumber(sequenceId));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while trying to record sequence number" + ex.Message);
            }
        }
        else
        {
            try
            {
                using var sw = new StreamWriter(filePath);
                sw.WriteLine("0");
                sw.WriteLine("1");
                sw.WriteLine("1");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while trying to record sequence number" + ex.Message);
            }
        }
    }

    public BigInteger DeserializeBigInteger(string number)
    {
        return BigInteger.Parse(number);
    }
}