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

    public virtual MedicalExpert MedicalExpert { get; set; }

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

    public virtual Patient Patient { get; set; }

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}