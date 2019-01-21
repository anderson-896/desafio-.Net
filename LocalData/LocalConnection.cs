using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalData
{
    public class LocalConnection
    {   
        public static DatabaseEntities GetConnection()
        {
            return new DatabaseEntities();
        }
    }
}
