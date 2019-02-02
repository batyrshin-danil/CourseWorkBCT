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
            while (leavesTree.Count > 1)
            {
                var left = leavesTree[0];
                var right = leavesTree[1];
                leavesTree.RemoveAt(1);
                leavesTree.RemoveAt(0);

                double Pi = left.getPi() + right.getPi();
                string Name = Convert.ToString(Pi);

                leavesTree.Add(new Node(Name, Pi, left, right));
                
                leavesTree = SortingLeafs(leavesTree);
            }

            rootTree = leavesTree[0];
            leavesTree = null;
            Dictionary<string, string> tableCodes = new Dictionary<string, string>();

            rootTree.searchSymbol(tableCodes, "");

            return tableCodes;
        }
        
    }
}
