﻿using MedicaiFacility.BusinessObject;
using MedicaiFacility.DataAccess.IRepostory;
using MedicaiFacility.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicaiFacility.Service
{
    internal class MedicalExpertService : IMedicalExpertService
    {
        private readonly IMedicalExpertRepository _medicalExpertRepository;
        public MedicalExpertService(IMedicalExpertRepository medicalExpertRepository)
        {
            _medicalExpertRepository = medicalExpertRepository;
        }

        public MedicalExpert getById(int id)
        {
           return _medicalExpertRepository.getById(id);
        }
    }
}
