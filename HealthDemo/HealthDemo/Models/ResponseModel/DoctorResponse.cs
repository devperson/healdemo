﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthDemo.Models.ResponseModel
{
    public class DoctorResponse: ResponseBase
    {
        public List<Doctor> Result { get; set; }
    }
}
