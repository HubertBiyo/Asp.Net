using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using WcfContractsEx02;

namespace ConsoleAppEx02Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Person[] people = new Person[]
                {
                    new Person{ Mark=46,Name="Jim"},
                    new Person{ Mark=39,Name="Mike"},
                    new Person{ Mark=28,Name="Jack"},
                    new Person{ Mark=12,Name="Hubert"}
                };
            Console.WriteLine("People:");
            OutPutPeople(people);
            IAwardService client = ChannelFactory<IAwardService>.CreateChannel(
                new WSHttpBinding(),
                new EndpointAddress("http://localhost:59574/AwardService.svc")
                );
            client.SetPassMark(30);
            Person[] awardedPeople = client.GetAwardedPeople(people);
            Console.WriteLine();
            Console.WriteLine("Awarded people:");
            OutPutPeople(awardedPeople);
            Console.ReadKey();

        }
        static void OutPutPeople(Person[] people)
        {
            foreach (Person person in people)
            {
                Console.WriteLine("{0},Mark:{1}", person.Name, person.Mark);
            }
        }
    }
}
