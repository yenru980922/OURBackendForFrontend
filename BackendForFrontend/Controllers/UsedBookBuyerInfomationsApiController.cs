using BackendForFrontend.Models.Dtos;
using BackendForFrontend.Models.EFModels;
using BackendForFrontend202401.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging.Signing;
using System.Numerics;

namespace BackendForFrontend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsedBookBuyerInfomationsApiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsedBookBuyerInfomationsApiController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<UsedBookBuyerInformation>> Get(int orderId)
        {
            var buyerInfomation = await _context.UsedBookBuyerInformations.FirstOrDefaultAsync(i => i.OrderId == orderId.ToString());

            if(buyerInfomation == null) { return NotFound(); }

            return buyerInfomation;
        }

        [HttpPost]
        public async Task<ActionResult<string>> Post(UsedBookBuyerInformation data)
        {
            _context.UsedBookBuyerInformations.Add(data);
            await _context.SaveChangesAsync();

            return ("新增成功");
        }

        [HttpPost("orderRecipient")]
        public async Task<ActionResult<string>> PostOrderRecipient(string paymentId)
        {
            var buyerInfomations = new List<UsedBookBuyerInformation>();
            List<string> ids = new List<string>();

            //找到付款紀錄下的訂單編號
            var payment=await _context.UsedBookPaymentRecords.FirstOrDefaultAsync(p => paymentId == p.PaymentNumber);
            if (payment != null)
            {
                string Ids = payment.OrderId;
                ids = Ids.Split(',').ToList(); ;
            }

            //找到對應收件人資料
            var information=await _context.UsedBookBuyerInformations.FirstOrDefaultAsync(b => b.OrderId == paymentId);
            if (information != null)
            {
                foreach (string id in ids)
                {
                    UsedBookBuyerInformation buyerInformation = new UsedBookBuyerInformation
                    {
                        OrderId = id,
                        RecipientName = information.RecipientName,
                        RecipientPhone = information.RecipientPhone,
                        RecipientAddress = information.RecipientAddress,
                        RecipientEmail = information.RecipientEmail,
                        Remark = information.Remark
                    };
                    buyerInfomations.Add(buyerInformation);
                }
            }
            await Task.WhenAll(
                _context.UsedBookBuyerInformations.AddRangeAsync(buyerInfomations)
            );

            _context.UsedBookBuyerInformations.Remove(information);

            await _context.SaveChangesAsync();

            return ("新增成功");
        }
    }
}
