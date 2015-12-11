using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using BookCardIndexServiceLib;

namespace BookCardIndexServiceHost
{
    class Program
    {
        static void DisplayHostlnfo(ServiceHost host)
        {
            Console.WriteLine();
            Console.WriteLine("***** Host Info *****");
            foreach (System.ServiceModel.Description.ServiceEndpoint se in host.Description.Endpoints)
            {
                Console.WriteLine("Address: {0}", se.Address); // адрес
                Console.WriteLine("Binding: {0}", se.Binding.Name); // привязка
                Console.WriteLine("Contract: {0}", se.Contract.Name); // контракт
                Console.WriteLine();
            }
            Console.WriteLine("**********************");
        }
        static void Main(string[] args)
        {
            Console.WriteLine("***** Console Based WCF Host *****");
            using (ServiceHost serviceHost = new ServiceHost(typeof(BookCardIndexService)))
            {
                // Открыть хост и начать прослушивание входных сообщений.
                serviceHost.Open();
                DisplayHostlnfo(serviceHost);
                // Оставить службу функционирующей до тех пор, пока не будет нажата клавиша <Enter>
                Console.WriteLine("The service is ready.");
                Console.WriteLine("Press the Enter key to terminate service.");
                Console.ReadLine();
            }
        }
    }
}