namespace AsynchronousProcessing
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class Startup
    {
        public static void Main(string[] args)
        {
            AsyncAndAwait.Download();
        }
    }
}
