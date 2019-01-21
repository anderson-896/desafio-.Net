using Kurier.Analytics.Core.Domain.Response;
using Kurier.Analytics.Search.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kurier.Analytics.Search.Domain.Services
{
    public interface IQueryService
    {
        Task<IResponse<IEnumerable<UserSimpleQuery>>> SearchByUserAsync(int userId);
        Task<IResponse> InsertAsync(UserSimpleQuery query);
        Task<IResponse> UpdateAsync(UserSimpleQuery query);
        Task<IResponse> DeleteAsync(UserSimpleQuery queryId);
        Task<IResponse<UserSimpleQuery>> SearchByQueryAsync(string queryId);
        Task<IResponse<IEnumerable<UserSimpleQuery>>> SearchLastAsync(int userId);
    }
}
