using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ZaloPayCallbackAPI.Models
{
    public partial class RestaurantGLBContext : DbContext
    {
        public RestaurantGLBContext()
        {
        }

        public RestaurantGLBContext(DbContextOptions<RestaurantGLBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<MerchantAccountsZalopay> MerchantAccountsZalopays { get; set; } = null!;
        public virtual DbSet<WaitPayment> WaitPayments { get; set; } = null!;
        public virtual DbSet<ZaloPayCallbackDetail> ZaloPayCallbackDetails { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=DESKTOP-Q5S0M34\\SQLSERVER;Database=RestaurantGLB;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("Korean_Wansung_CI_AS");

            modelBuilder.Entity<MerchantAccountsZalopay>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.ToTable("MERCHANT_ACCOUNTS_ZALOPAY");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.AppId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("APP_ID");

                entity.Property(e => e.CdShop)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CD_SHOP");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Key1)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("KEY1");

                entity.Property(e => e.Key2)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("KEY2");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<WaitPayment>(entity =>
            {
                entity.HasKey(e => e.TransactionUuid)
                    .HasName("PK__WAIT_PAY__4F344C1784B71738");

                entity.ToTable("WAIT_PAYMENT");

                entity.Property(e => e.TransactionUuid)
                    .HasMaxLength(200)
                    .HasColumnName("TRANSACTION_UUID");

                entity.Property(e => e.BillNo).HasColumnName("BILL_NO");

                entity.Property(e => e.BillSeq).HasColumnName("BILL_SEQ");

                entity.Property(e => e.CdShop)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("CD_SHOP");

                entity.Property(e => e.DepositAmt)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("DEPOSIT_AMT");

                entity.Property(e => e.EcollectionCd)
                    .HasMaxLength(30)
                    .HasColumnName("ECOLLECTION_CD");

                entity.Property(e => e.InsertDate)
                    .HasColumnType("datetime")
                    .HasColumnName("INSERT_DATE")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifileDate)
                    .HasColumnType("datetime")
                    .HasColumnName("MODIFILE_DATE")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.MotherAccntNo)
                    .HasMaxLength(255)
                    .HasColumnName("MOTHER_ACCNT_NO");

                entity.Property(e => e.MotherAccntOwner)
                    .HasMaxLength(255)
                    .HasColumnName("MOTHER_ACCNT_OWNER");

                entity.Property(e => e.PosCreate).HasColumnName("POS_CREATE");

                entity.Property(e => e.PosNo)
                    .HasMaxLength(10)
                    .HasColumnName("POS_NO");

                entity.Property(e => e.QrData).HasColumnName("QR_DATA");

                entity.Property(e => e.SaleDate)
                    .HasColumnType("date")
                    .HasColumnName("SALE_DATE");

                entity.Property(e => e.Status)
                    .HasMaxLength(10)
                    .HasColumnName("STATUS");

                entity.Property(e => e.StatusOld)
                    .HasMaxLength(10)
                    .HasColumnName("STATUS_OLD");
            });

            modelBuilder.Entity<ZaloPayCallbackDetail>(entity =>
            {
                entity.HasKey(e => e.AppTransId)
                    .HasName("PK__ZaloPay___362C36BB9085BE39");

                entity.ToTable("ZaloPay_Callback_Detail");

                entity.Property(e => e.AppTransId)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("APP_TRANS_ID");

                entity.Property(e => e.Amount).HasColumnName("AMOUNT");

                entity.Property(e => e.AppId).HasColumnName("APP_ID");

                entity.Property(e => e.AppTime).HasColumnName("APP_TIME");

                entity.Property(e => e.AppUser)
                    .IsUnicode(false)
                    .HasColumnName("APP_USER");

                entity.Property(e => e.Channel).HasColumnName("CHANNEL");

                entity.Property(e => e.DiscountAmount).HasColumnName("DISCOUNT_AMOUNT");

                entity.Property(e => e.EmbedData).HasColumnName("EMBED_DATA");

                entity.Property(e => e.Item).HasColumnName("ITEM");

                entity.Property(e => e.MerchantUserId)
                    .IsUnicode(false)
                    .HasColumnName("MERCHANT_USER_ID");

                entity.Property(e => e.ServerTime).HasColumnName("SERVER_TIME");

                entity.Property(e => e.UserFeeAmount).HasColumnName("USER_FEE_AMOUNT");

                entity.Property(e => e.ZpTransId)
                    .IsUnicode(false)
                    .HasColumnName("ZP_TRANS_ID");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
