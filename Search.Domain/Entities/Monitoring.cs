using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kurier.Analytics.Search.Domain.Entities
{
    public class Monitoring
    {
        public int MonitoringId { get; set; }
        public string QueryId { get; set; }
        public int StatusId { get; set; }
        public string StatusDescription { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime AlterDate { get; set; }
        public DateTime CompletedDate { get; set; }
        public long QuantityNewProcesses { get; set; }
        public int UserId { get; set; }
        public int FiltrosQuantidade { get; set; }
    }
}
