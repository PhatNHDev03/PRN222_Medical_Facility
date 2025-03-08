﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace MedicaiFacility.BusinessObject;

public partial class HealthRecord
{
    public int RecordId { get; set; }

    public int? PatientId { get; set; }

    public int? UploadedBy { get; set; }

    public string FileName { get; set; }

    public string FilePath { get; set; }

    public string TestResult { get; set; }

    public string Diagnosis { get; set; }

    public string Prescription { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string SharedLink { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<HealthRecordDisease> HealthRecordDiseases { get; set; } = new List<HealthRecordDisease>();

    public virtual Patient Patient { get; set; }

    public virtual MedicalExpert UploadedByNavigation { get; set; }
}