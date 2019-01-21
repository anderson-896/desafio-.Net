using Kurier.Analytics.Acoes.Domain.Entities;
using Kurier.Analytics.Acoes.Domain.Entities.Brasil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kurier.Analytics.Search.Domain.Entities
{
    public class UserSimpleQueryOld : SimpleQueryOld
    {
        public string QueryId { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public int QueryType { get; set; } // 1 - Salva, 2 - Automática
        public int SearchTemplateId { get; set; }
        public string ActionModule { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifyDate { get; set; }
        public bool IsMonitored { get; set; }
        public bool IsActive { get; set; }
        public string QueryJsonDescription { get; set; }
        public int TotalResult { get; set; }
    }

    public class SimpleQueryOld
    {
        public string Term { get; set; }
        public int Size { get; set; }
        public int From { get; set; }
        public string BaseIndice { get; set; }
        public string[] IndiceYears { get; set; }
        public ICollection<TermFilter> TermFilters { get; set; } = new List<TermFilter>();
        public ICollection<TermsFilterOld> TermsFilters { get; set; } = new List<TermsFilterOld>();
        public ICollection<DateRangeFilter> DateRangeFilters { get; set; } = new List<DateRangeFilter>();
        public ICollection<NumericRangeFilter> NumericRangeFilters { get; set; } = new List<NumericRangeFilter>();
        public ICollection<AggInfo> Aggregations { get; set; } = new List<AggInfo>();
        public ICollection<SearchField> Fields { get; set; } = new List<SearchField>();
    }

    public class TermsFilterOld
    {
        public string Field { get; set; }

        public IEnumerable<string> Terms { get; set; }
    }
}
