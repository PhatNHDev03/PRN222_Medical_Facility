﻿using MedicaiFacility.BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicaiFacility.DataAccess.IRepostory
{
    public interface IMedicalFacilityRepository
    {
        List<MedicalFacility> GetAllMedicalFacility();
        MedicalFacility FindById(int id);
        void AddMedicalFacility(MedicalFacility medicalFacility);
        void UpdateMedicalFacility(MedicalFacility medicalFacility);
        void DeleteMedicalFacility(int id);
        (List<MedicalFacility>, int totalItem) FindAllWithPagination(int pg, int pageSize);
    }
}
