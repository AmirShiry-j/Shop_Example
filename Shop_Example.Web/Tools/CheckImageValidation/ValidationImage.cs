using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Shop_Example.Web.Tools.CheckImageValidation
{
    public static class ValidationImage
    {
        public static Func<IFormFile, bool> Validate = file =>
        {
            if (Path.GetExtension(file.FileName) == ".png"|| Path.GetExtension(file.FileName) == ".jpg")
            {
                return true;
            }

            return false;
        };
    }
}
