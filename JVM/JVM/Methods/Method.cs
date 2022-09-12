using JVM.Attributes;
using JVM.ConstantInfos;
using System;
using System.Collections.Generic;
using System.Text;

namespace JVM.Methods
{
    public class Method
    {
        private enum Opcodes : byte
        {
            iconst_5 = 0x08,
            bipush = 0x10,
            ldc2_w = 0x14,
            iload_0 = 0x1A,
            iload_1 = 0x1B,
            iload_2 = 0x1C,
            lload_0 = 0x1E,
            lload_2 = 0x20,
            lstore = 0x37,
            pop = 0x57,
            iadd = 0x60,
            ladd = 0x61,
            istore_1 = 0x3C,
            istore_2 = 0x3D,
            istore_3 = 0x3E,
            ireturn = 0xAC,
            @return = 0xB1,
            invokestatic = 0xB8
        }

        public Method_Info Method_Info { get; private set; }
        public Code_Attribute_Info Code_Attribute_Info{ get; private set; }
        public ClassFile ClassFile { get; private set; }

        //public Constant_Utf8_Info[] Parameters { get; private set; } // todo later
        public uint[] Locals { get; private set; }

        public Method(Method_Info method_Info, ClassFile classFile)
        {
            Method_Info = method_Info;
            Code_Attribute_Info = (Code_Attribute_Info)classFile.GetAttribute(method_Info, "Code");
            ClassFile = classFile;
            Locals = new uint[Code_Attribute_Info.Max_Locals];
        }

        public void Execute()
        {
            ReadOnlySpan<byte> code = Code_Attribute_Info.Code;

            while(true)
            {
                Opcodes opcode = (Opcodes)code.U1();

                switch (opcode)
                {
                    #region iconsts
                    case Opcodes.iconst_5:
                        Program.Stack.Push(5);
                        break;
                    #endregion iconsts

                    case Opcodes.bipush:
                        Program.Stack.Push(code.U1());
                        break;

                    case Opcodes.ldc2_w:
                        {
                            var index = code.U2();
                            uint hint = (uint)((Constant_Long_info)ClassFile.Constant_Pool[index - 1]).High_Bytes;
                            uint lint = (uint)((Constant_Long_info)ClassFile.Constant_Pool[index - 1]).Low_Bytes;

                            Program.Stack.Push(hint);
                            Program.Stack.Push(lint);
                            break;
                        }

                    #region load
                    case Opcodes.iload_1:
                        Program.Stack.Push(Locals[1]);
                        break;

                    case Opcodes.iload_2:
                        Program.Stack.Push(Locals[2]);
                        break;

                    case Opcodes.iload_0:
                        Program.Stack.Push(Locals[0]);
                        break;

                    case Opcodes.lload_0:
                        Program.Stack.Push(Locals[0]);
                        Program.Stack.Push(Locals[1]);
                        break;

                    case Opcodes.lload_2:
                        Program.Stack.Push(Locals[2]);
                        Program.Stack.Push(Locals[3]);
                        break;
                    #endregion load

                    case Opcodes.pop:
                        Program.Stack.Pop();
                        break;

                    #region add
                    case Opcodes.iadd:
                        Program.Stack.Push(Program.Stack.Pop() + Program.Stack.Pop());
                        break;

                    case Opcodes.ladd:
                        long result = Program.Stack.Pop() + Program.Stack.Pop() << 32;
                        Program.Stack.Push((uint)((result & 0xFFFF0000) >> 32));
                        Program.Stack.Push((uint)(result  & 0x0000FFFF));
                        break;
                    #endregion add

                    #region store
                    case Opcodes.istore_1:
                        Locals[1] = Program.Stack.Pop();
                        break;

                    case Opcodes.istore_2:
                        Locals[2] = Program.Stack.Pop();
                        break;

                    case Opcodes.istore_3:
                        Locals[3] = Program.Stack.Pop();
                        break;

                    case Opcodes.lstore:
                        {
                            var index = code.U1();
                            Locals[index + 1] = Program.Stack.Pop();
                            Locals[index] = Program.Stack.Pop();
                            break;
                        }
                    #endregion store

                    #region return
                    case Opcodes.ireturn:
                        return;

                    case Opcodes.@return:
                        if (Program.Stack.Count != 0) throw new Exception("bad code");
                        return;
                    #endregion return

                    case Opcodes.invokestatic:
                        {
                            Constant_Methodref_Info methodref = (Constant_Methodref_Info)ClassFile.Constant_Pool[((code.U1() << 8) | code.U1()) - 1];
                            Constant_NameAndType_Info nameAndTypeInfo = (Constant_NameAndType_Info)ClassFile.Constant_Pool[methodref.Name_And_Type_Index - 1];
                            string name = ClassFile.Constant_Pool[nameAndTypeInfo.Name_Index - 1].ToUTF8();
                            string description = ClassFile.Constant_Pool[nameAndTypeInfo.Descriptor_Index - 1].ToUTF8();

                            Method method = new Method(ClassFile.GetMethod(name, description), ClassFile);

                            for (int i = 0; Program.Stack.Count > 0; i++)
                            {
                                method.Locals[i] = Program.Stack.Pop();
                            }

                            method.Execute();
                            break;
                        }

                    default:
                        throw new NotImplementedException($"TODO: Implement opcode 0x{(byte)opcode:X2}");
                }
            }
        }
    }
}
