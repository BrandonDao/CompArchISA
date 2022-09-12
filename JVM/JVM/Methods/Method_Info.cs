﻿using JVM.Attributes;
using JVM.ConstantInfos;
using System;

namespace JVM.Methods
{
    public class Method_Info
    {
        public MethodAccessFlags Access_Flags { get; private set; }
        public ushort Name_Index { get; private set; }
        public ushort Descriptor_Index { get; private set; }
        public ushort Attributes_Count { get; private set; }
        public Attribute_Info[] Attributes { get; private set; }

        public Method_Info(ref ReadOnlySpan<byte> span, CP_Info[] Constant_Pool)
        {
            Access_Flags = (MethodAccessFlags)span.U2();
            Name_Index = span.U2();
            Descriptor_Index = span.U2();
            Attributes_Count = span.U2();
            Attributes = new Attribute_Info[Attributes_Count];
            ClassFile.ParseAttributes(Attributes_Count, Constant_Pool, Attributes, ref span);
        }
    }
}
