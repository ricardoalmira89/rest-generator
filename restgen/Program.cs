using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using restgen.Sync;

using restgen.Generator;

namespace restgen
{
    class Program
    {
        static void Main(string[] args)
        {
            EntityGenerator eg = new EntityGenerator();
            eg.start();

            ControllerGenerator cg = new ControllerGenerator(eg.entityName);
            cg.generate()
              .save();

            RepositoryGenerator rg = new RepositoryGenerator(eg.entityName);
            rg.generate()
              .save();

            SyncManager sm = new SyncManager();
            sm.writeReferencesCsproj(eg.entityName);
        }
    }
}
