using System;

namespace JVM
{
    public class Exception_Table
    {
        public ushort Start_Pc { get; private set; }
        public ushort End_Pc { get; private set; }
        public ushort Handler_Pc { get; private set; }
        public ushort Catch_Type { get; private set; }

        public Exception_Table(ref ReadOnlySpan<byte> span)
        {
            Start_Pc = span.U2();
            End_Pc = span.U2();
            Handler_Pc = span.U2();
            Catch_Type = span.U2();
        }
    }
}
