﻿using Core.Data.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace Core.Data.Entities
{
    public class Category:HasIdDate
    {
        public Category()
        {
            Emails = new HashSet<Email>();
        }
        public string? Name { get; set; }
        [JsonIgnore]
        public virtual ICollection<Email> Emails { get; set; }
    }
}
