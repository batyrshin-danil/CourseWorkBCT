using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CourseWorkBCT.ElementsTree;

namespace CourseWorkBCT.BudgetCodes
{
    class CodeHaffman
    {
        private List<Node> leafHaffmanTree = new List<Node>();
        private Node rootTreeHaffman;
        //
        // Словарь для хранения пар "символ:код".
        public Dictionary<string, string> tableCodes { get; private set; }

        public CodeHaffman(Dictionary<string, double> probSymbol)
        {
            CreatingListLeaf(probSymbol);
            tableCodes = CreatingTreeHaffman();
        }
        //
        // Создаем список листьев кодового дерева Хаффмана и сортируем его по возрастания слева-направо.
        private void CreatingListLeaf(Dictionary<string,double> probSymbol)
        {
            foreach(string key in probSymbol.Keys)
            {
                leafHaffmanTree.Add(new Leaf(key, probSymbol[key]));
            }
            // Сортировка списка листьев по возрастанию.
            leafHaffmanTree = SortingLeafList(leafHaffmanTree);
        }
        //
        // Метод создания кодового дерева Хаффмана.
        private Dictionary<string, string> CreatingTreeHaffman()
        {
            while (leafHaffmanTree.Count > 1)
            {
                var left = leafHaffmanTree[0];
                var right = leafHaffmanTree[1];
                leafHaffmanTree.RemoveAt(1);
                leafHaffmanTree.RemoveAt(0);

                double Pi = left.getPi() + right.getPi();
                string Name = Convert.ToString(Pi);

                leafHaffmanTree.Add(new Node(Name, Pi, left, right));
                
                leafHaffmanTree = SortingLeafList(leafHaffmanTree);
            }

            rootTreeHaffman = (Node) leafHaffmanTree[0];
            leafHaffmanTree = null;
            Dictionary<string, string> tableCodes = new Dictionary<string, string>();

            rootTreeHaffman.searchSymbol(tableCodes, "");

            return tableCodes;
        }
        //
        // Метод реализующий сортировку списка листьев по возрастанию.
        private List<Node> SortingLeafList(List<Node> leafList)
        {
            return leafList.OrderBy(Node => Node.getPi()).ToList();
        }
    }
}
