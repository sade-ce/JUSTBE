﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RealEstate.Entities.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace RealEstate.Entities.ViewModels
{
    public class RulesViewModel
    {
        public Rules Rules { get; set; }
        public RulesModel RulesModel { get; set; }
        public IEnumerable<RulesModel> RulesList { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public RulesPaging RulesPaging { get; set; }
        public IEnumerable<SearchAutoCompleteViewModel> ListSearchAutoComplete { get; set; }
  
    }

    public class RulesModel
    {
        public int RuleId { get; set; }
        [Required(ErrorMessage = "Name is required")]
        [DisplayName("Name")]
        public string Name { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public string UserRole { get; set; }
        //public string Updatedby { get; set; }
        //public DateTime UpdatedOn { get; set; }

    }
    public class RulesPaging
    {
        public string PreviousID { get; set; }
        public string Previous { get; set; }
        public string NextID { get; set; }
        public string Next { get; set; }

    }
}
