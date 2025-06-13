using System;
using System.Collections.Generic;

namespace RestaurantGLB_Webserver.Models
{
    public partial class ZaloPayCallbackDetail
    {
        public int AppId { get; set; }
        public string AppTransId { get; set; } = null!;
        public long AppTime { get; set; }
        public string AppUser { get; set; } = null!;
        public long Amount { get; set; }
        public string EmbedData { get; set; } = null!;
        public string Item { get; set; } = null!;
        public string ZpTransId { get; set; } = null!;
        public long ServerTime { get; set; }
        public int Channel { get; set; }
        public string MerchantUserId { get; set; } = null!;
        public long UserFeeAmount { get; set; }
        public long DiscountAmount { get; set; }
    }
}
