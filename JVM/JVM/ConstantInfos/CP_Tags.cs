namespace JVM
{
    public enum CP_Tags : byte
    {
        CONSTANT_Class = 07,
        CONSTANT_Fieldref = 09,
        CONSTANT_Methodref = 10,
        CONSTANT_InterfaceMethodref = 11,
        CONSTANT_String = 08,
        CONSTANT_Integer = 03,
        CONSTANT_Float = 04,
        CONSTANT_Long = 05,
        CONSTANT_Double = 06,
        CONSTANT_NameAndType = 12,
        CONSTANT_Utf8 = 01,
        CONSTANT_MethodHandle = 15,
        CONSTANT_MethodType = 16,
        CONSTANT_InvokeDynamic = 18
    }
}
