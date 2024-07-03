namespace MinimalAPIs.Endpoints;
using BackendForFrontend.Models.EFModels;

public record CouponResponse(
    int NumberOfCoupons,
    IReadOnlyCollection<CouponResponseItem> Data);

public record CouponResponseItem(
    int CouponId,
    string Code,
    DateTime StartDate,
    DateTime EndDate,
    bool Valid,
    string Description,
    int AvailabilityCount,
    string UsingStatus,
    int MinimumValue,
    int DiscountValue,
    int DiscountLimit
    );