using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace KilgiAPI.Models;

[Table("provider_payments")]
[Index("ProviderId", Name = "idx_provider_payments_provider_id")]
[Index("UserId", Name = "idx_provider_payments_user_id")]
public partial class ProviderPayment
{
    [Key]
    [Column("payment_id")]
    [StringLength(50)]
    public string PaymentId { get; set; } = null!;

    [Column("user_id")]
    [StringLength(50)]
    public string UserId { get; set; } = null!;

    [Column("provider_id")]
    [StringLength(50)]
    public string ProviderId { get; set; } = null!;

    [Column("total_amount")]
    public double TotalAmount { get; set; }

    [Column("notes")]
    public string? Notes { get; set; }

    [Column("timestamp")]
    public long Timestamp { get; set; }

    [ForeignKey("ProviderId")]
    [InverseProperty("ProviderPayments")]
    public virtual Provider Provider { get; set; } = null!;

    [InverseProperty("Payment")]
    public virtual ICollection<ProviderPaymentAllocation> ProviderPaymentAllocations { get; set; } = new List<ProviderPaymentAllocation>();

    [ForeignKey("UserId")]
    [InverseProperty("ProviderPayments")]
    public virtual User User { get; set; } = null!;
}
