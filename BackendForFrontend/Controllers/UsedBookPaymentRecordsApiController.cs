using BackendForFrontend.Models.Dtos;
using BackendForFrontend.Models.EFModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendForFrontend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsedBookPaymentRecordsApiController : ControllerBase
    {
        private readonly AppDbContext _context;
        public UsedBookPaymentRecordsApiController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IEnumerable<UsedBookPaymentRecord>> Get(string paymentNumber)
        {
            var paymentRecord = await _context.UsedBookPaymentRecords.Where(i => i.PaymentNumber == paymentNumber).ToListAsync();

            return paymentRecord;
        }

        [HttpPost]
        public async Task<ActionResult<string>> Post(UsedBookPaymentRecord data)
        {

            _context.UsedBookPaymentRecords.Add(data);
            await _context.SaveChangesAsync();

            return ("新增成功");
        }

        [HttpPut]
        public async Task<ActionResult<string>> Put(string paymentNumber, bool status)
        {
            UsedBookPaymentRecord paymentRecord = _context.UsedBookPaymentRecords.FirstOrDefault(x => x.PaymentNumber == paymentNumber);

            if (paymentRecord == null) { return ("付款編號不存在"); }
            
            paymentRecord.status = status;

            try
            {
                await _context.SaveChangesAsync();
                return ("修改成功");
            }
            catch (Exception ex)
            {
                // 處理保存失敗的情況
                return StatusCode(500, "無法保存更改: " + ex.Message);
            }
        }
    }
}
