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


            if (!string.IsNullOrEmpty(bindingContext.ValueProvider
                .GetValue($"{nameof(discountDto.Name)}").FirstValue.ToString()))
            {
                discountDto.Name = bindingContext.ValueProvider
                .GetValue($"{nameof(discountDto.Name)}").FirstValue;
            }


            if (!string.IsNullOrEmpty(bindingContext.ValueProvider
                .GetValue($"{nameof(discountDto.CouponCode)}").FirstValue.ToString()))
            {
                discountDto.CouponCode = bindingContext.ValueProvider
                .GetValue($"{nameof(discountDto.CouponCode)}").FirstValue;
            }

            if (!string.IsNullOrEmpty(bindingContext.ValueProvider
                .GetValue($"{nameof(discountDto.UsePercentage)}").FirstValue.ToString()))
            {
                discountDto.UsePercentage = bool.Parse(bindingContext.ValueProvider
                .GetValue($"{nameof(discountDto.UsePercentage)}").FirstValue);
            }

            if (!string.IsNullOrEmpty(bindingContext.ValueProvider
                .GetValue($"{nameof(discountDto.RequiresCouponCode)}").FirstValue.ToString()))
            {
                discountDto.RequiresCouponCode = bool.Parse(bindingContext.ValueProvider
                                .GetValue($"{nameof(discountDto.RequiresCouponCode)}").FirstValue.ToString());
            }

            if (!string.IsNullOrEmpty(bindingContext.ValueProvider
                .GetValue($"{nameof(discountDto.LimitationTimes)}").FirstValue.ToString()))
            {
                discountDto.LimitationTimes = int.Parse(bindingContext.ValueProvider
                                .GetValue($"{nameof(discountDto.LimitationTimes)}").FirstValue);
            }

            if (!string.IsNullOrEmpty(bindingContext.ValueProvider
                .GetValue($"{nameof(discountDto.DiscountAmount)}").FirstValue.ToString()))
            {
                discountDto.DiscountAmount = int.Parse(bindingContext.ValueProvider
                .GetValue($"{nameof(discountDto.DiscountAmount)}").FirstValue);
            }

            if (!string.IsNullOrEmpty(bindingContext.ValueProvider
                .GetValue($"{nameof(discountDto.DiscountPercentage)}").FirstValue.ToString()))
            {
                discountDto.DiscountPercentage = int.Parse(bindingContext.ValueProvider
               .GetValue($"{nameof(discountDto.DiscountPercentage)}").FirstValue);
            }

            if (!string.IsNullOrEmpty(bindingContext.ValueProvider
                 .GetValue($"{nameof(discountDto.DiscountType)}").FirstValue.ToString()))
            {
                discountDto.DiscountType = (DiscountType)int.Parse(bindingContext.ValueProvider
                .GetValue($"{nameof(discountDto.DiscountType)}").FirstValue);
            }

            if (!string.IsNullOrEmpty(bindingContext.ValueProvider
                .GetValue($"{nameof(discountDto.DiscountLimitation)}").FirstValue.ToString()))
            {
                discountDto.DiscountLimitation = (DiscountLimitationType)int.Parse(bindingContext.ValueProvider
                .GetValue($"{nameof(discountDto.DiscountLimitation)}").FirstValue);
            }


            if (!string.IsNullOrEmpty(bindingContext.ValueProvider
            .GetValue($"{nameof(discountDto.StartDate)}").FirstValue.ToString()))
            {
                discountDto.StartDate = PersianDateTime.Parse(bindingContext.ValueProvider
                .GetValue($"{nameof(discountDto.StartDate)}").FirstValue);
            }

            if (!string.IsNullOrEmpty(bindingContext.ValueProvider
            .GetValue($"{nameof(discountDto.EndDate)}").Values))
            {
                discountDto.EndDate = PersianDateTime.Parse(bindingContext.ValueProvider
                .GetValue($"{nameof(discountDto.EndDate)}").FirstValue.ToString());
            }

            var applidToProductItems = bindingContext.ValueProvider
                .GetValue($"{nameof(discountDto.ApplidToProductItem)}");

            if (!string.IsNullOrEmpty(applidToProductItems.Values))
            {
                discountDto.ApplidToProductItem = applidToProductItems
                .Values.ToString().Split(",").Select(x => int.Parse(x)).ToList();
            }


            bindingContext.Result = ModelBindingResult.Success(discountDto);

            return Task.CompletedTask;
        }
    }
}
