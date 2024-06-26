﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataModelLibrary.Models;

public enum TransactionType
{
    Deposit = 1,
    Withdraw = 2,
    Transfer = 3,
    ServiceCharge = 4,
    BillPay = 5,
    IncomingTransfer = 6
}

public static class ServiceFee
{
    public const decimal Withdraw = 0.05m;
    public const decimal Transfer = 0.1m;
}

public class Transaction
{
    public int TransactionID { get; set; }

    public TransactionType TransactionType { get; set; }

    [ForeignKey("Account")]
    public int AccountNumber { get; set; }
    public virtual Account Account { get; set; }

    [ForeignKey("DestinationAccount")]
    public int? DestinationAccountNumber { get; set; }
    public virtual Account DestinationAccount { get; set; }

    [Column(TypeName = "money")]
    [Range(double.Epsilon, double.MaxValue, ErrorMessage = "The amount must be a positive number.")]
    public decimal Amount { get; set; }

    [StringLength(30, ErrorMessage = "The comment field must be less than 30 characters.")]
    public string Comment { get; set; }

    public DateTime TransactionTimeUtc { get; set; }
}
