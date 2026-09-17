using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace KilgiAPI.Models;

[Table("batch_expenses")]
[Index("LotId", Name = "idx_batch_expenses_lot_id")]
public partial class BatchExpense
{
    [Key]
    [Column("expense_id")]
    [StringLength(50)]
    public string ExpenseId { get; set; } = null!;

    [Column("lot_id")]
    [StringLength(50)]
    public string LotId { get; set; } = null!;

    [Column("expense_account_code")]
    [StringLength(100)]
    public string ExpenseAccountCode { get; set; } = null!;

    [Column("expense_account_name")]
    [StringLength(255)]
    public string ExpenseAccountName { get; set; } = null!;

    [Column("amount")]
    public double Amount { get; set; }

    [Column("payment_source")]
    [StringLength(100)]
    public string PaymentSource { get; set; } = null!;

    [Column("timestamp")]
    public long Timestamp { get; set; }

    [ForeignKey("LotId")]
    [InverseProperty("BatchExpenses")]
    public virtual Lot Lot { get; set; } = null!;
}
