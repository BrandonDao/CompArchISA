using JVM.ConstantInfos;
using System;

namespace JVM
{
    public static class Extensions
    {
        public static uint U4(this ref ReadOnlySpan<byte> span)
        {
            return (uint)(span.U2() << 16 | span.U2());
        }
        public static ushort U2(this ref ReadOnlySpan<byte> span)
        {
            return (ushort)(span.U1() << 8 | span.U1());
        }
        public static byte U1(this ref ReadOnlySpan<byte> span)
        {
            ReadOnlySpan<byte> slicedVal = span.Slice(0, 1);
#pragma warning disable IDE0057 // Use range operator
            span = span.Slice(1);
#pragma warning restore IDE0057

            return slicedVal[0];
        }
        public static string ToUTF8(this CP_Info info)
        {
            return ((Constant_Utf8_Info)info).Value;
        }
    }
}
