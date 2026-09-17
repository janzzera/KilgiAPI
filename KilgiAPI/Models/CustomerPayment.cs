using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace KilgiAPI.Models;

[Table("customer_payments")]
[Index("CustomerId", Name = "idx_customer_payments_customer_id")]
[Index("UserId", Name = "idx_customer_payments_user_id")]
public partial class CustomerPayment
{
    [Key]
    [Column("payment_id")]
    [StringLength(50)]
    public string PaymentId { get; set; } = null!;

    [Column("user_id")]
    [StringLength(50)]
    public string UserId { get; set; } = null!;

    [Column("customer_id")]
    [StringLength(50)]
    public string CustomerId { get; set; } = null!;

    [Column("total_amount")]
    public double TotalAmount { get; set; }

    [Column("notes")]
    public string? Notes { get; set; }

    [Column("timestamp")]
    public long Timestamp { get; set; }

    [ForeignKey("CustomerId")]
    [InverseProperty("CustomerPayments")]
    public virtual Customer Customer { get; set; } = null!;

    [InverseProperty("Payment")]
    public virtual ICollection<CustomerPaymentAllocation> CustomerPaymentAllocations { get; set; } = new List<CustomerPaymentAllocation>();

    [ForeignKey("UserId")]
    [InverseProperty("CustomerPayments")]
    public virtual User User { get; set; } = null!;
}
