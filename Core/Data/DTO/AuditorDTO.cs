﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.DTO
{
    public class AuditorDTO
    {
        public int Id { get; set; }
        public int? Empno { get; set; }
        public string? Name { get; set; }
        public bool IsActive { get; set; }
    }
}
