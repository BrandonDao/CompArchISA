using System;

namespace JVM.ConstantInfos
{
    [CP_Tag(CP_Tags.CONSTANT_NameAndType)]
    class Constant_NameAndType_Info : CP_Info
    {
        public override CP_Tags Tag => CP_Tags.CONSTANT_NameAndType;
        public ushort Name_Index { get; private set; }
        public ushort Descriptor_Index { get; private set; }

        public override void Parse(ref ReadOnlySpan<byte> span)
        {
            Name_Index = span.U2();
            Descriptor_Index = span.U2();
        }
    }
}
