﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataModelLibrary.Models;

public enum PeriodType
{
    OneOff = 1,
    Monthly = 2
}

public enum StatusType
{
    Scheduled = 1,
    InsufficientBalance = 2,
    Fail = 3
}

public class BillPay
{
    public int BillPayID { get; set; }

    [ForeignKey("Account")]
    public int AccountNumber { get; set; }
    public virtual Account Account { get; set; }

    [ForeignKey("Payee")]
    public int PayeeID { get; set; }
    public virtual Payee Payee { get; set; }

    [Column(TypeName = "money")]
    [Range(double.Epsilon, double.MaxValue, ErrorMessage = "The amount must be a positive number.")]
    public decimal Amount { get; set; }

    public DateTime ScheduleTimeUtc { get; set; }

    public PeriodType Period { get; set; }

    public StatusType Status { get; set; }


}

