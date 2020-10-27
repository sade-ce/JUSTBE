using RealEstate.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.ViewModels
{
    public class BuildingViewModel
    {
        public UserProfileView UserProfile { get; set; }
        public Buildings Building { get; set; }
        public BuildingView BuildingViews { get; set; }
        public IEnumerable<BuildingView> BuildingList { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public BuildingPaging BuildingPaging { get; set; }
        public List<BuildingAttachments1> BuildingAttachmentsDropDown { get; set; }
        public List<Rules1> RulesDropDown { get; set; }
        public IEnumerable<SearchAutoCompleteViewModel> ListSearchAutoComplete { get; set; }
        public IEnumerable<BuildingVendorView> BuildingVendorList { get; set; }
        public IEnumerable<NeighborhoodDropDown> NeighborhoodDDL { get; set; }
        public IEnumerable<GalleryView> Gallery { get; set; }
        public IEnumerable<BuildingDocuments> BuildingDocuments { get; set; }
        public DealBuildingView DealBuilding { get; set; }
        public IEnumerable<BuildingDropdown> BuildingDropdown { get; set; }
    }
}
