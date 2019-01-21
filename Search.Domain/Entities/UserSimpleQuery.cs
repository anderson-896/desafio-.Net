using Kurier.Acoes.Domain.Entities;
using Kurier.Acoes.Domain.Entities.Brasil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kurier.Analytics.Search.Domain.Entities
{
    public class UserSimpleQuery : SimpleQuery
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
}
