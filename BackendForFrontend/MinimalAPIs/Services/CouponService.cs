using BackendForFrontend.Models.EFModels;
using MinimalAPIs.Models.DTOs;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace MinimalAPIs.Services
{
    public class CouponService : ICouponService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public CouponService(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CouponDTO>> GetAllCouponsAsync()
        {
            var coupons = await _dbContext.Coupons.ToListAsync();
            return _mapper.Map<IEnumerable<CouponDTO>>(coupons);
        }

        public async Task<CouponDTO> GetCouponByIdAsync(int id)
        {
            var coupon = await _dbContext.Coupons.FindAsync(id);
            return _mapper.Map<CouponDTO>(coupon);
        }

        public async Task<CouponDTO> GetCouponByCodeAsync(string code)
        {
            var coupon = await _dbContext.Coupons.FirstOrDefaultAsync(c => c.Code == code);
            return _mapper.Map<CouponDTO>(coupon);
        }

        public async Task<CouponDTO> CreateCouponAsync(CouponCreateDTO couponCreateDTO)
        {
            var coupon = _mapper.Map<Coupon>(couponCreateDTO);
            _dbContext.Coupons.Add(coupon);
            await _dbContext.SaveChangesAsync();
            return _mapper.Map<CouponDTO>(coupon);
        }

        public async Task<CouponDTO> UpdateCouponAsync(int id, CouponCreateDTO couponCreateDTO)
        {
            var coupon = await _dbContext.Coupons.FindAsync(id);
            if (coupon == null)
            {
                return null;
            }

            _mapper.Map(couponCreateDTO, coupon);
            await _dbContext.SaveChangesAsync();
            return _mapper.Map<CouponDTO>(coupon);
        }

        public async Task<CouponDTO> UpdateCouponByCodeAsync(string code, CouponCreateDTO couponCreateDTO)
        {
            var coupon = await _dbContext.Coupons.FirstOrDefaultAsync(c => c.Code == code);
            if (coupon == null)
            {
                return null;
            }

            _mapper.Map(couponCreateDTO, coupon);
            await _dbContext.SaveChangesAsync();
            return _mapper.Map<CouponDTO>(coupon);
        }

        public async Task<bool> DeleteCouponAsync(int id)
        {
            var coupon = await _dbContext.Coupons.FindAsync(id);
            if (coupon == null)
            {
                return false;
            }

            _dbContext.Coupons.Remove(coupon);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteCouponByCodeAsync(string code)
        {
            var coupon = await _dbContext.Coupons.FirstOrDefaultAsync(c => c.Code == code);
            if (coupon == null)
            {
                return false;
            }

            _dbContext.Coupons.Remove(coupon);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}