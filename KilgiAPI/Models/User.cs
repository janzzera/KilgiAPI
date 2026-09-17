using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace KilgiAPI.Models;

[Table("users")]
public partial class User
{
    [Key]
    [Column("user_id")]
    [StringLength(50)]
    public string UserId { get; set; } = null!;

    [Column("username")]
    [StringLength(255)]
    public string Username { get; set; } = null!;

    [Column("display_name")]
    [StringLength(255)]
    public string DisplayName { get; set; } = null!;

    [Column("business_name")]
    [StringLength(255)]
    public string BusinessName { get; set; } = null!;

    [Column("email_address")]
    [StringLength(255)]
    public string? EmailAddress { get; set; }

    [Column("mobile_number")]
    [StringLength(50)]
    public string? MobileNumber { get; set; }

    [Column("password_hash")]
    [StringLength(255)]
    public string PasswordHash { get; set; } = null!;

    [Column("password_salt")]
    [StringLength(255)]
    public string PasswordSalt { get; set; } = null!;

    [Column("account_status")]
    [StringLength(50)]
    public string AccountStatus { get; set; } = null!;

    [Column("created_at")]
    public long CreatedAt { get; set; }

    [Column("updated_at")]
    public long UpdatedAt { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<AccountingPeriod> AccountingPeriods { get; set; } = new List<AccountingPeriod>();

    [InverseProperty("User")]
    public virtual ICollection<CustomerPayment> CustomerPayments { get; set; } = new List<CustomerPayment>();

    [InverseProperty("User")]
    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();

    [InverseProperty("User")]
    public virtual ICollection<JournalEntry> JournalEntries { get; set; } = new List<JournalEntry>();

    [InverseProperty("User")]
    public virtual ICollection<Lot> Lots { get; set; } = new List<Lot>();

    [InverseProperty("User")]
    public virtual ICollection<ProviderPayment> ProviderPayments { get; set; } = new List<ProviderPayment>();

    [InverseProperty("User")]
    public virtual ICollection<Provider> Providers { get; set; } = new List<Provider>();

    [InverseProperty("User")]
    public virtual ICollection<RetailSale> RetailSales { get; set; } = new List<RetailSale>();

    [InverseProperty("User")]
    public virtual ICollection<WholesaleInvoice> WholesaleInvoices { get; set; } = new List<WholesaleInvoice>();
}
