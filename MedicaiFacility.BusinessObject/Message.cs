﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace MedicaiFacility.BusinessObject;

public partial class Message
{
    public int MessageId { get; set; }

    public int? ConversationId { get; set; }

    public int? SenderId { get; set; }

    public string MessageText { get; set; }

    public DateTime? SentAt { get; set; }

    public virtual Conversation Conversation { get; set; }

    public virtual User Sender { get; set; }
}