using FileHelpers;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discounter
{
    class Parser
    {
        
        public static Array readFile(string path) {
            
            FileHelperEngine engine = new FileHelperEngine(typeof(Member));
            Member[] mem = engine.ReadFile(path) as Member[];
 
            return mem;
        }

        public static void writeFile(string path, Member[] mem){ 

            FileHelperEngine engine = new FileHelperEngine(typeof(Member));
            engine.WriteFile(path, mem);

        }

    }
}
