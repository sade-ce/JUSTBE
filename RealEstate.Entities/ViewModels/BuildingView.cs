using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.ViewModels
{
    public class BuildingView
    {
      
        public int BuildingId { get; set; }
        public string Name { get; set; }

        public int Neighborhood_Id { get; set; }


        public string Address { get; set; }

        public int NumberOfUnits { get; set; }
        public string FrontDeskPhone { get; set; }
        
        public string Vendors { get; set; }
        public int BuildingAttachment_Id { get; set; }
        public int Rule_Id { get; set; }
        //public string BuildingAttachments { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string UserType { get; set; }
        public string Image { get; set; }
        public string Neighborhood { get; set; }
        public int? TransactionBuildingId { get; set; }
    }
    public class BuildingPaging
    {
        public string PreviousID { get; set; }
        public string Previous { get; set; }
        public string NextID { get; set; }
        public string Next { get; set; }

    }
    public class BuildingAttachments1
    {
        public int BuildingAttachment_Id { get; set; }
        public string Name { get; set; }
    }
    public class Rules1
    {
        public int Rule_Id { get; set; }
        public string Name { get; set; }
        public bool IsChecked { get; set; }
    }

    public class BuildingDropdown
    {
        public int BuildingId { get; set; }
        public string Name { get; set; }
    }

    public class BuildingDocuments
    {
        public int BuildingAttachmentId { get; set; }
        public int Building_Id { get; set; }
        public int BuildingAttachment_Id { get; set; }
        public string AttachmentFile { get; set; }
        public string DocumentName { get; set; }
        public string CreatedBy { get; set; }

    }

    public class NeighborhoodDropDown
    {
        public int StateID { get; set; }
        public int Neighborhood_Id { get; set; }
        public string Name { get; set; }
    }

    public class GalleryView
    {
        public int BuildingGalleryId { get; set; }
        public int Building_Id { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
    }

    public class DealBuildingView
    {
        public int TransactionBuildingId { get; set; }
        public int Building_Id { get; set; }
        public string Transaction_ID { get; set; }
        public string CreatedBy { get; set; }
        public string Email { get; set; }

    }
    
}
