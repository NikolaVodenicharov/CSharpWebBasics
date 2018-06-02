namespace AsynchronousProcessing
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    public class AsyncAndAwait
    {
        public static async void DoWork()
        {
            Console.WriteLine("Begin");
            var tasks = new List<Task>();
            var results = new List<bool>();

            for (int i = 0; i < 10; i++)
            {
                tasks.Add(Task.Run(async () =>
                {
                    var result = await SlowMethod();
                }));
            }

            Console.WriteLine("Processing...");

            //Task.WaitAll(tasks.ToArray());       // Threads that finish they job are waiting

            await Task.WhenAll(tasks.ToArray());    // release thread to do other job when finish the the currnet job

            Console.WriteLine("Finish");
        }

        private static async Task<bool> SlowMethod()
        {
            Thread.Sleep(1000);
            Console.WriteLine("Result");

            return true;
        }

        public static void Download()
        {
            Task
                .Run(async () =>
                {
                    var httpClient = new HttpClient();
                    var result = await httpClient.GetStringAsync("http://softuni.bg");

                    Console.WriteLine(result);
                })
                .GetAwaiter()
                .GetResult();
        }
    }
}
