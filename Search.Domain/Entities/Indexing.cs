using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kurier.Analytics.Search.Domain.Entities
{
    public class Indexing
    {
        public int IndexacaoId { get; set; }
        public string PesquisaId { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? DataAtualizacao { get; set; }
        public DateTime? DataIndexando { get; set; }
        public DateTime? DataIndexado { get; set; }
        public DateTime? DataErro { get; set; }
        public int StatusId { get; set; }
        public string StatusDescricao { get; set; }
    }
}
