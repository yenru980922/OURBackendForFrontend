using BackendForFrontend.Models.EFModels;
using MinimalAPIs.Models.DTOs;
using MinimalAPIs.Services;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using BackendForFrontend.MinimalAPIs.Models.DTOs;

namespace MinimalAPIs.Endpoints
{
    public static class CouponEndpoints
    {
        public static async Task<IResult> GetAllCoupons([FromServices] ICouponService couponService, [FromServices] ILogger<Program> logger)
        {
            logger.LogInformation("�C�X�Ҧ��u�f��");
            var coupons = await couponService.GetAllCouponsAsync();
            return TypedResults.Ok(coupons);
        }

        public static async Task<IResult> GetCouponById([FromRoute] int id, [FromServices] ICouponService couponService)
        {
            var coupon = await couponService.GetCouponByIdAsync(id);
            if (coupon == null)
            {
                return TypedResults.NotFound("�䤣�즹�u�f��");
            }
            return TypedResults.Ok(coupon);
        }

        public static async Task<IResult> GetCouponByCode([FromRoute] string code, [FromServices] ICouponService couponService)
        {
            var coupon = await couponService.GetCouponByCodeAsync(code);
            if (coupon == null)
            {
                return TypedResults.NotFound("���u�f��N�X���s�b");
            }
            return TypedResults.Ok(coupon);
        }

        public static async Task<IResult> AddCoupon([FromBody] CouponCreateDTO coupon, [FromServices] ICouponService couponService, [FromServices] IMapper mapper, [FromServices] IValidator<CouponCreateDTO> validator, [FromServices] ILogger<Program> logger)
        {
            logger.LogInformation("�s�W�u�f��");
            var validationResult = await validator.ValidateAsync(coupon);
            if (!validationResult.IsValid)
            {
                return TypedResults.ValidationProblem(validationResult.ToDictionary());
            }

            var newCoupon = await couponService.CreateCouponAsync(coupon);
            return TypedResults.CreatedAtRoute(
                routeName: "GetCouponById",
                routeValues: new { id = newCoupon.CouponID },
                value: newCoupon
            );
        }

        public static async Task<IResult> UpdateCoupon([FromRoute] int id, [FromBody] CouponCreateDTO coupon, [FromServices] ICouponService couponService, [FromServices] IMapper mapper, [FromServices] IValidator<CouponCreateDTO> validator, [FromServices] ILogger<Program> logger)
        {
            logger.LogInformation("��s�u�f��");
            var validationResult = await validator.ValidateAsync(coupon);
            if (!validationResult.IsValid)
            {
                return TypedResults.ValidationProblem(validationResult.ToDictionary());
            }

            var updatedCoupon = await couponService.UpdateCouponAsync(id, coupon);
            if (updatedCoupon == null)
            {
                return TypedResults.NotFound("��s���ѡA�䤣�즹�u�f��");
            }
            return TypedResults.Ok(updatedCoupon);
        }

        public static async Task<IResult> UpdateCouponByCode([FromRoute] string code, [FromBody] CouponCreateDTO coupon, [FromServices] ICouponService couponService, [FromServices] ILogger<Program> logger)
        {
            logger.LogInformation("��s�u�f��");
            var updatedCoupon = await couponService.UpdateCouponByCodeAsync(code, coupon);
            if (updatedCoupon == null)
            {
                return TypedResults.NotFound("��s���ѡA�䤣�즹�u�f��");
            }
            return TypedResults.Ok(updatedCoupon);
        }

        public static async Task<IResult> DeleteCoupon([FromRoute] int id, [FromServices] ICouponService couponService)
        {
            var result = await couponService.DeleteCouponAsync(id);
            if (!result)
            {
                return TypedResults.NotFound("�R�����ѡA�L���u�f��");
            }
            return TypedResults.Ok();
        }

        public static async Task<IResult> DeleteCouponByCode([FromRoute] string code, [FromServices] ICouponService couponService)
        {
            var result = await couponService.DeleteCouponByCodeAsync(code);
            if (!result)
            {
                return TypedResults.NotFound("�R�����ѡA�L���u�f��");
            }
            return TypedResults.Ok();
        }

    }
}