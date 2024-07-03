using MinimalAPIs.Models.DTOs;

namespace MinimalAPIs.Services;

public interface ICouponService
{
    Task<IEnumerable<CouponDTO>> GetAllCouponsAsync();
    Task<CouponDTO> GetCouponByIdAsync(int id);
    Task<CouponDTO> GetCouponByCodeAsync(string code);
    Task<CouponDTO> CreateCouponAsync(CouponCreateDTO couponCreateDTO);
    Task<CouponDTO> UpdateCouponAsync(int id, CouponCreateDTO couponCreateDTO);
    Task<CouponDTO> UpdateCouponByCodeAsync(string code, CouponCreateDTO couponCreateDTO);
    Task<bool> DeleteCouponAsync(int id);
    Task<bool> DeleteCouponByCodeAsync(string code);
}