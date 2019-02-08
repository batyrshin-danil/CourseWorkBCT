using System;
using System.Drawing;

using CourseWorkBCT.WordNote;
using CourseWorkBCT.SupportClass;
using CourseWorkBCT.BlocksDS;

using ZedGraph;

namespace CourseWorkBCT
{
    class Program
    {
        static void Main()
        {

            Student student = new Student("Данил", "Батыршин", "Русланович", "ИКТр-62", "160526", "Хабаров Е.О.");

            CourceWorkBCT courceWorkBCT = new CourceWorkBCT(student);

            GraphicsModulator.DrawGraphics(courceWorkBCT.Modulator);

            Console.ReadLine();
        }
       
    }
}
