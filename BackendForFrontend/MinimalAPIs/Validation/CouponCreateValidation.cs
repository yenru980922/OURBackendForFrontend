using MinimalAPIs.Models.DTOs;
using FluentValidation;

namespace MinimalAPIs.Validation
{
    public class CouponCreateValidation : AbstractValidator<CouponCreateDTO>
    {
        public CouponCreateValidation()
        {
            RuleFor(x => x.Code).NotEmpty().WithMessage("優惠券代碼不得為空。");
            RuleFor(x => x.DiscountValue).InclusiveBetween(1, 100).WithMessage("優惠比例必須介於 1 到 100 之間。");
            RuleFor(x => x.DiscountValue).GreaterThan(0).WithMessage("優惠比例必須大於 0。");
            RuleFor(x => x.DiscountValue).LessThan(100).WithMessage("優惠比例必須小於100。");
        }
    }
}
