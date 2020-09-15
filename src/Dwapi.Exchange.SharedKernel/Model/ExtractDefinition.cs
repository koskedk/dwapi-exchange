using System;

namespace Dwapi.Exchange.SharedKernel.Model
{
    public abstract class ExtractDefinition : Entity<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string SqlScript { get; set; }
        public long RecordCount { get; set; }
        public DateTime Updated { get; set; }
        public DateTime Refreshed { get; set; }

        public override string ToString()
        {
            return $"{Name}";
        }
    }
}
