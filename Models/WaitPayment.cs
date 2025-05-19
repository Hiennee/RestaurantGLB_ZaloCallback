using System;
using System.Collections.Generic;

namespace ZaloPayCallbackAPI.Models
{
    public partial class WaitPayment
    {
        public string TransactionUuid { get; set; } = null!;
        public int BillNo { get; set; }
        public decimal? DepositAmt { get; set; }
        public string? MotherAccntNo { get; set; }
        public string? MotherAccntOwner { get; set; }
        public string? EcollectionCd { get; set; }
        public DateTime? SaleDate { get; set; }
        public string? Status { get; set; }
        public int? BillSeq { get; set; }
        public string? CdShop { get; set; }
        public string? PosNo { get; set; }
        public int? PosCreate { get; set; }
        public DateTime? ModifileDate { get; set; }
        public DateTime? InsertDate { get; set; }
        public string? StatusOld { get; set; }
        public string? QrData { get; set; }
    }
}
