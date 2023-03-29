using JVM.Attributes;
using JVM.Methods;
using System.Collections.Generic;

namespace JVM
{
    class Program
    {
        public static Stack<uint> Stack = new Stack<uint>();

        static byte[] LoadAssemblyFile()
        {
            return System.IO.File.ReadAllBytes(@"C:\Users\brand\Documents\Github\CompArchISA\JavaFiles\Program.class");
        }

        static void Main(string[] args)
        {
            var machineCode = LoadAssemblyFile();

            var classFile = new ClassFile(machineCode);
            Method_Info main = classFile.GetMethod("main", "([Ljava/lang/String;)V");
            Code_Attribute_Info code = (Code_Attribute_Info)classFile.GetAttribute(main, "Code");
            
            var mainMaybe = new Method(main, classFile);
            mainMaybe.Execute();

            classFile.DebugPrintInfo();
        }
    }
}