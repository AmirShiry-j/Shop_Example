using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop_Example.Dtoes.Admin.User
{
    public class IndexPageUsersVM
    {
        public int Page { get; set; }
        public int CounInPage { get; set; }
        public int CountAllItems { get; set; }

        public List<ListInfoUserDto> Users { get; set; }
    }
    public class ListInfoUserDto
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public bool ConfirmedEmail { get; set; }
        public string PhoneNumber { get; set; }
        public bool ConfirmedPhoneNumber { get; set; }
        public string Roles { get; set; }
    }
}
