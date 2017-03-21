using System.Collections.Generic;

namespace CleanKludge.Core.Sections
{
    public class CodeSection : ISection
    {
        public SectionType Type => SectionType.Code;
        public CodeLanguage Language { get; set; }
        public List<string> Content { get; set; }
    }
}