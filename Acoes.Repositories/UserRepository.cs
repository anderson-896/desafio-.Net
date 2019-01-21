using Actions.Domain.Entities;
using Actions.Domain.Repositories;
using LocalData;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Actions.Repositories
{
    public class UserRepository : IUserRepository
    {
        public async Task InsertAsync(User user)
        {
            using (var db = LocalConnection.GetConnection())
            {
                // Insert User 
                var tb_user = new TB_USER()
                {
                    FIRIST_NAME = user.FirstName,
                    LAST_NAME = user.LastName,
                    EMAIL = user.Email,
                    PASSWORD = user.Password,
                    CREATE_DATE = DateTime.Now,
                    IS_ACTIVE = true,
                    UPDATE_DATE = DateTime.Now
                };
                db.TB_USER.Add(tb_user);
                
                // Insert phones
                user.Phones.ForEach(p =>
                {
                    db.TB_PHONE.Add(new TB_PHONE()
                    {
                        NUMBER = (int)p.Number,
                        AREA_CODE = (int)p.Area_code,
                        COUNTRY_CODE = p.Country_code,
                        CREATE_DATE = DateTime.Now,
                        USER_ID = tb_user.ID
                    });
                });

                await db.SaveChangesAsync();
            }
        }

        public async Task<User> SearchByEmailAsync(string userEmail)
        {
            using (var db = LocalConnection.GetConnection())
            {
                TB_USER user = await db.TB_USER.Where(u => u.EMAIL == userEmail).FirstOrDefaultAsync();
                if (user != null)
                {
                    return new User()
                    {
                        UserId = user.ID,
                        Email = user.EMAIL,
                        Created_at = user.CREATE_DATE,
                        FirstName = user.FIRIST_NAME,
                        LastName = user.LAST_NAME
                    };
                } else
                {
                    return null;
                }
                
            }
        }

        public async Task<User> SearchByIdAsync(int userId)
        {
            using (var db = LocalConnection.GetConnection())
            {
                TB_USER user = await db.TB_USER.Where(u => u.ID == userId).FirstOrDefaultAsync();
                return new User()
                {
                    UserId = user.ID,
                    Email = user.EMAIL,
                    Created_at = user.CREATE_DATE,
                    FirstName = user.FIRIST_NAME,
                    LastName = user.LAST_NAME                    
                };
            }
        }       
    }
}
