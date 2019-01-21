using Auth.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Auth.Domain.Entities;
using LocalData;
using System.Data.Entity;

namespace Auth.Repositories
{
    public class UserTokenRepository : IUserTokenRepository
    {

        public async Task DeleteByEmailAsync(string email)
        {
            using (var db = LocalConnection.GetConnection())
            {
                var usr = await db.TB_USER.Where(u => u.EMAIL == email).FirstOrDefaultAsync();
                db.TB_TOKEN_AUTHENTICATION
                    .RemoveRange(usr.TB_TOKEN_AUTHENTICATION);
                await db.SaveChangesAsync();
            }
        }

        public async Task InsertToken(UserToken token)
        {
            using (var db = LocalConnection.GetConnection())
            {
                db.TB_TOKEN_AUTHENTICATION.Add(new TB_TOKEN_AUTHENTICATION {
                    IS_VALID = true,
                    TOKEN = token.Token,
                    USER_ID = token.UserId,
                    CREATE_DATE = DateTime.Now,
                    UPDATE_DATE = DateTime.Now
                });
                await db.SaveChangesAsync();
            }
        }

        public async Task<UserCredentials> SearchAsync(string login)
        {
            using (var db = LocalConnection.GetConnection())
            {
                TB_USER user = await db.TB_USER.Where(u => u.EMAIL == login).FirstOrDefaultAsync();
                if (user != null)
                {
                    return new UserCredentials()
                    {
                        Id = user.ID,
                        IsActive = user.IS_ACTIVE,
                        Email = user.EMAIL,
                        Password = user.PASSWORD
                    };
                } else
                {
                    return null;
                }
            }
        }

        public async Task<UserToken> SearchByTokenAsync(string token)
        {
            using (var db = LocalConnection.GetConnection())
            {
                var t = await db.TB_TOKEN_AUTHENTICATION.Where(x => x.TOKEN == token).FirstOrDefaultAsync();
                if (t != null)
                {
                    return new UserToken()
                    {
                        UserId = t.USER_ID,
                        CreateDate = t.CREATE_DATE,
                        Email = t.TB_USER.EMAIL,
                        IsValid = t.IS_VALID,
                        ModifyDate = t.UPDATE_DATE,
                        Token = t.TOKEN
                    };

                } else
                {
                    return null;
                }
            }
        }

        public async Task<IEnumerable<UserToken>> SearchTokensByLoginAsync(string login)
        {
            using (var db = LocalConnection.GetConnection())
            {
                var t = await db.TB_TOKEN_AUTHENTICATION.Where(x => x.TB_USER.EMAIL == login).ToListAsync();
                List<UserToken> tks = new List<UserToken>();
                t.ForEach(tk =>
                {
                    tks.Add(new UserToken()
                    {
                        UserId = tk.USER_ID,
                        CreateDate = tk.CREATE_DATE,
                        Email = tk.TB_USER.EMAIL,
                        IsValid = tk.IS_VALID,
                        ModifyDate = tk.UPDATE_DATE,
                        Token = tk.TOKEN
                    });
                });
                return tks;
            }
           
        }

        public async Task UpdateTokenAsync(UserToken token)
        {
            using (var db = LocalConnection.GetConnection())
            {
                var tk = await db.TB_TOKEN_AUTHENTICATION.Where(t => t.TOKEN == token.Token).FirstOrDefaultAsync();
                if (tk != null)
                {
                    tk.IS_VALID = token.IsValid;
                    tk.UPDATE_DATE = tk.UPDATE_DATE;
                    await db.SaveChangesAsync();
                }
            }
            
        }
    }
}
