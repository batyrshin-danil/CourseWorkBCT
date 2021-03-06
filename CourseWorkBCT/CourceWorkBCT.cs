﻿using System;

using CourseWorkBCT.BlocksDS;
using CourseWorkBCT.SupportClass;


namespace CourseWorkBCT
{
    public class CourceWorkBCT
    {
        public MessageSource MessageSource { get; private set; }
        public SourceCoder SourceCoder { get; private set; }
        public ChannelEncoder ChannelEncoder { get; private set; }
        public Modulator Modulator { get; private set; }
        public CommunicationChannel CommunicationChannel { get; private set; }
        public Demodulator Demodulator { get; private set; }
        public ChannelDecoder ChannelDecoder { get; private set; }
        public SourceDecoder SourceDecoder { get; private set; } 

        public Student Student { get; private set; }

        public CourceWorkBCT(Student student)
        {
            MessageSource = new MessageSource(student.VariableNumber);
            SourceCoder = new SourceCoder(MessageSource);
            ChannelEncoder = new ChannelEncoder(SourceCoder);
            Modulator = new Modulator(SourceCoder, ChannelEncoder);
            CommunicationChannel = new CommunicationChannel(MessageSource, Modulator);
            Demodulator = new Demodulator(ChannelEncoder, Modulator, CommunicationChannel);
            ChannelDecoder = new ChannelDecoder(ChannelEncoder, Demodulator);
            SourceDecoder = new SourceDecoder(SourceCoder, ChannelDecoder);
        }
    }
}
