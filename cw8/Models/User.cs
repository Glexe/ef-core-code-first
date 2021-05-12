using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cw8.Models
{
    public partial class User
    {
        public enum UserRoles : int
        {
            [StringValue(nameof(Admin))]
            Admin,
            [StringValue(nameof(User))]
            User
        }

        public int UserID { get; set; }
        public string Login { get; set; }
        public byte[] PasswordHashed { get; set; }
        public string UserRole { get; set; }
        public byte[] Salt { get; set; }

        public byte[] RefreshToken { get; set; }
        public DateTime RefreshTokenExpirationDate { get; set; }
    }
}
