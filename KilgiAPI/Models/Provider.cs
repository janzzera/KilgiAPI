using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace KilgiAPI.Models;

[Table("providers")]
[Index("UserId", Name = "idx_providers_user_id")]
[Index("UserId", "DisplayName", Name = "idx_providers_user_name")]
public partial class Provider
{
    [Key]
    [Column("provider_id")]
    [StringLength(50)]
    public string ProviderId { get; set; } = null!;

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

    [InverseProperty("Provider")]
    public virtual ICollection<Lot> Lots { get; set; } = new List<Lot>();

    [InverseProperty("Provider")]
    public virtual ICollection<ProviderPayment> ProviderPayments { get; set; } = new List<ProviderPayment>();

    [ForeignKey("UserId")]
    [InverseProperty("Providers")]
    public virtual User User { get; set; } = null!;
}
