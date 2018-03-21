using System;

namespace Sample.BusinessLogic
{
    public class DefaultOutputWriter : IOutputWriter
    {
        public void WriteLine(string message)
        {
            Console.WriteLine(message);
        }
    }
}