using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using restgen.Generator;

namespace restgen
{
    class Program
    {
        static void Main(string[] args)
        {
            EntityGenerator eg = new EntityGenerator();
            eg.start();
        }
    }
}
