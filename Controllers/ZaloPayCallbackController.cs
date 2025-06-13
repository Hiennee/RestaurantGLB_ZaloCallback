using Microsoft.AspNetCore.Mvc;
using System;
using Newtonsoft.Json.Linq;
using RestaurantGLB_Webserver.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using ZaloPay.Helper.Crypto;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace RestaurantGLB_Webserver.Controllers
{
    [ApiController]
    [Route("api/zalo")]
    public class ZaloPayCallbackController : ControllerBase
    {
        private readonly UnionPOSContext _context;

        public ZaloPayCallbackController(UnionPOSContext context)
        {
            _context = context;
        }

        [HttpPost("callback")]
        public async Task<IActionResult> Post([FromBody] dynamic req)
        {
            var result = new Dictionary<string, object>();
            try
            {
                JsonElement jsonElement = (JsonElement)req;
                string jsonString = jsonElement.GetRawText();
                JObject reqJson = JObject.Parse(jsonString);

                string key2 = _context.MerchantAccountsZalopays.Select(zalo => zalo.Key2).FirstOrDefault() ?? "";
                var dataStr = Convert.ToString(reqJson["data"]);
                var reqMac = Convert.ToString(reqJson["mac"]);

                var mac = HmacHelper.Compute(ZaloPayHMAC.HMACSHA256, key2, dataStr);

                Console.WriteLine("mac = {0}", mac);

                if (!reqMac.Equals(mac))
                {
                    result["return_code"] = -1;
                    result["return_message"] = "mac not equal";
                }
                else
                {
                    var dataJson = JsonConvert.DeserializeObject<Dictionary<string, object>>(dataStr);
                    Console.WriteLine("update order's status = success where app_trans_id = {0}", dataJson["app_trans_id"]);

                    ZaloPayCallbackDetail data = new ZaloPayCallbackDetail();
                    data.AppId = Convert.ToInt32(dataJson["app_id"]);
                    data.AppTransId = Convert.ToString(dataJson["app_trans_id"]) ?? "";
                    data.AppTime = Convert.ToInt64(dataJson["app_time"]);
                    data.AppUser = Convert.ToString(dataJson["app_user"]) ?? "";
                    data.Amount = Convert.ToInt64(dataJson["amount"]);
                    data.EmbedData = Convert.ToString(dataJson["embed_data"]) ?? "";
                    data.Item = Convert.ToString(dataJson["item"]) ?? "";
                    data.ZpTransId = Convert.ToString(dataJson["zp_trans_id"]) ?? "";
                    data.ServerTime = Convert.ToInt64(dataJson["server_time"]);
                    data.Channel = Convert.ToInt32(dataJson["channel"]);
                    data.MerchantUserId = Convert.ToString(dataJson["merchant_user_id"]) ?? "";
                    data.UserFeeAmount = Convert.ToInt64(dataJson["user_fee_amount"]);
                    data.DiscountAmount = Convert.ToInt64(dataJson["discount_amount"]);

                    await _context.ZaloPayCallbackDetails.AddAsync(data);
                    await _context.SaveChangesAsync();

                    result["return_code"] = 1;
                    result["return_message"] = "success";
                }
            }
            catch (Exception ex)
            {
                result["return_code"] = 0; // ZaloPay server sẽ callback lại (tối đa 3 lần)
                result["return_message"] = ex.Message;
            }
            return Ok(result);
        }
    }
}
