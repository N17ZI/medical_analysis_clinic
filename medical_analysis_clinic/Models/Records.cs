using System;

namespace medical_analysis_clinic.Models
{
    internal class Records
    {
        public Records(string recordsName,string date)
        {
            RecordsName = recordsName;
            Date = date;
        }

        public string RecordsName { get; set; }
        public string Date { get; set; }
        public override string ToString()
        {
            return this.RecordsName + " " + this.Date;
        }
    }
}
