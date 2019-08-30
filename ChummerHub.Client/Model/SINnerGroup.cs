using System.Security.Cryptography;
using System.Text;

namespace SINners.Models
{
    public partial class SINnerGroup
    {
        public string Password
        {
            set
            {
                if (!string.IsNullOrEmpty(value))
                    this.PasswordHash = GetHashString(value);
                else
                {
                    this.PasswordHash = null;
                }
            }
        }

        public SINnerGroup(SINnerSearchGroup searchGroup)
        {
            Id = searchGroup.Id;
            Groupname = searchGroup.Groupname;
            IsPublic = searchGroup.IsPublic;
            MyParentGroupId = searchGroup.MyParentGroupId;
            Language = searchGroup.Language;
            MyAdminIdentityRole = searchGroup.MyAdminIdentityRole;
        }
        public static byte[] GetHash(string inputString)
        {
            HashAlgorithm algorithm = SHA256.Create();
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }

        public static string GetHashString(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(inputString))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }
    }
}
