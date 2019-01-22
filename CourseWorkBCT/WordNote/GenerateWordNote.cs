using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;

using Xceed.Words.NET;
using WpfMath;

using CourseWorkBCT.BlocksDS;

namespace CourseWorkBCT.WordNote
{
    class GenerateWordNote
    {
        public string AdressNoteTemplate { get; set; } = @"D:\Users\Batyrshin Danil\Source\Repos\batyrshin-danil\CourseWorkBCT\CourseWorkBCT\Resources\Word\ExplanatoryNoteTemplate.docx";

        private DocX explanatoryNoteTemplate;


        public GenerateWordNote(CourceWorkBCT launcher, string outputFileName)
        {
            explanatoryNoteTemplate = DocX.Load(AdressNoteTemplate);

            InsertValuesMessageSource(launcher.MessageSource);

            StopGeneration(outputFileName);
        }

        private void InsertValuesMessageSource(MessageSource messageSource)
        {
            var speedSourceBookmarks = explanatoryNoteTemplate.Bookmarks["speed_source"];

            var parser = new TexFormulaParser();

            speedSourceBookmarks.Paragraph.AppendEquation("(a + b + c + d) \u2219 10^3 = ");
        }

        private void StopGeneration(string outputFileName)
        {
            explanatoryNoteTemplate.SaveAs(outputFileName);
        }
    }
}
