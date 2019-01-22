using System;
using System.Collections.Generic;
using CourseWorkBCT.EconomicalCodes.ElementsTree;

namespace CourseWorkBCT.EconomicalCodes
{
    public class CodeHaffman : CodeShennonaFano
    {
        public CodeHaffman(Dictionary<string, double> probalitiesSymbol) : base(probalitiesSymbol) { }
        //
        // Метод создания кодового дерева Хаффмана.
        protected override Dictionary<string, string> CreatingTree()
        {
            while (leafsTree.Count > 1)
            {
                var left = leafsTree[0];
                var right = leafsTree[1];
                leafsTree.RemoveAt(1);
                leafsTree.RemoveAt(0);

                double Pi = left.getPi() + right.getPi();
                string Name = Convert.ToString(Pi);

                leafsTree.Add(new Node(Name, Pi, left, right));
                
                leafsTree = SortingLeafs(leafsTree);
            }

            rootTree = leafsTree[0];
            leafsTree = null;
            Dictionary<string, string> tableCodes = new Dictionary<string, string>();

            rootTree.searchSymbol(tableCodes, "");

            return tableCodes;
        }
        
    }
}
