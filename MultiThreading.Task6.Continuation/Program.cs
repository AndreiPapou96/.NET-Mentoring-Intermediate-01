/*
*  Create a Task and attach continuations to it according to the following criteria:
   a.    Continuation task should be executed regardless of the result of the parent task.
   b.    Continuation task should be executed when the parent task finished without success.
   c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation
   d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled
   Demonstrate the work of the each case with console utility.
*/
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task6.Continuation
{
    class Program
    {
        static CancellationTokenSource tokenSource = new CancellationTokenSource();
        static CancellationToken ct;
        static object lockObject = new object();
        static void Main(string[] args)
        {
            ct = tokenSource.Token;

            Console.WriteLine("Create a Task and attach continuations to it according to the following criteria:");
            Console.WriteLine("a.    Continuation task should be executed regardless of the result of the parent task.");
            Console.WriteLine("b.    Continuation task should be executed when the parent task finished without success.");
            Console.WriteLine("c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation.");
            Console.WriteLine("d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled.");
            Console.WriteLine("Demonstrate the work of the each case with console utility.");
            Console.WriteLine();

            // feel free to add your code
            /*Console.WriteLine("A:");
            var process = Task.Run(() => Process('c'));
            var continuation = process.ContinueWith(result => ContinueProcess(result.Status), TaskContinuationOptions.None);
            continuation.Wait();*/

            /*Console.WriteLine("B:");
            var process = Task.Run(() => Process('a'));
            var continuation = process.ContinueWith(result => ContinueProcess(result.Status), TaskContinuationOptions.NotOnRanToCompletion);
            continuation.Wait();*/

            /*Console.WriteLine("C:");
            var process = Task.Run(() => Process('c'));
            var continuation = process.ContinueWith(result => ContinueProcess(result.Status), TaskContinuationOptions.OnlyOnFaulted | TaskContinuationOptions.ExecuteSynchronously);
            continuation.Wait();*/

            Console.WriteLine("D:");
            
            var process = Task.Run(() => Process('a', ct), ct);
            Thread.Sleep(1000);
            tokenSource.Cancel();
            var continuation = process.ContinueWith(result => ContinueProcess(result.Status), TaskContinuationOptions.OnlyOnCanceled | TaskContinuationOptions.LongRunning);
            continuation.Wait();

            Console.ReadLine();
        }

        static void Process(char option, CancellationToken ct)
        {
            switch (option)
            {
                case 'a':
                    Console.Write($"A ran to completion. #{ShowThreadInformation(Task.CurrentId.ToString())}");
                    break;
                case 'c':
                    Console.WriteLine($"Throwing Exception... #{ShowThreadInformation(Task.CurrentId.ToString())}");
                    throw new Exception("exception, option c");
            }

            Task.Delay(3000).Wait();
            /*if (ct.IsCancellationRequested)
            {
                Console.WriteLine("Is Cancelled");
                return;
            }*/

            ct.ThrowIfCancellationRequested();

            Console.WriteLine("Test");
        }

        static void Process(char option)
        {
            
            switch (option)
            {
                case 'a':
                    Console.Write($"A ran to completion. #{ShowThreadInformation(Task.CurrentId.ToString())}");
                    break;
                case 'c':
                    Console.WriteLine($"Throwing Exception... #{ShowThreadInformation(Task.CurrentId.ToString())}");
                    throw new Exception("exception, option c");
            }

            Task.Delay(3000).Wait();
            Console.WriteLine("Test");
        }

        static void ContinueProcess(object result)
        {
            Console.WriteLine($"Continued: {result} - #{ShowThreadInformation(Task.CurrentId.ToString())}");

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
