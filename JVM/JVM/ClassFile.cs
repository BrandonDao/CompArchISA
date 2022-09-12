using JVM.Attributes;
using JVM.ConstantInfos;
using JVM.Fields;
using JVM.Methods;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace JVM
{
    public class ClassFile
    {
        #region Properties
        private byte[] OriginalMachineCode { get; set; }

        public uint Magic => 0xCAFEBABE;
        public ushort Minor_Version { get; private set; }
        public ushort Major_Version { get; private set; }

        public ushort Constant_Pool_Count { get; private set; }
        public CP_Info[] Constant_Pool { get; private set; }

        public ushort Access_Flags { get; private set; }
        public ushort This_Class { get; private set; }
        public ushort Super_Class { get; private set; }

        public ushort Interfaces_Count { get; private set; }
        public ushort[] Interfaces { get; private set; }

        public ushort Fields_Count { get; private set; }
        public Field_Info[] Fields { get; private set; }

        public ushort Methods_Count { get; private set; }
        public Method_Info[] Methods { get; private set; }

        public ushort Attributes_Count { get; private set; }
        public Attribute_Info[] Attributes { get; private set; }
        #endregion Properties

        public ClassFile(byte[] machineCode)
        {
            OriginalMachineCode = machineCode;

            var span = new ReadOnlySpan<byte>(machineCode);

            if (span.U4() != 0xCAFEBABE) throw new ArgumentException("Not a valid Java bytecode file!");

            Minor_Version = span.U2();
            Major_Version = span.U2();

            Constant_Pool_Count = span.U2();
            Constant_Pool = new CP_Info[Constant_Pool_Count - 1];
            ParseConstantPool(ref span);

            Access_Flags = span.U2();
            This_Class = span.U2();
            Super_Class = span.U2();

            Interfaces_Count = span.U2();
            Interfaces = new ushort[Interfaces_Count];
            ParseInterfaces(ref span);

            Fields_Count = span.U2();
            Fields = new Field_Info[Fields_Count];
            ParseFields(ref span);

            Methods_Count = span.U2();
            Methods = new Method_Info[Methods_Count];
            ParseMethods(ref span);

            Attributes_Count = span.U2();
            Attributes = new Attribute_Info[Attributes_Count];
            ParseAttributes(Attributes_Count, Constant_Pool, Attributes, ref span);

            if (span.Length != 0) throw new Exception("Either invalid Java bytecode file or my code sucks!");
        }

        private void ParseConstantPool(ref ReadOnlySpan<byte> span)
        {
            var TagToCPInfo = new Dictionary<CP_Tags, Func<CP_Info>>();

            Type CPInfoType = typeof(CP_Info);
            Type[] allTypes = Assembly.GetAssembly(CPInfoType).GetTypes();

            IEnumerable<Type> CPInfos = allTypes.Where(
                type => type.IsSubclassOf(CPInfoType)
             && type.GetCustomAttribute(typeof(CP_TagAttribute)) != null);

            foreach (Type type in CPInfos)
            {
                TagToCPInfo.Add(
                    type.GetCustomAttribute<CP_TagAttribute>().Tag,
                    () => (CP_Info)Activator.CreateInstance(type));
            }

            for (int i = 0; i < Constant_Pool.Length; i++)
            {
                CP_Tags tag = (CP_Tags)span.U1();

                CP_Info cpInfo = TagToCPInfo[tag].Invoke();
                cpInfo.Parse(ref span);

                Constant_Pool[i] = cpInfo;
                if (tag == CP_Tags.CONSTANT_Long) i++;
            }
        }
        private void ParseInterfaces(ref ReadOnlySpan<byte> span)
        {
            for (int i = 0; i < Interfaces_Count; i++)
            {
                Interfaces_Count = span.U2();
            }
        }
        private void ParseFields(ref ReadOnlySpan<byte> span)
        {
            for (int i = 0; i < Fields_Count; i++)
            {
                Fields[i] = new Field_Info(ref span, Constant_Pool);
            }
        }
        private void ParseMethods(ref ReadOnlySpan<byte> span)
        {
            for (int i = 0; i < Methods_Count; i++)
            {
                Methods[i] = new Method_Info(ref span, Constant_Pool);
            }
        }
        public static void ParseAttributes(ushort attributesCount, CP_Info[] ConstantPool, Attribute_Info[] attributes, ref ReadOnlySpan<byte> span)
        {
            for (int i = 0; i < attributesCount; i++)
            {
                var attributeNameIndex = span.U2();

                if (ConstantPool[attributeNameIndex - 1].ToUTF8() == "Code")
                {
                    attributes[i] = new Code_Attribute_Info(attributeNameIndex, attributeLength: span.U4(), ConstantPool, ref span);
                }
                else
                {
                    attributes[i] = new Attribute_Info(attributeNameIndex, attributeLength: span.U4(), ref span);
                    attributes[i].Parse(ref span);
                }
            }
        }

        public Method_Info GetMethod(string name, string description)
            => Methods
                .Where(method => method.Access_Flags == (MethodAccessFlags.ACC_PUBLIC | MethodAccessFlags.ACC_STATIC))
                .Where(method => Constant_Pool[method.Name_Index - 1].ToUTF8() == name
                    && Constant_Pool[method.Descriptor_Index - 1].ToUTF8() == description)
                .FirstOrDefault();

        public Attribute_Info GetAttribute(Method_Info method, string attributeName)
            => method.Attributes
                .Where(attribute => Constant_Pool[attribute.Attribute_Name_Index - 1].ToUTF8() == attributeName)
                .FirstOrDefault();

        public void DebugPrintInfo()
        {
            Console.WriteLine($"Magic: 0x{Magic:X4}");
            Console.WriteLine($"Minor_Version: {Minor_Version}");
            Console.WriteLine($"Major_Version: {Major_Version}");
            Console.WriteLine($"\nAccess Flags: 0x{Access_Flags:X2}");
            Console.WriteLine($"This_Class: {This_Class}");
            Console.WriteLine($"Super_Class: {Super_Class}");

            Console.WriteLine($"\nConstant_Pool_Count: {Constant_Pool_Count}");
            Console.WriteLine("Constant_Pool:");
            foreach (var cpInfo in Constant_Pool)
            {
#pragma warning disable IDE0057 // Use range operator
                Console.WriteLine($"    {cpInfo.GetType().Name.Substring(9)}");
#pragma warning restore IDE0057 // Use range operator
            }

            Console.WriteLine($"\nInterfaces_Count: {Interfaces_Count:X2}");
            Console.WriteLine($"Fields_Count: {Fields_Count:X2}");
            Console.WriteLine($"Methods_Count: {Methods_Count:X2}");
            Console.WriteLine($"Attributes_Count: {Attributes_Count:X2}");
        }
    }
}