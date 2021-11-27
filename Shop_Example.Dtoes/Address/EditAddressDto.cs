using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop_Example.Dtoes.Address
{
    public class EditAddressDto
    {
        [Required(ErrorMessage = "لطفا نام و نام خانوادگی تحویل گیرنده محصول را وارد کنید")]
        public string RecipientName { set; get; }

        public List<SelectListItem> Uniteds { get; set; }
        [Required(ErrorMessage = "لطفا استان محل سکونت خود را انتخاب کنید")]
        public int UnitedId { get; set; }

        public List<SelectListItem> Cities { get; set; }
        [Required(ErrorMessage = "لطفا شهر محل سکونت خود را انتخاب کنید")]
        public int CityId { get; set; }

        [Required(ErrorMessage = "لطفا کد پستی خود را وارد کنید")]
        public string PostalCode { get; set; }

        [Required(ErrorMessage = "لطفا آدرس دقیق محل دریافت محصول را وارد کنید")]
        public string FullAddress { get; set; }

        [Required]
        public int AddressId { get; set; }
    }
}
