using System;

namespace JVM.ConstantInfos
{
    public abstract class CP_Info
    {
        public abstract CP_Tags Tag { get; }
        public abstract void Parse(ref ReadOnlySpan<byte> span);
    }
}