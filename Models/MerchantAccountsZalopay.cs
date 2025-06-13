using System;
using System.Collections.Generic;

namespace RestaurantGLB_Webserver.Models
{
    public partial class MerchantAccountsZalopay
    {
        public int UserId { get; set; }
        public string StoreCode { get; set; } = null!;
        public string AppId { get; set; } = null!;
        public string Key1 { get; set; } = null!;
        public string Key2 { get; set; } = null!;
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
