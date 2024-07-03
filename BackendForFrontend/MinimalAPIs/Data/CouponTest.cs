using MinimalAPIs.MinimalAPIs.Models;
using MinimalAPIs.Models;

namespace MinimalAPIs.Data
{
    public class CouponTest
    {
        public static List<Coupon> conponList = new()
        {
            new Coupon
            {
                CouponID = 1,
                Code = "DISCOUNT1",
                Description = "Discount1",
                DiscountValue = 10,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(10),
                UsingStatus = "Active"

            },
            new Coupon
            {
                CouponID = 2,
                Code = "DISCOUNT2",
                Description = "Discount2",
                DiscountValue = 20,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(10),
                UsingStatus = "Active"

            }
        };

    }
}
