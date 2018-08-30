using ReactiveUI;
using System;

namespace ToolBelt.Models
{
    // TODO: Replace with the real data...
    public class Project : ReactiveObject
    {
        private DateTime _estimatedEndDate;
        private DateTime _estimatedStartDate;
        private int _id;
        private string _name;

        public DateTime EstimatedEndDate
        {
            get => _estimatedEndDate;
            set => this.RaiseAndSetIfChanged(ref _estimatedEndDate, value);
        }

        public DateTime EstimatedStartDate
        {
            get => _estimatedStartDate;
            set => this.RaiseAndSetIfChanged(ref _estimatedStartDate, value);
        }

        public int Id
        {
            get => _id;
            set => this.RaiseAndSetIfChanged(ref _id, value);
        }

        public string Name
        {
            get => _name;
            set => this.RaiseAndSetIfChanged(ref _name, value);
        }
    }
}
