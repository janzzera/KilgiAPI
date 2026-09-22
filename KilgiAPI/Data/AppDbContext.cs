using KilgiAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace KilgiAPI.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AccountingPeriod> AccountingPeriods { get; set; }

    public virtual DbSet<BatchExpense> BatchExpenses { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<CustomerPayment> CustomerPayments { get; set; }

    public virtual DbSet<CustomerPaymentAllocation> CustomerPaymentAllocations { get; set; }

    public virtual DbSet<JournalEntry> JournalEntries { get; set; }

    public virtual DbSet<JournalLine> JournalLines { get; set; }

    public virtual DbSet<Lot> Lots { get; set; }

    public virtual DbSet<Provider> Providers { get; set; }

    public virtual DbSet<ProviderPayment> ProviderPayments { get; set; }

    public virtual DbSet<ProviderPaymentAllocation> ProviderPaymentAllocations { get; set; }

    public virtual DbSet<RetailSale> RetailSales { get; set; }

    public virtual DbSet<SpoilageLog> SpoilageLogs { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<WholesaleInvoice> WholesaleInvoices { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Name = ConnectionStrings:DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasPostgresEnum("auth", "aal_level", new[] { "aal1", "aal2", "aal3" })
            .HasPostgresEnum("auth", "code_challenge_method", new[] { "s256", "plain" })
            .HasPostgresEnum("auth", "factor_status", new[] { "unverified", "verified" })
            .HasPostgresEnum("auth", "factor_type", new[] { "totp", "webauthn", "phone", "recovery_code" })
            .HasPostgresEnum("auth", "oauth_authorization_status", new[] { "pending", "approved", "denied", "expired" })
            .HasPostgresEnum("auth", "oauth_client_type", new[] { "public", "confidential" })
            .HasPostgresEnum("auth", "oauth_registration_type", new[] { "dynamic", "manual" })
            .HasPostgresEnum("auth", "oauth_response_type", new[] { "code" })
            .HasPostgresEnum("auth", "one_time_token_type", new[] { "confirmation_token", "reauthentication_token", "recovery_token", "email_change_token_new", "email_change_token_current", "phone_change_token" })
            .HasPostgresEnum("realtime", "action", new[] { "INSERT", "UPDATE", "DELETE", "TRUNCATE", "ERROR" })
            .HasPostgresEnum("realtime", "equality_op", new[] { "eq", "neq", "lt", "lte", "gt", "gte", "in", "like", "ilike", "is", "match", "imatch", "isdistinct" })
            .HasPostgresEnum("storage", "buckettype", new[] { "STANDARD", "ANALYTICS", "VECTOR" })
            .HasPostgresExtension("extensions", "pg_stat_statements")
            .HasPostgresExtension("extensions", "pgcrypto")
            .HasPostgresExtension("extensions", "uuid-ossp")
            .HasPostgresExtension("vault", "supabase_vault");

        modelBuilder.Entity<AccountingPeriod>(entity =>
        {
            entity.HasKey(e => e.PeriodId).HasName("accounting_periods_pkey");

            entity.Property(e => e.IsClosed).HasDefaultValue(false);

            entity.HasOne(d => d.User).WithMany(p => p.AccountingPeriods)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("accounting_periods_user_id_fkey");
        });

        modelBuilder.Entity<BatchExpense>(entity =>
        {
            entity.HasKey(e => e.ExpenseId).HasName("batch_expenses_pkey");

            entity.HasOne(d => d.Lot).WithMany(p => p.BatchExpenses).HasConstraintName("batch_expenses_lot_id_fkey");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("customers_pkey");

            entity.Property(e => e.IsActive).HasDefaultValue(1);

            entity.HasOne(d => d.User).WithMany(p => p.Customers).HasConstraintName("customers_user_id_fkey");
        });

        modelBuilder.Entity<CustomerPayment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("customer_payments_pkey");

            entity.HasOne(d => d.Customer).WithMany(p => p.CustomerPayments).HasConstraintName("customer_payments_customer_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.CustomerPayments).HasConstraintName("customer_payments_user_id_fkey");
        });

        modelBuilder.Entity<CustomerPaymentAllocation>(entity =>
        {
            entity.HasKey(e => e.AllocationId).HasName("customer_payment_allocations_pkey");

            entity.HasOne(d => d.Invoice).WithMany(p => p.CustomerPaymentAllocations).HasConstraintName("customer_payment_allocations_invoice_id_fkey");

            entity.HasOne(d => d.Payment).WithMany(p => p.CustomerPaymentAllocations).HasConstraintName("customer_payment_allocations_payment_id_fkey");
        });

        modelBuilder.Entity<JournalEntry>(entity =>
        {
            entity.HasKey(e => e.EntryId).HasName("journal_entries_pkey");

            entity.HasOne(d => d.Lot).WithMany(p => p.JournalEntries)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("journal_entries_lot_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.JournalEntries).HasConstraintName("journal_entries_user_id_fkey");
        });

        modelBuilder.Entity<JournalLine>(entity =>
        {
            entity.HasKey(e => e.LineId).HasName("journal_lines_pkey");

            entity.HasOne(d => d.Entry).WithMany(p => p.JournalLines).HasConstraintName("journal_lines_entry_id_fkey");
        });

        modelBuilder.Entity<Lot>(entity =>
        {
            entity.HasKey(e => e.LotId).HasName("lots_pkey");

            entity.HasOne(d => d.Provider).WithMany(p => p.Lots)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("lots_provider_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Lots).HasConstraintName("lots_user_id_fkey");
        });

        modelBuilder.Entity<Provider>(entity =>
        {
            entity.HasKey(e => e.ProviderId).HasName("providers_pkey");

            entity.Property(e => e.IsActive).HasDefaultValue(1);

            entity.HasOne(d => d.User).WithMany(p => p.Providers).HasConstraintName("providers_user_id_fkey");
        });

        modelBuilder.Entity<ProviderPayment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("provider_payments_pkey");

            entity.HasOne(d => d.Provider).WithMany(p => p.ProviderPayments).HasConstraintName("provider_payments_provider_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.ProviderPayments).HasConstraintName("provider_payments_user_id_fkey");
        });

        modelBuilder.Entity<ProviderPaymentAllocation>(entity =>
        {
            entity.HasKey(e => e.AllocationId).HasName("provider_payment_allocations_pkey");

            entity.HasOne(d => d.Lot).WithMany(p => p.ProviderPaymentAllocations).HasConstraintName("provider_payment_allocations_lot_id_fkey");

            entity.HasOne(d => d.Payment).WithMany(p => p.ProviderPaymentAllocations).HasConstraintName("provider_payment_allocations_payment_id_fkey");
        });

        modelBuilder.Entity<RetailSale>(entity =>
        {
            entity.HasKey(e => e.SaleId).HasName("retail_sales_pkey");

            entity.HasOne(d => d.User).WithMany(p => p.RetailSales).HasConstraintName("retail_sales_user_id_fkey");
        });

        modelBuilder.Entity<SpoilageLog>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("spoilage_logs_pkey");

            entity.HasOne(d => d.Lot).WithMany(p => p.SpoilageLogs).HasConstraintName("spoilage_logs_lot_id_fkey");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("users_pkey");
        });

        modelBuilder.Entity<WholesaleInvoice>(entity =>
        {
            entity.HasKey(e => e.InvoiceId).HasName("wholesale_invoices_pkey");

            entity.HasOne(d => d.Customer).WithMany(p => p.WholesaleInvoices).HasConstraintName("wholesale_invoices_customer_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.WholesaleInvoices).HasConstraintName("wholesale_invoices_user_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
