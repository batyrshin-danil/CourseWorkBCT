using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CourseWorkBCT.ElementsTree;

namespace CourseWorkBCT.BlocksDS
{
    class CodeShennonaFano
    {
        private List<Node> leafShennonaFanoTree = new List<Node>();
        private Node rootTreeShennonaFano;
        //
        // Словарь для хранения пар "символ:код".
        private Dictionary<string, string> tableCodes;

        public CodeShennonaFano(Dictionary<string, double> probalitiesSymbol)
        {
            CreatingListLeaf(probalitiesSymbol);
        }

        public Dictionary<string, string> getTableCodes()
        {
            return tableCodes;
        }
        //
        // Создаем список листьев кодового дерева Хаффмана и сортируем его по возрастания слева-направо.
        private void CreatingListLeaf(Dictionary<string, double> probalitiesSymbol)
        {
            foreach (string key in probalitiesSymbol.Keys)
            {
                leafShennonaFanoTree.Add(new Leaf(key, probalitiesSymbol[key]));
            }
            //
            // Сортировка списка листьев по возрастанию.
            leafShennonaFanoTree = SortingLeafList(leafShennonaFanoTree);
        }
        //
        // Метод создания кодового дерева Шеннона-Фано.
        private Dictionary<string, string> CreatingTreeShennonaFano()
        {
            while (true)
            {
                List<Node>[] twoHalvesListLeaf = DivisionLeafList(leafShennonaFanoTree);

                var left = leafShennonaFanoTree[0];
                var right = leafShennonaFanoTree[1];
                leafShennonaFanoTree.RemoveAt(1);
                leafShennonaFanoTree.RemoveAt(0);

                double Pi = left.getPi() + right.getPi();
                string Name = Convert.ToString(Pi);

                leafShennonaFanoTree.Add(new Node(Name, Pi, left, right));

                leafShennonaFanoTree = SortingLeafList(leafShennonaFanoTree);
            }

            rootTreeShennonaFano = (Node)leafShennonaFanoTree[0];
            leafShennonaFanoTree = null;
            Dictionary<string, string> tableCodes = new Dictionary<string, string>();

            rootTreeShennonaFano.searchSymbol(tableCodes, "");

            return tableCodes;
        }
        //
        // Метод деления списка листьев на две примерно равные половины.
        private List<Node>[] DivisionLeafList(List<Node> leafList)
        {
            // Массив хранящий два списка листьев, что были разделены.
            List<Node>[] listLeafList = new List<Node>[2];
            // Минимальная разность по модулю между суммами двух списков.
            double minDifferenceAbs = 0;

            for (int indexLeafList = 0; indexLeafList < leafList.Count; indexLeafList++) {

                double sumLeftList = 0, sumRightList = 0;
                /* Считаем сумму вероятностей листьев правого и левого узла. 
                 * Списки листьев формируются исходя из смещения границы левого 
                 * списка на один элемент вправо с каждой новой итерацией главного цикла. */
                for (int indexLeaf = 0; indexLeaf < leafList.Count; indexLeaf++)
                {
                    if (indexLeaf < indexLeafList)
                    {
                        sumLeftList += leafList[indexLeaf].getPi();  
                        continue;
                    }
                    sumRightList += leafList[indexLeaf].getPi();                    
                }
                double differenceAbs = Math.Abs(sumLeftList - sumRightList);
                /* Переменная хранящая результат выполнения логического условия
                 * необходимого, но не достаточного для правильного построения кодового дерева. */
                bool equalityLeftAndRightSum = (sumLeftList == sumRightList) || (sumLeftList < sumRightList);
                
                if ((differenceAbs < minDifferenceAbs) && equalityLeftAndRightSum || indexLeafList == 0)
                {
                    minDifferenceAbs = differenceAbs;
                    listLeafList[0] = leafList.GetRange(0, indexLeafList);
                    listLeafList[1] = leafList.GetRange(indexLeafList, (leafList.Count - indexLeafList));
                }
                else if (!equalityLeftAndRightSum)
                {
                    return listLeafList;
                }
            }

            return listLeafList;
        }
        //
        // Метод реализующий сортировку списка листьев по возрастанию.
        private List<Node> SortingLeafList(List<Node> leafList)
        {
            return leafList.OrderBy(Node => Node.getPi()).ToList();
        }
    }
}
