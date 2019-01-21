using System;
using System.Linq;
using System.Text;

namespace CrossCutting
{
    public interface ISecurityHelper
    {
        string SHA256(string input);

        string GenerateUniqueToken(DateTime? date = null);
    }

    public class SecurityHelper : ISecurityHelper
    {
        public string GenerateUniqueToken(DateTime? date = default(DateTime?))
        {
            if (date.HasValue)
            {
                byte[] time = BitConverter.GetBytes(DateTime.Now.ToBinary());
                byte[] key = Guid.NewGuid().ToByteArray();
                string token = Convert.ToBase64String(time.Concat(key).ToArray());
                return token;
            }
            else
                return Guid.NewGuid().ToString();
        }

        public string SHA256(string input)
        {
            StringBuilder sb = new StringBuilder();

            using (var sha2 = System.Security.Cryptography.SHA256.Create())
            {
                Encoding encoding = Encoding.UTF8;
                Byte[] bytes = sha2.ComputeHash(encoding.GetBytes(input));

                foreach (var item in bytes)
                {
                    sb.Append(item.ToString("x2"));
                }
            }
            return sb.ToString();
        }
    }
}
