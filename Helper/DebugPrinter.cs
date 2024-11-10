using Microsoft.OpenApi.Any;

namespace WebXeDapAPI.Helper
{
    public class DebugPrinter
    {
        public static void DebugPrint(string message)
        {
            Console.WriteLine("----------------------");
            Console.WriteLine(message);
            Console.WriteLine("----------------------");

        }
    }
}
