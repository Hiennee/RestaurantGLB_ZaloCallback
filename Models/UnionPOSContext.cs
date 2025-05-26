using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ZaloPayCallbackAPI.Models
{
    public partial class UnionPOSContext : DbContext
    {
        public UnionPOSContext()
        {
        }

        public UnionPOSContext(DbContextOptions<UnionPOSContext> options)
            : base(options)
        {
        }

        public virtual DbSet<MerchantAccountsZalopay> MerchantAccountsZalopays { get; set; } = null!;
        public virtual DbSet<ZaloPayCallbackDetail> ZaloPayCallbackDetails { get; set; } = null!;

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

                entity.Property(e => e.StoreCode)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("StoreCode");

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

            modelBuilder.Entity<ZaloPayCallbackDetail>(entity =>
            {
                entity.HasKey(e => e.AppTransId);

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
