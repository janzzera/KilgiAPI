using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace KilgiAPI.Models;

[Table("customers")]
[Index("UserId", Name = "idx_customers_user_id")]
public partial class Customer
{
    [Key]
    [Column("customer_id")]
    [StringLength(50)]
    public string CustomerId { get; set; } = null!;

    [Column("user_id")]
    [StringLength(50)]
    public string UserId { get; set; } = null!;

    [Column("display_name")]
    [StringLength(255)]
    public string DisplayName { get; set; } = null!;

    [Column("contact_number")]
    [StringLength(50)]
    public string? ContactNumber { get; set; }

    [Column("address")]
    [StringLength(500)]
    public string? Address { get; set; }

    [Column("notes")]
    public string? Notes { get; set; }

    [Column("is_active")]
    public int IsActive { get; set; }

    [Column("created_at")]
    public long CreatedAt { get; set; }

    [Column("updated_at")]
    public long UpdatedAt { get; set; }

    [InverseProperty("Customer")]
    public virtual ICollection<CustomerPayment> CustomerPayments { get; set; } = new List<CustomerPayment>();

    [ForeignKey("UserId")]
    [InverseProperty("Customers")]
    public virtual User User { get; set; } = null!;

    [InverseProperty("Customer")]
    public virtual ICollection<WholesaleInvoice> WholesaleInvoices { get; set; } = new List<WholesaleInvoice>();
}
