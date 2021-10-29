using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using Shop_Example.Entities.Products.Comments;

namespace Shop_Example.Entities.Models
{
    public class User : IdentityUser
    {
        public string FullName { get; set; }

        public string ImageProfileName { get; set; }

        //
        public Address Address { get; set; }
        public ICollection<Comment> Comments { get; set; }


    }
}
