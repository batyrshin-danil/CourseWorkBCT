using System.Collections.Generic;

namespace CourseWorkBCT.EconomicalCodes.ElementsTree
{
    //
    // Класс описывающий лист дерева, как частный случай узла.
    //
    public sealed class Leaf : Node
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
