using BackendForFrontend.Models.EFModels;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MinimalAPIs.Models.DTOs
{
    public partial class CouponDTO
    {
        public int CouponID { get; set; }

        public int PromotionID { get; set; }

        public string Code { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public bool? Valid { get; set; }

        public string Description { get; set; }

        public int AvailabilityCount { get; set; }

        public string UsingStatus { get; set; }

        public int MinimumValue { get; set; }

        public int DiscountValue { get; set; }

        public int DiscountLimit { get; set; }

        public int CouponTypeId { get; set; }

        public virtual ICollection<CouponRedemption> CouponRedemptions { get; set; } = new List<CouponRedemption>();

        public virtual CouponType CouponType { get; set; }
    }
}