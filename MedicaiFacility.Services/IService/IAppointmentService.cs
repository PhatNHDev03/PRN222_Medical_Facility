﻿using MedicaiFacility.BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicaiFacility.Service.IService
{
	public interface IAppointmentService
	{
		List<Appointment> GetAll();
		Appointment GetById(int id);
		void Create(Appointment appointment);
		void Update(Appointment appointment);
		void DeleteById(int id);
		(List<Appointment> list, int totalItems) GetALlPagainations(int pg, int pageSize);
		(List<Appointment> list, int totalItems) GetALlPagainationsByPatientId(int pg, int pageSize, int patientId);
		(List<Appointment> list, int totalItems) GetALlPagainationsByExpertId(int pg, int pageSize, int expertId);
		List<Appointment> GetAllByExpertId(int expertId);
    }
}
