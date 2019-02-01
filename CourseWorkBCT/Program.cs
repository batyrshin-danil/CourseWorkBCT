using System;

using CourseWorkBCT.WordNote;
using CourseWorkBCT.SupportClass;

namespace CourseWorkBCT
{
    class Program
    {
        static void Main()
        {
            Student student = new Student("Данил", "Батыршин", "Русланович", "ИКТр-62", "160526", "Хабаров Е.О.");

            CourceWorkBCT courceWorkBCT = new CourceWorkBCT(student);

            Console.ReadLine();
        }
    }
}
