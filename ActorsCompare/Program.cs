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
            int firstId = 7836;
            int secondId = 9838;

            ActorsComparator comparator = new ActorsComparator(firstId, secondId);
            
            comparator.CompareActors();
        }
    }
}
