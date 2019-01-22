using System.Collections.Generic;

namespace CourseWorkBCT.EconomicalCodes.ElementsTree
{
    //
    // Класс узла дерева.
    // 
    public class Node
    {
        private Node leftElement;
        private Node rightElement;
        protected string Name;
        protected double Pi { get; set; }

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

        public double getPi()
        {
            return Pi;
        }
    }
}
