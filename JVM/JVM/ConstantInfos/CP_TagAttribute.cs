using System;

namespace JVM.ConstantInfos
{
    public class CP_TagAttribute : Attribute
    {
        public CP_Tags Tag { get; }
        public CP_TagAttribute(CP_Tags tag)
        {
            Tag = tag;
        }
    }
}
