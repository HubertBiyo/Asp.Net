using System;
using WcfServiceEx01;
namespace ConsoleAppEx01Client
{
    class Program
    {
        static void Main(string[] args)
        {
           var  Title = "ConsoleAppEx01";

            string numbericInput = null;
            int intParam;
            do
            {
                Console.WriteLine("Enter an integer and press enter to call the WCF Service.");
                numbericInput = Console.ReadLine();
            } while (!int.TryParse(numbericInput,out intParam));
            Service client = new Service();
            Console.WriteLine(client.GetData(intParam));
            Console.WriteLine("press an key to exit");
            Console.ReadKey();
        }
    }
}
