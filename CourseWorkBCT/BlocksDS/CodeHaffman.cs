using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWorkOTS.BlocksDS
{
    class CodeHaffman
    {
        private List<Node> leafHaffmanTree = new List<Node>();
        private Node rootTreeHaffman;
        //
        // Словарь для хранения пар "символ:код".
        private Dictionary<string, string> tableCodes;

        public CodeHaffman(Dictionary<string, double> probalitiesSymbol)
        {
            CreatingListLeaf(probalitiesSymbol);
            tableCodes = CreatingTreeHaffman();
        }

        public Dictionary<string, string> getTableCodes()
        {
            return tableCodes;
        }
        //
        // Создаем список листьев кодового дерева Хаффмана и сортируем его по возрастания слева-направо.
        //
        private void CreatingListLeaf(Dictionary<string,double> probalitiesSymbol)
        {
            foreach(string key in probalitiesSymbol.Keys)
            {
                leafHaffmanTree.Add(new Leaf(key, probalitiesSymbol[key]));
            }
            //
            // Сортировка по возрастанию.
            leafHaffmanTree = SortingLeafList(leafHaffmanTree);
        }

        private Dictionary<string, string> CreatingTreeHaffman()
        {
            while (leafHaffmanTree.Count > 1)
            {
                var left = leafHaffmanTree[0];
                var right = leafHaffmanTree[1];
                leafHaffmanTree.RemoveAt(1);
                leafHaffmanTree.RemoveAt(0);

                double Pi = left.Pi + right.Pi;
                string Name = "P(a_i) = " + Convert.ToString(Pi);

                leafHaffmanTree.Add(new Node(Name, Pi, left, right));
                
                leafHaffmanTree = SortingLeafList(leafHaffmanTree);
            }

            rootTreeHaffman = (Node) leafHaffmanTree[0];
            leafHaffmanTree = null;
            Dictionary<string, string> tableCodes = new Dictionary<string, string>();

            Int32[] widthArray = new Int32[2];

            rootTreeHaffman.searchSymbol(tableCodes, "");

            Console.WriteLine(widthArray[0] + " : " + widthArray[1]);

            return tableCodes;
        }

        private List<Node> SortingLeafList(List<Node> leafList)
        {
            return leafList.OrderBy(Node => Node.Pi).ToList();
        }

        //
        // Вложенный класс для отрисовки кодового дерева Хаффмана в консоль.
        //
        private class PrintTreeHaffman
        {
            public int[,] graphTree = new int[16, 32];

            public PrintTreeHaffman(Leaf rootTree)
            {

            }

            private void FillingGraphTree(Leaf rootTree)
            {

            }
        }
        //
        // Вложенные классы описывающие лист и узел кодового дерева.
        //
        private class Leaf : Node
        {
            public Leaf(string Name, double Pi) : base(Name, Pi, null, null)
            {

            }

            public override void searchSymbol(Dictionary<string, string> tableCodes, string acc)
            {
                tableCodes[Name] = acc;
            }
        }

        private class Node
        {
            public Node leftElement;
            public Node rightElement;
            public string Name;
            public double Pi;

            public Node(string Name, double Pi, Node leftElement, Node rightElement)
            {
                this.Name = Name;
                this.Pi = Pi;
                this.leftElement = leftElement;
                this.rightElement = rightElement;
            }

            public virtual void searchSymbol(Dictionary<string, string> tableCodes, string acc)
            {
                leftElement.searchSymbol(tableCodes, acc + "0");
                rightElement.searchSymbol(tableCodes, acc + "1");
            }
        }
    }
}
