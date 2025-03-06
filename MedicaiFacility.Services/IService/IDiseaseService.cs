﻿using MedicaiFacility.BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicaiFacility.Service.IService
{
    public interface IDiseaseService
    {
        List<Disease> GetAllDisease();
        Disease FindById(int id);
        void AddDisease(Disease disease);
        void UpdateDisease(Disease disease);
        (List<Disease>, int totalItem) FindAllWithPagination(int pg, int pageSize);
    }
}
