using System;
using System.Collections.Generic;

namespace ToolBelt.Data
{
    public partial class Project
    {
        public DateTime CreateDate { get; set; } = DateTime.Now;

        public int CrewSize { get; set; }

        public DateTime? DeleteDate { get; set; }

        public string Description { get; set; }

        public bool IsDeleted { get; set; }

        public decimal PayRate { get; set; }

        public int ProjectId { get; set; }
    }
    
    /* NOTE: This part of the Project class simply exists to keep all the Foreign Keys in a single place*/
    public partial class Project
    {
        public Account Account { get; set; }

        public string AccountId { get; set; }

        public Trade Trade { get; set; }

        public int TradeId { get; set; }

        public List<ProjectTradeSpecialty> ProjectTradeSpecialties { get; } = new List<ProjectTradeSpecialty>();
    }
}
