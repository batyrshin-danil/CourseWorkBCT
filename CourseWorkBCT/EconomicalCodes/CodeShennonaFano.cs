using System;
using System.Collections.Generic;
using System.Linq;
using CourseWorkBCT.EconomicalCodes.ElementsTree;

namespace CourseWorkBCT.EconomicalCodes
{
    public class CodeShennonaFano : EconomicalCode
    {                
        public CodeShennonaFano(Dictionary<string, double> probalitiesSymbol) : base(probalitiesSymbol){}
        //
        // Метод создания кодового дерева Шеннона-Фано.
        protected override Dictionary<string, string> CreatingTree()
        {
            return null;
        }
        //
        // Метод деления списка листьев на две примерно равные половины.
        private List<Node>[] DivisionLeafs(List<Node> leafs)
        {
            // Массив хранящий два списка листьев, что были разделены.
            List<Node>[] separatedLeafs = new List<Node>[2];
            // Минимальная разность по модулю между суммами двух списков.
            double minDifferenceAbs = 0;

            for (int leafIndex = 0; leafIndex < leafs.Count; leafIndex++) {

                double sumLeftList = 0, sumRightList = 0;
                /* Считаем сумму вероятностей листьев правого и левого узла. 
                 * Списки листьев формируются исходя из смещения границы левого 
                 * списка на один элемент вправо с каждой новой итерацией главного цикла. */
                for (int pointerLeafs = 0; pointerLeafs < leafs.Count; pointerLeafs++)
                {
                    if (pointerLeafs < leafIndex)
                    {
                        sumLeftList += leafs[pointerLeafs].getPi();  
                        continue;
                    }
                    sumRightList += leafs[pointerLeafs].getPi();                    
                }
                double differenceAbs = Math.Abs(sumLeftList - sumRightList);
                /* Переменная хранящая результат выполнения логического условия
                 * необходимого, но не достаточного для правильного построения кодового дерева. */
                bool equalityLeftAndRightSum = (sumLeftList == sumRightList) || (sumLeftList < sumRightList);
                
                if ((differenceAbs < minDifferenceAbs) && equalityLeftAndRightSum || leafIndex == 0)
                {
                    minDifferenceAbs = differenceAbs;
                    separatedLeafs[0] = leafs.GetRange(0, leafIndex);
                    separatedLeafs[1] = leafs.GetRange(leafIndex, (leafs.Count - leafIndex));
                }
                else if (!equalityLeftAndRightSum)
                {
                    return separatedLeafs;
                }
            }

            return separatedLeafs;
        }        
    }
}