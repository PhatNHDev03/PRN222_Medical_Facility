﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace MedicaiFacility.BusinessObject;

public partial class Appointment
{
    public int AppointmentId { get; set; }

    public int? PatientId { get; set; }

    public int? ExpertId { get; set; }

    public int? FacilityId { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string Status { get; set; }

    public int? TransactionId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual MedicalExpert Expert { get; set; }

    public virtual MedicalFacility Facility { get; set; }

    public virtual ICollection<HealthRecord> HealthRecords { get; set; } = new List<HealthRecord>();

    public virtual MedicalHistory MedicalHistory { get; set; }

    public virtual Patient Patient { get; set; }

    public virtual RatingsAndFeedback RatingsAndFeedback { get; set; }

    public virtual Transaction Transaction { get; set; }
}