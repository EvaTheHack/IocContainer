using System;

namespace IocContainer.Tests.Services
{
    public class Logger : ILogger
    {
        public void Show(string message)
        {
            Console.WriteLine(message);
        }
    }
}
