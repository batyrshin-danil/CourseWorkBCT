using System;
using System.Collections.Generic;
using CourseWorkBCT.ElementsTree;

namespace CourseWorkBCT.BudgetCodes
{
    public class CodeHaffman : CodeShennonaFano
    {
        public CodeHaffman(Dictionary<string, double> probSymbol) : base(probSymbol) { }
        //
        // Метод создания кодового дерева Хаффмана.
        protected override Dictionary<string, string> CreatingTree()
        {
            while (leafTree.Count > 1)
            {
                var left = leafTree[0];
                var right = leafTree[1];
                leafTree.RemoveAt(1);
                leafTree.RemoveAt(0);

                double Pi = left.getPi() + right.getPi();
                string Name = Convert.ToString(Pi);

                leafTree.Add(new Node(Name, Pi, left, right));
                
                leafTree = SortingLeafList(leafTree);
            }

            rootTree = leafTree[0];
            leafTree = null;
            Dictionary<string, string> tableCodes = new Dictionary<string, string>();

            rootTree.searchSymbol(tableCodes, "");

            return tableCodes;
        }
        
    }
}
