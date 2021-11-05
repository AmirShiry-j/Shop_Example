using Shop_Example.DataLayer.Repositorys.UnitOfWorkRepository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop_Example.Web.Tools.GetAvgStarsProduct
{
    public class AvgStarsProduct
    {
        private readonly IUnitOfWork _unitOfWork;
        public AvgStarsProduct(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public byte GetAvgStars(int ProductId)
        {
            var comments = _unitOfWork.CommentRepository.GetAllAsync(p => p.ProductId == ProductId,
                                                                     include => include.Stars).Result;
            if (comments == null || !comments.Any())
            {
                return 1;//مقدار پیش فرض
            }
            else
            {
                var stars = comments.Select(p => p.Stars);

                byte avgStarts = (byte)stars.Average(p => p.AverageStars);

                return avgStarts;
            }
        }
    }
}
