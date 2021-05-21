/*
 * 2.	Write a program, which creates a chain of four Tasks.
 * First Task – creates an array of 10 random integer.
 * Second Task – multiplies this array with another random integer.
 * Third Task – sorts this array by ascending.
 * Fourth Task – calculates the average value. All this tasks should print the values to console.
 */
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task2.Chaining
{
    class Program
    {
        const int IntAmount = 10;
        static object lockObject = new object();
        static void Main(string[] args)
        {
            Console.WriteLine($"Main Thread - {ShowThreadInformation(Task.CurrentId.ToString())}");
            Console.WriteLine(".Net Mentoring Program. MultiThreading V1 ");
            Console.WriteLine("2.	Write a program, which creates a chain of four Tasks.");
            Console.WriteLine("First Task – creates an array of 10 random integer.");
            Console.WriteLine("Second Task – multiplies this array with another random integer.");
            Console.WriteLine("Third Task – sorts this array by ascending.");
            Console.WriteLine("Fourth Task – calculates the average value. All this tasks should print the values to console");
            Console.WriteLine();

            var generateArray = Task.Run(() => GenerateArray(IntAmount));
            var multiplyArray = generateArray.ContinueWith(array => MultiplyArray(array.Result));
            var sortArray = multiplyArray.ContinueWith(array => SortArray(array.Result));
            var calculateAverage = sortArray.ContinueWith(array => CalculateAverage(array.Result));

            calculateAverage.Wait();

            Console.ReadLine();
        }

        static int[] GenerateArray(int number)
        {
            var random = new Random();
            var array = new int[number];

            for (var i = 0; i < number; i++)
            {
                array[i] = random.Next(101);
            }

            Console.WriteLine($"Task #1 - New Array: [{string.Join(", ", array)}] - {ShowThreadInformation(Task.CurrentId.ToString())}");
            return array;
        }

        static int[] MultiplyArray(int[] array)
        {
            var random = new Random();
            var newArray = new int[array.Length];
            var multiplier = random.Next(101);

            for (var i = 0; i < array.Length; i++)
            {
                newArray[i] = array[i] * multiplier;
            }

            Console.WriteLine($"Task #2 - Multplied Array: [{string.Join(", ", newArray)}]; Multiplier: {multiplier} - {ShowThreadInformation(Task.CurrentId.ToString())}");
            return newArray;
        }

        static int[] SortArray(int[] array)
        {
            var newArray = array.OrderBy(number => number).ToArray();

            Console.WriteLine($"Task #3 - Sorted Array: [{string.Join(", ", newArray)}] - {ShowThreadInformation(Task.CurrentId.ToString())}");
            return newArray;
        }

        static double CalculateAverage(int[] array)
        {
            var average = array.Average();

            Console.WriteLine($"Task #4 - Average Value: {average} - {ShowThreadInformation(Task.CurrentId.ToString())}");
            return average;
        }

        private static string ShowThreadInformation(String taskName)
        {
            String msg = null;
            Thread thread = Thread.CurrentThread;
            lock (lockObject)
            {
                msg = String.Format("{0} thread information\n", taskName) +
                      String.Format("   Background: {0}\n", thread.IsBackground) +
                      String.Format("   Thread Pool: {0}\n", thread.IsThreadPoolThread) +
                      String.Format("   Thread ID: {0}\n", thread.ManagedThreadId);
            }

            return msg;
        }
    }
}
