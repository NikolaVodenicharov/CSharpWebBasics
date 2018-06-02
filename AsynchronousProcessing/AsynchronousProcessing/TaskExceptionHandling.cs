using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace AsynchronousProcessing
{
    public class TaskExceptionHandling
    {
        public static void Run()
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var directoryInfo = new DirectoryInfo(currentDirectory + "\\Images");
            var files = directoryInfo.GetFiles();

            const string resultDirectory = "Result";
            if (Directory.Exists(resultDirectory))
            {
                Directory.Delete(resultDirectory, true);
            }
            Directory.CreateDirectory(resultDirectory);

            var tasks = new List<Task>();

            foreach (var file in files)
            {
                var task = Task.Run(() =>
                {
                    // var image = Image.FromFile(file.FullName);
                    var image = Image.FromFile("Invalid file");
                    image.RotateFlip(RotateFlipType.RotateNoneFlipY);
                    image.Save($"{resultDirectory}\\flip-{file.Name}");

                    Console.WriteLine($"{file.Name} processed...");
                });

                tasks.Add(task);
            }

            try
            {
                Task.WaitAll(tasks.ToArray());

            }
            catch (AggregateException ex)
            {
                foreach (var exception in ex.InnerExceptions)
                {
                    Console.WriteLine(exception);
                }
            }
            Console.WriteLine("Finished");
        }
    }
}
