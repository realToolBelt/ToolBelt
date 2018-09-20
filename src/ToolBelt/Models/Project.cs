using Newtonsoft.Json;
using ReactiveUI;
using System;

namespace ToolBelt.Models
{
    // NOTE: We want to explicitly set the values here to match what we have in the database
    public enum ProjectStatus
    {
        Open = 1,
        Closed = 2,
        Deleted = 3
    }

    // TODO: Replace with the real data...
    public class Project : ReactiveObject
    {
        private DateTime _createDate;
        private string _description;
        private DateTime _estimatedEndDate;
        private DateTime _estimatedStartDate;
        private int _id;
        private string _name;
        private string _skillsRequired;
        private ProjectStatus _status;
        private WorkPaymentType _paymentType;
        private decimal _payRate;

        public Project()
        {
            CreateDate = DateTime.UtcNow;
            Status = ProjectStatus.Open;
        }

        /// <summary>
        /// Gets or sets the date the project was created.
        /// </summary>
        [JsonProperty("create_date")]
        public DateTime CreateDate
        {
            get => _createDate;
            set => this.RaiseAndSetIfChanged(ref _createDate, value);
        }

        [JsonProperty("description")]
        public string Description
        {
            get => _description;
            set => this.RaiseAndSetIfChanged(ref _description, value);
        }

        [JsonProperty("end_date")]
        public DateTime EstimatedEndDate
        {
            get => _estimatedEndDate;
            set => this.RaiseAndSetIfChanged(ref _estimatedEndDate, value);
        }

        [JsonProperty("start_date")]
        public DateTime EstimatedStartDate
        {
            get => _estimatedStartDate;
            set => this.RaiseAndSetIfChanged(ref _estimatedStartDate, value);
        }

        [JsonProperty("id")]
        public int Id
        {
            get => _id;
            set => this.RaiseAndSetIfChanged(ref _id, value);
        }

        [JsonProperty("name")]
        public string Name
        {
            get => _name;
            set => this.RaiseAndSetIfChanged(ref _name, value);
        }

        public string SkillsRequired
        {
            get => _skillsRequired;
            set => this.RaiseAndSetIfChanged(ref _skillsRequired, value);
        }

        public ProjectStatus Status
        {
            get => _status;
            set => this.RaiseAndSetIfChanged(ref _status, value);
        }

        public WorkPaymentType PaymentType
        {
            get => _paymentType;
            set => this.RaiseAndSetIfChanged(ref _paymentType, value);
        }

        public decimal PaymentRate
        {
            get => _payRate;
            set => this.RaiseAndSetIfChanged(ref _payRate, value);
        }
    }

    public enum WorkPaymentType
    {
        Hourly,
        Piece
    }
}
