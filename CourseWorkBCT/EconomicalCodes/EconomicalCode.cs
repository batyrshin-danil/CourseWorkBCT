using System;
using System.Collections.Generic;
using System.Linq;

using CourseWorkBCT.EconomicalCodes.ElementsTree;

namespace CourseWorkBCT.EconomicalCodes
{
    public abstract class EconomicalCode
    {
        protected List<Node> leavesTree = new List<Node>();
        protected Node rootTree;
        //
        // Словарь для хранения пар "символ:код".
        public Dictionary<string, string> economicalCodes { get; private set; }

        public EconomicalCode(Dictionary<string, double> probalitiesSymbol)
        {
            CreatingListLeaf(probalitiesSymbol);
            economicalCodes = CreatingTree();
        }
        //
        // Создаем список листьев кодового дерева и сортируем его по возрастания слева-направо.
        protected void CreatingListLeaf(Dictionary<string, double> probalitiesSymbol)
        {
            foreach (string key in probalitiesSymbol.Keys)
            {
                leavesTree.Add(new Leaf(key, probalitiesSymbol[key]));
            }
            // Сортировка списка листьев по возрастанию.
            leavesTree = SortingLeafs(leavesTree);
        }
        //
        // Метод создания кодового дерева экономного кода.
        protected abstract Dictionary<string, string> CreatingTree();
        //
        // Метод реализующий сортировку списка листьев по возрастанию.
        protected List<Node> SortingLeafs(List<Node> leafs)
        {
            return leafs.OrderBy(Node => Node.getPi()).ToList();
        }
    }
}
