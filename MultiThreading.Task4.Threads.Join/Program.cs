/*
 * 4.	Write a program which recursively creates 10 threads.
 * Each thread should be with the same body and receive a state with integer number, decrement it,
 * print and pass as a state into the newly created thread.
 * Use Thread class for this task and Join for waiting threads.
 * 
 * Implement all of the following options:
 * - a) Use Thread class for this task and Join for waiting threads.
 * - b) ThreadPool class for this task and Semaphore for waiting threads.
 */

using System;
using System.Collections.Generic;
using System.Threading;

namespace MultiThreading.Task4.Threads.Join
{
    class Program
    {
        const int ThreadCount = 10;
        const int MaxRunningThreads = 3;
        static Semaphore semaphore = new Semaphore(1, 1);

        static void Main(string[] args)
        {
            Console.WriteLine("4.	Write a program which recursively creates 10 threads.");
            Console.WriteLine("Each thread should be with the same body and receive a state with integer number, decrement it, print and pass as a state into the newly created thread.");
            Console.WriteLine("Implement all of the following options:");
            Console.WriteLine();
            Console.WriteLine("- a) Use Thread class for this task and Join for waiting threads.");
            Console.WriteLine("- b) ThreadPool class for this task and Semaphore for waiting threads.");

            Console.WriteLine();

            // - a) Use Thread class for this task and Join for waiting threads.
            Console.WriteLine("Create thread recursively using Thread Class and Thread.Join: ");
            ProcessThreadA(ThreadCount);
      
            Console.ReadLine();

            Console.WriteLine("Create thread recursively using ThreadPool class and Semaphore: ");
            ProcessThreadB(ThreadCount);
            Console.ReadLine();
        }

        static void ProcessThreadA(int i)
        {
            if (i > 0)
            {
                var thread = new Thread(() => ProcessThreadA(i - 1));
                
                Console.WriteLine($"Option A: Thread #{i} created");
                thread.Start();
                thread.Join();
                Console.WriteLine($"Option A: Thread #{i} completed");
            }
        }

        static void ProcessThreadB(object state)
        {
            var i = (int)state;
            if (i > 0)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(ProcessThreadB), i - 1);
                Console.WriteLine($"Option B: Thread #{i} created");
                semaphore.WaitOne();
                Console.WriteLine($"Option B: Thread #{i} completed");
                semaphore.Release();
            }
        }
    }
}
