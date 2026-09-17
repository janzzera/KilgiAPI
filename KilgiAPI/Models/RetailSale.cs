using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace KilgiAPI.Models;

[Table("retail_sales")]
[Index("UserId", Name = "idx_retail_sales_user_id")]
public partial class RetailSale
{
    [Key]
    [Column("sale_id")]
    [StringLength(50)]
    public string SaleId { get; set; } = null!;

    [Column("user_id")]
    [StringLength(50)]
    public string UserId { get; set; } = null!;

    [Column("total_amount")]
    public double TotalAmount { get; set; }

    [Column("notes")]
    public string? Notes { get; set; }

    [Column("timestamp")]
    public long Timestamp { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("RetailSales")]
    public virtual User User { get; set; } = null!;
}
