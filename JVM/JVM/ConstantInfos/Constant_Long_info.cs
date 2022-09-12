using System;

namespace JVM.ConstantInfos
{
    [CP_Tag(CP_Tags.CONSTANT_Long)]
    public class Constant_Long_info : CP_Info
    {
        public override CP_Tags Tag => CP_Tags.CONSTANT_Long;
        public uint High_Bytes { get; private set; }
        public uint Low_Bytes { get; private set; }

        public override void Parse(ref ReadOnlySpan<byte> span)
        {
            High_Bytes = span.U4();
            Low_Bytes = span.U4();
        }
    }
}
