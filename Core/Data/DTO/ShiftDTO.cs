﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.DTO
{
    public class ShiftDTO
    {
        public int? ShiftId { get; set; }
        public string? Shift { get; set; }
        public string? Description { get; set; }
        public string? ShiftCode { get; set; }
        public bool IsActive { get; set; }
    }
}
