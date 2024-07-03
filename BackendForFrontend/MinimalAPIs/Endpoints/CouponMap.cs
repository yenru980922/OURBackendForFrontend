using BackendForFrontend.MinimalAPIs.Models.DTOs;
using BackendForFrontend.Models.EFModels;
using MinimalAPIs.Models.DTOs;

namespace MinimalAPIs.Endpoints;

public static class CouponMap
{
    public static IEndpointRouteBuilder MapCouponEndpoints(this IEndpointRouteBuilder builder)
    {
        var groupBuilder = builder.MapGroup("/api/Coupon");
        groupBuilder.MapGet("/", CouponEndpoints.GetAllCoupons)
            .WithName("GetAllCoupon")
            // .Produces<APIResponse<IEnumerable<Coupon>>>(200)
            .WithTags("Coupon API");

        groupBuilder.MapGet("/{id:int}", CouponEndpoints.GetCouponById)
            .WithName("GetCouponById")
            .Produces<APIResponse<Coupon>>(404)
            .WithTags("Coupon API");

        groupBuilder.MapGet("/{code}", CouponEndpoints.GetCouponByCode)
            .WithName("GetCouponByCode")
            .Produces<APIResponse<Coupon>>(404)
            .WithTags("Coupon API");

        groupBuilder.MapPost("/", CouponEndpoints.AddCoupon)
            .WithName("AddCoupon")
            .Produces<CouponDTO>(201)
            .Produces(400)
            .WithTags("Coupon API")
            .Accepts<CouponCreateDTO>("application/json");

        groupBuilder.MapPut("/{id:int}", CouponEndpoints.UpdateCoupon)
            .WithName("UpdateCoupon")
            .Produces(200)
            .WithTags("Coupon API");

        groupBuilder.MapPut("/{code}", CouponEndpoints.UpdateCouponByCode)
            .WithName("UpdateCouponByCode")
            .Produces(200)
            .WithTags("Coupon API");

        groupBuilder.MapDelete("/{id:int}", CouponEndpoints.DeleteCoupon)
            .WithName("DeleteCoupon")
            .Produces(200)
            .WithTags("Coupon API");

        groupBuilder.MapDelete("/{code}", CouponEndpoints.DeleteCouponByCode)
            .WithName("DeleteCouponByCode")
            .Produces(200)
            .WithTags("Coupon API");

        return builder;
    }
}
