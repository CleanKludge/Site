namespace CleanKludge.Core.Sections
{
    public class TitleSection : ISection
    {
        public SectionType Type => SectionType.Title;
        public string Content { get; set; }
    }
}