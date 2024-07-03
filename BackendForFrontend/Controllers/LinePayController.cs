using BackendForFrontend.Models.Dtos;
using BackendForFrontend.Models.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackendForFrontend.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class LinePayController : ControllerBase
	{
		private readonly LinePayService _linePayService;
		public LinePayController()
		{
			_linePayService = new LinePayService();
		}

		[HttpPost("Create")]
		public async Task<PaymentResponseDto> CreatePayment(PaymentRequestDto dto)
		{
			return await _linePayService.SendPaymentRequest(dto);
		}

		[HttpPost("Confirm")]
		public async Task<PaymentConfirmResponseDto> ConfirmPayment([FromQuery] string transactionId, [FromQuery] string orderId, PaymentConfirmDto dto)
		{
			return await _linePayService.ConfirmPayment(transactionId, orderId, dto);
		}

		[HttpGet("Cancel")]
		public async void CancelTransaction([FromQuery] string transactionId)
		{
			_linePayService.TransactionCancel(transactionId);
		}

	}
}

