using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace KilgiAPI.Models;

[Table("wholesale_invoices")]
[Index("CustomerId", "Timestamp", Name = "idx_wholesale_invoices_cust_time")]
[Index("CustomerId", Name = "idx_wholesale_invoices_customer_id")]
[Index("UserId", Name = "idx_wholesale_invoices_user_id")]
public partial class WholesaleInvoice
{
    [Key]
    [Column("invoice_id")]
    [StringLength(50)]
    public string InvoiceId { get; set; } = null!;

    [Column("user_id")]
    [StringLength(50)]
    public string UserId { get; set; } = null!;

    [Column("customer_id")]
    [StringLength(50)]
    public string CustomerId { get; set; } = null!;

    [Column("invoice_number")]
    [StringLength(100)]
    public string InvoiceNumber { get; set; } = null!;

    [Column("description")]
    [StringLength(500)]
    public string Description { get; set; } = null!;

    [Column("total_amount")]
    public double TotalAmount { get; set; }

    [Column("notes")]
    public string? Notes { get; set; }

    [Column("timestamp")]
    public long Timestamp { get; set; }

    [ForeignKey("CustomerId")]
    [InverseProperty("WholesaleInvoices")]
    public virtual Customer Customer { get; set; } = null!;

    [InverseProperty("Invoice")]
    public virtual ICollection<CustomerPaymentAllocation> CustomerPaymentAllocations { get; set; } = new List<CustomerPaymentAllocation>();

    [ForeignKey("UserId")]
    [InverseProperty("WholesaleInvoices")]
    public virtual User User { get; set; } = null!;
}
