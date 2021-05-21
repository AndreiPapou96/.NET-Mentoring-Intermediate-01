/*
 * 5. Write a program which creates two threads and a shared collection:
 * the first one should add 10 elements into the collection and the second should print all elements
 * in the collection after each adding.
 * Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task5.Threads.SharedCollection
{
    class Program
    {
        const int TaskCount = 2;
        static List<int> numbers = new List<int>();
        static object locker = new object();

        static void Main(string[] args)
        {
            var tasks = new Task[TaskCount];
            Console.WriteLine("5. Write a program which creates two threads and a shared collection:");
            Console.WriteLine("the first one should add 10 elements into the collection and the second should print all elements in the collection after each adding.");
            Console.WriteLine("Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.");
            Console.WriteLine();

            tasks[0] = Task.Run(Read);
            tasks[1] = Task.Run(Write);

            Console.ReadLine();
        }

        static void Read()
        {
            var currentCount = numbers.Count;

            while (true)
            {
                if (currentCount < numbers.Count)
                {
                    Console.WriteLine($"{currentCount} - {numbers.Count}: {string.Join(", ", numbers)}");
                    currentCount = numbers.Count;
                }                
            }
        }

        static async void Write()
        {
            var random = new Random();
            for (var i = 0; i < 10; i++)
            {
                await Task.Delay(random.Next(3, 5) * 1000);
                numbers.Add(i);
            }
        }
    }
}
