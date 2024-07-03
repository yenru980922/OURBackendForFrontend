using Newtonsoft.Json;

namespace BackendForFrontend.Models.Dtos
{
	public class PaymentConfirmDto
	{
		[JsonProperty("amount")]
		public int Amount { get; set; }
		[JsonProperty("currency")]
		public string Currency { get; set; }
	}
}
