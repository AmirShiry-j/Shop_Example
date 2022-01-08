using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop_Example.Dtoes.Profile
{
    public class FullInfoUserDto
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public bool IsConfirmEmail { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsConfirmedPhoneNumber { get; set; }
        public bool TwoFactorLogin { get; set; }
    }
}
