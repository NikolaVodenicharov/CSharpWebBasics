namespace AsynchronousProcessing
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Threading.Tasks;

    public class FlipPicture
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
                    var image = Image.FromFile(file.FullName);
                    image.RotateFlip(RotateFlipType.RotateNoneFlipY);
                    image.Save($"{resultDirectory}\\flip-{file.Name}");

                    Console.WriteLine($"{file.Name} processed...");
                });

                tasks.Add(task);
            }

            Task.WaitAll(tasks.ToArray());
            Console.WriteLine("Finished");
        }
    }
}
