using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace KilgiAPI.Models;

[Table("customer_payment_allocations")]
[Index("InvoiceId", Name = "idx_cpa_invoice_id")]
[Index("PaymentId", Name = "idx_cpa_payment_id")]
public partial class CustomerPaymentAllocation
{
    [Key]
    [Column("allocation_id")]
    [StringLength(50)]
    public string AllocationId { get; set; } = null!;

    [Column("payment_id")]
    [StringLength(50)]
    public string PaymentId { get; set; } = null!;

    [Column("invoice_id")]
    [StringLength(50)]
    public string InvoiceId { get; set; } = null!;

    [Column("amount_applied")]
    public double AmountApplied { get; set; }

    [Column("allocation_order")]
    public int AllocationOrder { get; set; }

    [Column("timestamp")]
    public long Timestamp { get; set; }

    [ForeignKey("InvoiceId")]
    [InverseProperty("CustomerPaymentAllocations")]
    public virtual WholesaleInvoice Invoice { get; set; } = null!;

    [ForeignKey("PaymentId")]
    [InverseProperty("CustomerPaymentAllocations")]
    public virtual CustomerPayment Payment { get; set; } = null!;
}
