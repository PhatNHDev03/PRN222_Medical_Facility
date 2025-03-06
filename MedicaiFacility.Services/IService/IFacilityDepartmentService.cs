﻿using MedicaiFacility.BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicaiFacility.Service.IService
{
    public interface IFacilityDepartmentService
    {
        List<FacilityDepartment> GetAllFacilityDepartment();
        FacilityDepartment FindById(int id);
        void AddFacilityDepartment(FacilityDepartment facilityDepartment);
        void UpdateFacilityDepartment(FacilityDepartment facilityDepartment);
        void DeleteFacilityDepartment(int id);
        (List<FacilityDepartment>, int totalItem) FindAllWithPagination(int pg, int pageSize);
    }
}
