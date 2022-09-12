using BitshiftingPractice.Instructions;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace BitshiftingPractice
{
    class Program
    {


        #region stuff
        static int RotateLeft(int number)
        {
            var outBit = number & (1 << 31);

            number <<= 1;

            return number |= outBit << 31;
        }
        static int RotateRight(int number)
        {
            var outBit = number & 1;

            number >>= 1;

            return number |= outBit << 31;
        }

        static bool IsNthBitOne(int number, int bitIndex)
        {
            return (number & (1 << bitIndex)) == 1;
        }
        static byte GetNthByte(uint number, int byteIndex)
        {
            return (byte)((number & (255 << (byteIndex * 8))) >> (byteIndex * 8));
        }

        static int Subtract(int subtrahend, int minuend)
        {
            var onesCompliment = ~subtrahend;
            var twosCompliment = onesCompliment + 1;

            return minuend + twosCompliment;
        }

        public class Node
        {
            public byte Value { get; set; }
            public Node Next { get; set; }

            public Node(byte value, Node next = null)
            {
                Value = value;
                Next = next;
            }
        }
        static Node Add(Node headA, Node headB, byte carryIn = 0)
        {
            if (headA == null && headB == null) return null;

            var a = headA == null ? 0 : headA.Value;
            var b = headB == null ? 0 : headB.Value;

            var sum = (byte)(a ^ b ^ carryIn);
            var carryOut = (byte)((carryIn & (a | b)) | (a & b));

            var node = new Node(sum, Add(headA?.Next, headB?.Next, carryOut));
            return node;
        }
        static LinkedList<byte> Add(LinkedListNode<byte> headA, LinkedListNode<byte> headB)
        {
            int a = 0;
            for (LinkedListNode<byte> curr = headA; curr != null; curr = curr.Next)
            {
                a <<= 1;
                a += curr.Value;
            }

            int b = 0;
            for (LinkedListNode<byte> curr = headB; curr != null; curr = curr.Next)
            {
                b <<= 1;
                b += curr.Value;
            }

            int c = a + b;
            var newList = new LinkedList<byte>();
            do
            {
                newList.AddFirst((byte)(c & 0b1));
                c >>= 1;
            } while (c != 0);

            return newList;
        }

        static unsafe void LoopThrough(int[] array, int start, int end)
        {
            fixed (int* ptr = array)
            {
                var i = start;
                while (i < end)
                {
                    Console.WriteLine(ptr[i]);
                    i++;
                }
            }
        }

        static unsafe void BubbleSort(int[] array, int start, int end)
        {
            fixed (int* ptr = array)
            {
                var i = start;

                while (i < end)
                {
                    var x = i;
                    while (x < end)
                    {
                        if (ptr[i] > ptr[x])
                        {
                            var temp = ptr[i];
                            ptr[i] = ptr[x];
                            ptr[x] = temp;
                        }
                        x++;
                    }
                    i++;
                }
            }
        }
        static void Print(Node head)
        {
            Console.WriteLine("");

            var curr = head;
            while (curr != null)
            {
                Console.Write($"{curr.Value}->");
                curr = curr.Next;
            }
            Console.Write("null");
        }
        #endregion stuff

        static readonly short[] Registers = new short[32];
        static readonly uint[] RAM = new uint[8];
        const int RIP = 30;

        //"(SET) R([1-2][0-9] |(?:[0-9]) )(\\d+)"

        public static string[] LoadAssemblyProgram()
        {
            return System.IO.File.ReadAllLines("Program.asm");
        }

        static void FetchDecodeAndExecute()
        {
            if (Registers[RIP] >= RAM.Length) return;

            uint instruction = RAM[Registers[RIP]];
            byte opCode = GetNthByte(instruction, 3);

            switch (opCode)
            {
                case (byte)OpCodes.ADD:
                    {
                        byte RA = GetNthByte(instruction, 1);
                        byte RB = GetNthByte(instruction, 0);
                        byte RC = GetNthByte(instruction, 2);

                        Registers[RC] = (short)(Registers[RA] + Registers[RB]);
                        break;
                    }

                case (byte)OpCodes.LT:
                    {
                        byte RDest = GetNthByte(instruction, 2);
                        byte RA = GetNthByte(instruction, 1);
                        byte RB = GetNthByte(instruction, 0);

                        Registers[RDest] = (short)(Registers[RA] < Registers[RB] ? 1 : 0);
                        break;
                    }

                case (byte)OpCodes.JMP:
                    {
                        Registers[RIP] = GetNthByte(instruction, 0);
                        return;
                    }

                case (byte)OpCodes.JMPT:
                    {
                        byte RCondition = GetNthByte(instruction, 2);
                        if (Registers[RCondition] == 1)
                        {
                            Registers[RIP] = GetNthByte(instruction, 0);
                        }
                        return;
                    }

                case (byte)OpCodes.JMPF:
                    {
                        byte RCondition = GetNthByte(instruction, 2);
                        if (Registers[RCondition] == 0)
                        {
                            Registers[RIP] = GetNthByte(instruction, 0);
                        }
                        return;
                    }

                case (byte)OpCodes.SET:
                    {
                        byte RA = GetNthByte(instruction, 2);
                        short value = (short)(GetNthByte(instruction, 0) | (GetNthByte(instruction, 1) << 2));

                        Registers[RA] = value;
                        break;
                    }
            }
            Registers[RIP]++;
        }

        static void Main(string[] args)
        {
            string[] instructions = LoadAssemblyProgram();

            var instruction = Instruction.Parse(instructions[1]);
            var instructionStr = instruction.Disassemble();

            ;

            for (int i = 0; i < instructions.Length; i++)
            {
                RAM[i] = Instruction.Parse(instructions[i]).MachineCode;
            }

            while (true)
            {
                FetchDecodeAndExecute();
            }
            ;
        }
    }
}
