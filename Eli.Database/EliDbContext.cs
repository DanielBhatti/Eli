using System;
using System.Collections.Generic;
using Eli.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace Eli.Database;

public partial class EliDbContext : DbContext
{
    public EliDbContext(DbContextOptions<EliDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AaaTransaction> AaaTransactions { get; set; }

    public virtual DbSet<BoaTransaction> BoaTransactions { get; set; }

    public virtual DbSet<CapitalOneTransaction> CapitalOneTransactions { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<CitiTransaction> CitiTransactions { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<CreditCard> CreditCards { get; set; }

    public virtual DbSet<CreditCardTransaction> CreditCardTransactions { get; set; }

    public virtual DbSet<CreditCardTransactionCategory> CreditCardTransactionCategories { get; set; }

    public virtual DbSet<CreditCardTransactionLabel> CreditCardTransactionLabels { get; set; }

    public virtual DbSet<DiscoverTransaction> DiscoverTransactions { get; set; }

    public virtual DbSet<FidelityTransaction> FidelityTransactions { get; set; }

    public virtual DbSet<Label> Labels { get; set; }

    public virtual DbSet<PurchaseChannel> PurchaseChannels { get; set; }

    public virtual DbSet<TdTransaction> TdTransactions { get; set; }

    public virtual DbSet<UsBankTransaction> UsBankTransactions { get; set; }

    public virtual DbSet<ValleyBankTransaction> ValleyBankTransactions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AaaTransaction>(entity =>
        {
            entity.HasKey(e => e.AaaTransactionId).HasName("aaa_transaction_pkey");

            entity.Property(e => e.AaaTransactionId).HasDefaultValueSql("uuidv7()");
        });

        modelBuilder.Entity<BoaTransaction>(entity =>
        {
            entity.HasKey(e => e.BoaTransactionId).HasName("boa_transaction_pkey");

            entity.Property(e => e.BoaTransactionId).HasDefaultValueSql("uuidv7()");
        });

        modelBuilder.Entity<CapitalOneTransaction>(entity =>
        {
            entity.HasKey(e => e.CapitalOneTransactionId).HasName("capital_one_transaction_pkey");

            entity.Property(e => e.CapitalOneTransactionId).HasDefaultValueSql("uuidv7()");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("category_pkey");

            entity.Property(e => e.CategoryId).HasDefaultValueSql("uuidv7()");
        });

        modelBuilder.Entity<CitiTransaction>(entity =>
        {
            entity.HasKey(e => e.CitiTransactionId).HasName("citi_transaction_pkey");

            entity.Property(e => e.CitiTransactionId).HasDefaultValueSql("uuidv7()");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.CountryId).HasName("country_pkey");

            entity.Property(e => e.CountryId).HasDefaultValueSql("uuidv7()");
            entity.Property(e => e.IsoCode).IsFixedLength();
            entity.Property(e => e.IsoCode3).IsFixedLength();
        });

        modelBuilder.Entity<CreditCard>(entity =>
        {
            entity.HasKey(e => e.CreditCardId).HasName("credit_card_pkey");

            entity.Property(e => e.CreditCardId).HasDefaultValueSql("uuidv7()");
        });

        modelBuilder.Entity<CreditCardTransaction>(entity =>
        {
            entity.HasKey(e => e.CreditCardTransactionId).HasName("credit_card_transaction_pkey");

            entity.Property(e => e.CreditCardTransactionId).HasDefaultValueSql("uuidv7()");

            entity.HasOne(d => d.CreditCard).WithMany(p => p.CreditCardTransactions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("credit_card_transaction_credit_card_id_fkey");

            entity.HasOne(d => d.PurchaseChannel).WithMany(p => p.CreditCardTransactions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("credit_card_transaction_purchase_channel_id_fkey");
        });

        modelBuilder.Entity<CreditCardTransactionCategory>(entity =>
        {
            entity.HasKey(e => e.CreditCardTransactionCategoryId).HasName("credit_card_transaction_category_pkey");

            entity.Property(e => e.CreditCardTransactionCategoryId).HasDefaultValueSql("uuidv7()");

            entity.HasOne(d => d.Category).WithMany(p => p.CreditCardTransactionCategories)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("credit_card_transaction_category_category_id_fkey");

            entity.HasOne(d => d.Transaction).WithMany(p => p.CreditCardTransactionCategories)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("credit_card_transaction_category_transaction_id_fkey");
        });

        modelBuilder.Entity<CreditCardTransactionLabel>(entity =>
        {
            entity.HasKey(e => e.CreditCardTransactionLabelId).HasName("credit_card_transaction_label_pkey");

            entity.Property(e => e.CreditCardTransactionLabelId).HasDefaultValueSql("uuidv7()");

            entity.HasOne(d => d.Label).WithMany(p => p.CreditCardTransactionLabels)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("credit_card_transaction_label_label_id_fkey");

            entity.HasOne(d => d.Transaction).WithMany(p => p.CreditCardTransactionLabels)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("credit_card_transaction_label_transaction_id_fkey");
        });

        modelBuilder.Entity<DiscoverTransaction>(entity =>
        {
            entity.HasKey(e => e.DiscoverTransactionId).HasName("discover_transaction_pkey");

            entity.Property(e => e.DiscoverTransactionId).HasDefaultValueSql("uuidv7()");
        });

        modelBuilder.Entity<FidelityTransaction>(entity =>
        {
            entity.HasKey(e => e.FidelityTransactionId).HasName("fidelity_transaction_pkey");

            entity.Property(e => e.FidelityTransactionId).HasDefaultValueSql("uuidv7()");
        });

        modelBuilder.Entity<Label>(entity =>
        {
            entity.HasKey(e => e.LabelId).HasName("label_pkey");

            entity.Property(e => e.LabelId).HasDefaultValueSql("uuidv7()");
        });

        modelBuilder.Entity<PurchaseChannel>(entity =>
        {
            entity.HasKey(e => e.PurchaseChannelId).HasName("purchase_channel_pkey");

            entity.Property(e => e.PurchaseChannelId).HasDefaultValueSql("uuidv7()");
        });

        modelBuilder.Entity<TdTransaction>(entity =>
        {
            entity.HasKey(e => e.TdTransactionId).HasName("td_transaction_pkey");

            entity.Property(e => e.TdTransactionId).HasDefaultValueSql("uuidv7()");
        });

        modelBuilder.Entity<UsBankTransaction>(entity =>
        {
            entity.HasKey(e => e.UsBankTransactionId).HasName("us_bank_transaction_pkey");

            entity.Property(e => e.UsBankTransactionId).HasDefaultValueSql("uuidv7()");
        });

        modelBuilder.Entity<ValleyBankTransaction>(entity =>
        {
            entity.HasKey(e => e.ValleyBankTransactionId).HasName("valley_bank_transaction_pkey");

            entity.Property(e => e.ValleyBankTransactionId).HasDefaultValueSql("uuidv7()");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
