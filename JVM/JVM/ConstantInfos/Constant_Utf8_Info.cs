using System;
using System.Text;

namespace JVM.ConstantInfos
{
    [CP_Tag(CP_Tags.CONSTANT_Utf8)]
    public class Constant_Utf8_Info : CP_Info
    {
        public override CP_Tags Tag => CP_Tags.CONSTANT_Utf8;
        public ushort Length { get; private set; }
        public byte[] Bytes { get; private set; }
        public string Value { get; private set; }

        public override void Parse(ref ReadOnlySpan<byte> span)
        {
            Length = span.U2();
            Bytes = new byte[Length];

            for (int i = 0; i < Length; i++)
            {
                Bytes[i] = span.U1();
            }

            Value = Encoding.UTF8.GetString(Bytes);
        }
    }
}
