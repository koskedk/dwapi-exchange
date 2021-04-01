namespace Dwapi.Exchange.SharedKernel.Model
{
    public class ExtractBlock
    {
        public long First { get; }
        public long Last { get; }

        public ExtractBlock(long first, long last)
        {
            First = first;
            Last = last;
        }

        public override string ToString()
        {
            return $"BETWEEN {First} AND {Last}";
        }
    }
}
