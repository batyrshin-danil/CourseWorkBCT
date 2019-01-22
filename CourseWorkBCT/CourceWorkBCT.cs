using System;

using CourseWorkBCT.BlocksDS;
using CourseWorkBCT.SupportClass;


namespace CourseWorkBCT
{
    public class CourceWorkBCT
    {
        // Блок источника сообщений.
        public MessageSource MessageSource { get; private set; }
        // Блок кодера источника.
        public SourceCoder SourceCoder { get; private set; }
        // Блок кодера канала.
        public ChannelEncoder ChannelEncoder { get; private set; }
        // Блок модулятора.
        public Modulator Modulator { get; private set; }

        public Student Student { get; private set; }

        public CourceWorkBCT(Student student)
        {
            MessageSource = new MessageSource(student.VariableNumber);
            SourceCoder = new SourceCoder(MessageSource);
            ChannelEncoder = new ChannelEncoder(SourceCoder);
            Modulator = new Modulator(SourceCoder, ChannelEncoder);
        }
    }
}
