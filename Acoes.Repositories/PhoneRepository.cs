using Actions.Domain.Entities;
using Actions.Domain.Repositories;
using LocalData;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Actions.Repositories
{
    public class PhoneRepository : IPhoneRepository
    {
        public async Task InsertPhonesAsync(Phone[] phones, int userId)
        {
            using (var db = LocalConnection.GetConnection())
            {                
                foreach (var phone in phones)
                {
                    db.TB_PHONE.Add(new TB_PHONE()
                    {
                        NUMBER = (int)phone.Number,
                        AREA_CODE = (int)phone.Area_code,
                        COUNTRY_CODE = phone.Country_code,
                        USER_ID = userId,
                        CREATE_DATE = DateTime.Now,
                        UPDATE_DATE = DateTime.Now
                    });
                }
               
                await db.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Phone>> SearchByUserIdAsync(int userId)
        {
            using (var db = LocalConnection.GetConnection())
            {
                var phones = await db.TB_PHONE.Where(p => p.USER_ID == userId).ToListAsync();
                List<Phone> result = new List<Phone>();
                phones.ForEach(p =>
                {
                    result.Add(new Phone()
                    {
                        Number = p.NUMBER,
                        Area_code = p.AREA_CODE,
                        Country_code = p.COUNTRY_CODE
                    });
                });
                return result;
            }
        }
    }
}
