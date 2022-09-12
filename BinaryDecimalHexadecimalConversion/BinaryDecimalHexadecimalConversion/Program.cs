using System;
using System.Text;

namespace BinaryDecimalHexadecimalConversion
{
    class Program
    {
        public static string DecimalToBinary(int number)
        {
            var builder = new StringBuilder();

            do
            {
                builder.Insert(0, number & 0b1);
                number >>= 1;
            } while (number != 0);

            return builder.ToString();
        }
        public static int BinaryToDecimal(string number)
        {
            var output = 0;

            for(int i = 0; i < number.Length; i++)
            {
                output <<= 1;
                output += number[i] - '0';
            }

            return output;
        }
        public static string ToHexadecimal(int number)
        {
            var hexMap = new char[16] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f' };
            var builder = new StringBuilder();

            while (number != 0)
            {
                builder.Insert(0, hexMap[number & 0b1111]);
                number >>= 4;
            }

            return builder.ToString();
        }

        static void Main(string[] args)
        {
            var test = BinaryToDecimal("1010");

            ;
        }
    }
}
