﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace MedicaiFacility.BusinessObject;

public partial class Department
{
    public int DepartmentId { get; set; }

    public string DepartmentName { get; set; }

    public string Description { get; set; }

    public virtual ICollection<Disease> Diseases { get; set; } = new List<Disease>();

    public virtual ICollection<FacilityDepartment> FacilityDepartments { get; set; } = new List<FacilityDepartment>();
}