using System;

namespace JVM.ConstantInfos
{
    [CP_Tag(CP_Tags.CONSTANT_Class)]
    class Constant_Class_Info : CP_Info
    {
        public override CP_Tags Tag => CP_Tags.CONSTANT_Class;
        public ushort Name_Index { get; private set; }

        public override void Parse(ref ReadOnlySpan<byte> span)
        {
            Name_Index = span.U2();
        }
    }
}
