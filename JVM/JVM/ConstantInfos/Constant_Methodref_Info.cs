using System;

namespace JVM.ConstantInfos
{
    [CP_Tag(CP_Tags.CONSTANT_Methodref)]
    public class Constant_Methodref_Info : CP_Info
    {
        public override CP_Tags Tag => CP_Tags.CONSTANT_Methodref;
        public ushort Class_Index { get; private set; }
        public ushort Name_And_Type_Index { get; private set; }

        public override void Parse(ref ReadOnlySpan<byte> span)
        {
            Class_Index = span.U2();
            Name_And_Type_Index = span.U2();
        }
    }
}
