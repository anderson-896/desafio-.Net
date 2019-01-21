using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Domain.Response;
using Suporte.Domain.Entities;
using Suporte.Domain.Repositories;
using DatabaseHelper;
using Dapper;

namespace Suporte.Repositories
{
    public class UserRepository : IUserRepository
    {
        protected IConnectionFactory connectionFactory;

        public UserRepository(IConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory;
        }

        public async Task<User> SearchUserById(int UserId)
        {
            using (var connection = connectionFactory.GetConnection(Connections.ANALYTICS))
            {
                var query = @"SELECT [CD_USUARIO] AS UserId, [LOGIN], [NOME] As Name, [EMAIL] FROM [dbo].[TB_USUARIO] WHERE [CD_USUARIO] = @UserId";

                return (await connection.QueryAsync<User>(query, new { UserId = UserId })).FirstOrDefault();
            }
        }

        public async Task<User> SearchUserByLogin(string login)
        {
            using (var connection = connectionFactory.GetConnection(Connections.ANALYTICS))
            {
                var query = @"SELECT [CD_USUARIO] AS UserId, [LOGIN], [NOME] As Name, [EMAIL] FROM [dbo].[TB_USUARIO] WHERE [LOGIN] = @Login";

                return (await connection.QueryAsync<User>(query, new { login = login })).FirstOrDefault();
            }
        }

      
    }
}
