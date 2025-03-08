﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace MedicaiFacility.BusinessObject;

public partial class User
{
    public int UserId { get; set; }

    public string FullName { get; set; }

    public string Image { get; set; }

    public string Email { get; set; }

    public string PhoneNumber { get; set; }

    public string Password { get; set; }

    public string UserType { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string BankAccount { get; set; }

    public bool? Status { get; set; }

    public virtual ICollection<Conversation> Conversations { get; set; } = new List<Conversation>();

    public virtual ICollection<MedicalExpert> MedicalExperts { get; set; } = new List<MedicalExpert>();

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

    public virtual ICollection<Patient> Patients { get; set; } = new List<Patient>();

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}