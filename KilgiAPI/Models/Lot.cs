using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace KilgiAPI.Models;

[Table("lots")]
[Index("ProviderId", Name = "idx_lots_provider_id")]
[Index("UserId", Name = "idx_lots_user_id")]
public partial class Lot
{
    [Key]
    [Column("lot_id")]
    [StringLength(50)]
    public string LotId { get; set; } = null!;

    [Column("user_id")]
    [StringLength(50)]
    public string UserId { get; set; } = null!;

    [Column("provider_id")]
    [StringLength(50)]
    public string ProviderId { get; set; } = null!;

    [Column("provider_name")]
    [StringLength(255)]
    public string ProviderName { get; set; } = null!;

    [Column("vegetable_type")]
    [StringLength(100)]
    public string VegetableType { get; set; } = null!;

    [Column("total_sacks_purchased")]
    public int TotalSacksPurchased { get; set; }

    [Column("raw_kilos_received")]
    public double RawKilosReceived { get; set; }

    [Column("base_unit_price")]
    public double BaseUnitPrice { get; set; }

    [Column("purchase_payment_source")]
    [StringLength(100)]
    public string PurchasePaymentSource { get; set; } = null!;

    [Column("standard_freight")]
    public double StandardFreight { get; set; }

    [Column("freight_payment_source")]
    [StringLength(100)]
    public string FreightPaymentSource { get; set; } = null!;

    [Column("timestamp")]
    public long Timestamp { get; set; }

    [InverseProperty("Lot")]
    public virtual ICollection<BatchExpense> BatchExpenses { get; set; } = new List<BatchExpense>();

    [InverseProperty("Lot")]
    public virtual ICollection<JournalEntry> JournalEntries { get; set; } = new List<JournalEntry>();

    [ForeignKey("ProviderId")]
    [InverseProperty("Lots")]
    public virtual Provider Provider { get; set; } = null!;

    [InverseProperty("Lot")]
    public virtual ICollection<ProviderPaymentAllocation> ProviderPaymentAllocations { get; set; } = new List<ProviderPaymentAllocation>();

    [InverseProperty("Lot")]
    public virtual ICollection<SpoilageLog> SpoilageLogs { get; set; } = new List<SpoilageLog>();

    [ForeignKey("UserId")]
    [InverseProperty("Lots")]
    public virtual User User { get; set; } = null!;
}
