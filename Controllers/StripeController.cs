using ERP2024.Helpers;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace ERP2024.Controllers
{
    [Route("api/pay")]
    [ApiController]
    public class StripeController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public StripeController(IConfiguration configuration)
        {
            _configuration = configuration;
            StripeConfiguration.ApiKey = _configuration["Stripe:SecretKey"];
        }

        [HttpPost("create-payment-intent")]
        public async Task<IActionResult> CreatePaymentIntent([FromBody] PaymentIntentCreateRequest request)
        {
            var customerService = new CustomerService();
            Customer customer;

            // Check if customer exists
            var customers = customerService.List(new CustomerListOptions { Email = request.email }).Data;
            if (customers.Count > 0)
            {
                customer = customers.First();
            }
            else
            {
                // Create customer if not exists
                var customerOptions = new CustomerCreateOptions
                {
                    Email = request.email,
                    Description = "Customer for " + request.email
                };
                var requestOptions = new RequestOptions
                {
                    IdempotencyKey = "KG5LxwFBepaKHyUD",
                };
                customer = await customerService.CreateAsync(customerOptions);
            }
            

            var options = new PaymentIntentCreateOptions
            {
                Amount = request.amount,
                Customer = customer.Id,
                ReceiptEmail = request.email,
                Currency = "rsd",
                PaymentMethod = "pm_card_visa",
                Confirm = true,
                PaymentMethodTypes = new List<string> { "card" }
            };

            var service = new PaymentIntentService();
            var paymentIntent = await service.CreateAsync(options);

            return Ok(new { clientSecret = paymentIntent.ClientSecret });
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> Webhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var stripeEvent = EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"], _configuration["Stripe:WebhookSecret"]);

            if (stripeEvent.Type == Events.PaymentIntentSucceeded)
            {
                var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                // Handle the event here
                Console.WriteLine($"PaymentIntent succeeded: {paymentIntent.Id}");
            }
            else if (stripeEvent.Type == Events.PaymentIntentPaymentFailed)
            {
                var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                // Handle the event here
                Console.WriteLine($"PaymentIntent failed: {paymentIntent.Id}");
            }
            else
            {
                Console.WriteLine($"Unhandled event type: {stripeEvent.Type}");
            }

            return Ok();
        }

    }
    public class PaymentIntentCreateRequest
    {
        public long amount { get; set; }
        public string email { get; set; }  
        public string token { get; set; }
    }
}
