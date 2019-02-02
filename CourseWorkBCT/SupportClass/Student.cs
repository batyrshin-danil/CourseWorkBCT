using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWorkBCT.SupportClass
{
    public class Student
    {
        private string numberGroup;
        private string teacherName;

        public string FullName { get; private set; }
        public string LastNumberYearAdmission { get; private set; }

        public int[] VariableNumber { get; private set; }

        public Student(string firstName, string lastName, string middleName,
            string numberGroup, string recordBookNumber, string teacherName)
        {
            FullName = CreatedFullName(firstName, lastName, middleName);
            NumberGroup = numberGroup;
            VariableNumber = CreatedVariableNumber(recordBookNumber);
            TeacherName = teacherName;
        }

        public string TeacherName
        {
            get
            {
                return TeacherName;
            }
            private set
            {
                value.ToLower();
                value[0].ToString().ToUpper();
                value[value.Length - 1].ToString().ToUpper();
                value[value.Length - 3].ToString().ToUpper();

                teacherName = value;
            }
        }

        public string NumberGroup
        {
            get
            {
                return NumberGroup;
            }
            private set
            {
                numberGroup = value;
                LastNumberYearAdmission = numberGroup[numberGroup.Length - 2].ToString();
            }
        }

        private string CreatedFullName(string firstName, string lastName, string middleName)
        {
            lastName.ToLower();
            lastName[0].ToString().ToUpper();
            firstName[0].ToString().ToUpper();
            middleName[0].ToString().ToUpper();

            return lastName + firstName[0] + "." + middleName[0] + ".";
        }

        private int[] CreatedVariableNumber(string recordBookNumber)
        {
            int[] variable = new int[4];

            string variableString = LastNumberYearAdmission + recordBookNumber.Substring(3);

            for (int i = 0; i < 4; i++)
            {
                variable[i] = Convert.ToInt32(variableString[i].ToString());
            }

            return variable;
        }
    }
}
