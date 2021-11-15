using MD.PersianDateTime.Standard;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Shop_Example.Dtoes.Admin.Discount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop_Example.Web.Areas.Admin.ModelBinders.Discount
{
    public class DiscountEntityBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            CreateDiscountDto discountDto = new CreateDiscountDto();
            
            discountDto.Name = bindingContext.ValueProvider
                .GetValue($"{nameof(discountDto.Name)}").FirstValue;

            discountDto.CouponCode = bindingContext.ValueProvider
                .GetValue($"{nameof(discountDto.CouponCode)}").FirstValue;

            discountDto.UsePercentage = bool.Parse(bindingContext.ValueProvider
                .GetValue($"{nameof(discountDto.UsePercentage)}").FirstValue);

            discountDto.RequiresCouponCode = bool.Parse(bindingContext.ValueProvider
                .GetValue($"{nameof(discountDto.RequiresCouponCode)}").FirstValue);

            discountDto.LimitationTimes = int.Parse(bindingContext.ValueProvider
                .GetValue($"{nameof(discountDto.LimitationTimes)}").FirstValue);

            discountDto.DiscountAmount = int.Parse(bindingContext.ValueProvider
                .GetValue($"{nameof(discountDto.DiscountAmount)}").FirstValue);

            discountDto.DiscountPercentage = int.Parse(bindingContext.ValueProvider
                .GetValue($"{nameof(discountDto.DiscountPercentage)}").FirstValue);

            discountDto.DiscountType = (DiscountType)int.Parse(bindingContext.ValueProvider
                .GetValue($"{nameof(discountDto.DiscountType)}").FirstValue);

            discountDto.DiscountLimitation = (DiscountLimitationType)int.Parse(bindingContext.ValueProvider
                .GetValue($"{nameof(discountDto.DiscountLimitation)}").FirstValue);

            discountDto.StartDate = PersianDateTime.Parse(bindingContext.ValueProvider
                .GetValue($"{nameof(discountDto.StartDate)}").Values.ToString());

            discountDto.EndDate = PersianDateTime.Parse(bindingContext.ValueProvider
                .GetValue($"{nameof(discountDto.EndDate)}").Values.ToString());


            var applidToProductItems = bindingContext.ValueProvider
                .GetValue($"{nameof(discountDto.ApplidToProductItem)}");

            if (applidToProductItems != null)
            {
                discountDto.ApplidToProductItem = applidToProductItems
                .Values.ToString().Split(",").Select(x => int.Parse(x)).ToList();
            }

            bindingContext.Result = ModelBindingResult.Success(discountDto);

            return Task.CompletedTask;
        }
    }
}
