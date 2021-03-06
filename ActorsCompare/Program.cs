using System;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Collections.Generic;

namespace ActorsCompare
{   
    class Program
    {
        static void Main(string[] args)
        {
            SearchModule search = new SearchModule();

            Console.WriteLine("Ищем первого актера...\n");
            Console.WriteLine("Введите имя для поиска: ");
            int actor1 = search.ByName(Console.ReadLine());

            Console.WriteLine("Ищем второго актера...\n");
            Console.WriteLine("Введите имя для поиска: ");
            int actor2 = search.ByName(Console.ReadLine());

            Console.WriteLine("Actor1: " + actor1 + "\nActor2: " + actor2);

            ActorsComparator comparator = new ActorsComparator(actor1, actor2);            
            comparator.CompareActors();
        }
    }
}
