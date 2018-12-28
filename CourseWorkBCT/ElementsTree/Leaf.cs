using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWorkBCT.ElementsTree
{
    //
    // Класс описывающий лист дерева, как частный случай узла.
    //
    internal sealed class Leaf : Node
    {
        public Leaf(string Name, double Pi) : base(Name, Pi, null, null)
        {

        }

        public override void searchSymbol(Dictionary<string, string> tableCodes, string acc)
        {
            tableCodes[Name] = acc;
        }
    }
}
