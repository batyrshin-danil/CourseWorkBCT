using System.Collections.Generic;

namespace CourseWorkBCT.EconomicalCodes.ElementsTree
{
    //
    // Класс узла дерева.
    // 
    public class Node
    {
        public Node leftElement;
        public Node rightElement;
        protected string name;
        protected double Pi { get; set; }

        public Node(string Name, double Pi)
        {
            this.name = Name;
            this.Pi = Pi;
        }

        public Node(string Name, double Pi, Node leftElement, Node rightElement)
        {
            this.name = Name;
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

        public string getName()
        {
            return name;
        }
    }
}
