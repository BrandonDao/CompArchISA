using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace SharedLibrary.Layouts
{
    public interface ILayout
    {
        public string RegexPattern { get; }
        public byte[] Parse(Match match);
    }
}
