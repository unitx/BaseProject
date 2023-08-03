﻿using Core.Data.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace Core.Data.Entities
{
    public class Shift : HasIdDate
    {
        public string? ShiftCode { get; set; }
        public string? ShiftName { get; set; }
    }
}
