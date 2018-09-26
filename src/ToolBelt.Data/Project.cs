using System;
using System.Collections.Generic;

namespace ToolBelt.Data
{
    public class Project
    {
        public Account Account { get; set; }

        public string AccountId { get; set; }

        public DateTime CreateDate { get; set; } = DateTime.Now;

        public int CrewSize { get; set; }

        public DateTime? DeleteDate { get; set; }

        public string Description { get; set; }

        public bool IsDeleted { get; set; }

        public decimal PayRate { get; set; }

        public int ProjectId { get; set; }

        public List<ProjectTradeSpecialty> ProjectTradeSpecialties { get; } = new List<ProjectTradeSpecialty>();

        public Trade Trade { get; set; }

        public int TradeId { get; set; }
    }
}
