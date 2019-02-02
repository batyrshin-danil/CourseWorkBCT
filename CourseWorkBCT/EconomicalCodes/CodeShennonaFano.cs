using System;
using System.Collections.Generic;
using System.Linq;
using CourseWorkBCT.EconomicalCodes.ElementsTree;

namespace CourseWorkBCT.EconomicalCodes
{
    public class CodeShennonaFano : EconomicalCode
    {
        public CodeShennonaFano(Dictionary<string, double> probalitiesSymbol) : base(probalitiesSymbol) { }
        //
        // Метод создания кодового дерева Шеннона-Фано.
        protected override Dictionary<string, string> CreatingTree()
        {
            rootTree = new Node("n", 2.0);
            AddBranch(rootTree, leavesTree);
            Dictionary<string, string> tableCodes = new Dictionary<string, string>();

            rootTree.searchSymbol(tableCodes, "");

            foreach(string key in tableCodes.Keys)
            {
                Console.WriteLine(key + " : " + tableCodes[key]);
            }

            return tableCodes;
        }
        
        private void AddBranch(Node node, List<Node> leaves)
        {
            List<Node>[] divideLeaves = DivisionLeafs(leaves);
            if (divideLeaves[0].Count == 1 && divideLeaves[1].Count == 1)
            {
                node.leftElement = divideLeaves[0][0];
                node.rightElement = divideLeaves[1][0];
                return;
            }
            else if(divideLeaves[0].Count == 1)
            {
                node.leftElement = divideLeaves[0][0];
                node.rightElement = new Node(
                    CreateNameNode(divideLeaves[1]), CalculationProbabilityNode(divideLeaves[1]));
                AddBranch(node.rightElement, divideLeaves[1]);
                return;
            }
            else if (divideLeaves[1].Count == 1)
            {
                node.rightElement = divideLeaves[1][0];
                node.leftElement = new Node(
                    CreateNameNode(divideLeaves[0]), CalculationProbabilityNode(divideLeaves[0]));
                AddBranch(node.leftElement, divideLeaves[0]);
                return;
            }
            node.leftElement = new Node(
                    CreateNameNode(divideLeaves[0]), CalculationProbabilityNode(divideLeaves[0]));
            node.rightElement = new Node(
                    CreateNameNode(divideLeaves[1]), CalculationProbabilityNode(divideLeaves[1]));
            AddBranch(node.leftElement, divideLeaves[0]);
            AddBranch(node.rightElement, divideLeaves[1]);
        }

        private string CreateNameNode(List<Node> leaves)
        {
            string name = "| ";
            for (int i = 0; i < leaves.Count; i++)
            {
                name += leaves[i].getPi().ToString() + " | ";
            }
            return name;
        }

        private double CalculationProbabilityNode(List<Node> leaves)
        {
            double probability = 0;
            for (int i = 0; i < leaves.Count; i++)
            {
                probability += leaves[i].getPi();
            }
            return probability;
        }
        //
        // Метод деления списка листьев на две примерно равные половины.
        private List<Node>[] DivisionLeafs(List<Node> leaves)
        {
            // Массив хранящий два списка листьев, что были разделены.
            List<Node>[] separatedLeaves = new List<Node>[2];
            // Минимальная разность по модулю между суммами двух списков.
            double minDifferenceAbs = 0;

            for (int leafIndex = 1; leafIndex < leaves.Count; leafIndex++) {

                double sumLeftList = 0, sumRightList = 0;
                /* Считаем сумму вероятностей листьев правого и левого узла. 
                 * Списки листьев формируются исходя из смещения границы левого 
                 * списка на один элемент вправо с каждой новой итерацией главного цикла. */
                for (int pointerLeaves = 0; pointerLeaves < leaves.Count; pointerLeaves++)
                {
                    if (pointerLeaves < leafIndex)
                    {
                        sumLeftList += leaves[pointerLeaves].getPi();  
                        continue;
                    }
                    sumRightList += leaves[pointerLeaves].getPi();                    
                }
                double differenceAbs = Math.Abs(sumLeftList - sumRightList);
                /* Переменная хранящая результат выполнения логического условия
                 * необходимого, но не достаточного для правильного построения кодового дерева. */
                bool equalityLeftAndRightSum = (sumLeftList == sumRightList) || (sumLeftList < sumRightList);          
                
                if ((differenceAbs < minDifferenceAbs) && equalityLeftAndRightSum || leafIndex == 1)
                {
                    minDifferenceAbs = differenceAbs;
                }
                else
                {
                    continue;
                }
                separatedLeaves[0] = leaves.GetRange(0, leafIndex);
                separatedLeaves[1] = leaves.GetRange(leafIndex, (leaves.Count - leafIndex));
            }

            return separatedLeaves;
        }        
    }
}