﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace MedicaiFacility.BusinessObject;

public partial class Transaction
{
    public int TransactionId { get; set; }

    public int? UserId { get; set; }

    public string PaymentMethod { get; set; }

    public decimal Amount { get; set; }

    public string TransactionStatus { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public string TransactionType { get; set; }

    public virtual Appointment Appointment { get; set; }

    public virtual User User { get; set; }
}