using System;

namespace JVM.Attributes
{
    public class Attribute_Info
    {
        public ushort Attribute_Name_Index { get; private set; }
        public uint Attribute_Length { get; private set; }
        public byte[] Info { get; private set; }

        public Attribute_Info(ushort attributeNameIndex, uint attributeLength, ref ReadOnlySpan<byte> span)
        {
            Attribute_Name_Index = attributeNameIndex;
            Attribute_Length = attributeLength;
            Info = new byte[Attribute_Length];
        }
        public void Parse(ref ReadOnlySpan<byte> span)
        {
            for(int i = 0; i < Attribute_Length; i++)
            {
                Info[i] = span.U1();
            }
        }
    }
}
