﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.Models
{
    public class ResourceType
    {
        [Key]
        public int TypeId { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}