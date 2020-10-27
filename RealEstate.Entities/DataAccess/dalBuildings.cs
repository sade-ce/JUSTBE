using RealEstate.Entities.Models;
using RealEstate.Entities.Utility;
using RealEstate.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
namespace RealEstate.Entities.DataAccess
{
    public class dalBuildings
    {
        public EFDBContext objDB = new EFDBContext();
        RealEstateUtility objUtil = new RealEstateUtility();

        public IEnumerable<BuildingView> BuildingGetPaged(int PageNo, int PageSize, out int TotalRows, string SearchTerm = "")
        {
            List<BuildingView> objList = new List<BuildingView>();
            if (SearchTerm != "")
            {
                SearchTerm = Regex.Replace(SearchTerm, @"\s+", " ");
            }
            var parStart = new SqlParameter("@Start", (PageNo - 1) * PageSize);
            var parPageSize = new SqlParameter("@PageSize", PageSize);
            var parSearchTerm = new SqlParameter("@SearchTerm", SearchTerm);
            var spOutput = new SqlParameter
            {
                ParameterName = "@TotalCount",
                SqlDbType = System.Data.SqlDbType.Int,
                Direction = System.Data.ParameterDirection.Output
            };
            objList = objDB.Database.SqlQuery<BuildingView>("udspBuildingGetPaged @Start,@PageSize,@SearchTerm,@TotalCount out", parStart, parPageSize, parSearchTerm, spOutput).ToList();
            TotalRows = int.Parse(spOutput.Value.ToString());
            return objList;
        }
        public string Save(Buildings model, List<Rules1> Rules, string CreatedBy)
        {
            SPErrorViewModel objStatus = new SPErrorViewModel();
            var parName = new SqlParameter("@Name", model.Name);
            var parNeighbourhoodId = new SqlParameter("@Neighborhood_Id", Convert.ToInt32(model.Neighborhood_Id));
            var parAddress = new SqlParameter("@Address", model.Address == null ? "" : model.Address);
            var parNumberOfUnits = new SqlParameter("@NumberOfUnits", Convert.ToInt32(model.NumberOfUnits));
            var parFrontDeskPhone = new SqlParameter("@FrontDeskPhone", model.FrontDeskPhone == null ? "" : model.FrontDeskPhone);
            var parVendors = new SqlParameter("@Vendors", model.Vendors == null ? "" : model.Vendors);
            

            var parCreatedBy = new SqlParameter("@CreatedBy", CreatedBy);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspBuildingInsert @Name,@Neighborhood_Id,@Address,@NumberOfUnits,@FrontDeskPhone,@Vendors,@CreatedBy", parName, parNeighbourhoodId, parAddress, parNumberOfUnits, parFrontDeskPhone, parVendors, parCreatedBy).FirstOrDefault();
            int BuildingId = 0;
            if (objStatus.ErrorCode == 0)
            {
                 BuildingId = Convert.ToInt32(objStatus.ErrorMessage);
                foreach (var c in Rules)
                {
                    if (c.IsChecked == true)
                    {
                        var parBuildingId = new SqlParameter("@Building_Id", BuildingId);
                        var parRule_Id = new SqlParameter("@Rule_Id", c.Rule_Id);
                        var parRuleCreatedBy = new SqlParameter("@CreatedBy", CreatedBy);
                        objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspBuildingRulesInsert @Building_Id,@Rule_Id,@CreatedBy", parBuildingId, parRule_Id, parRuleCreatedBy).FirstOrDefault();
                    }
                }
            }
            return objStatus.ErrorCode.ToString()+"_"+ BuildingId.ToString();
        }

        ///// <summary>
        ///// added by sonika
        ///// </summary>
        ///// <param name="model"></param>
        ///// <param name="CreatedBy"></param>
        ///// <param name="TransactionId"></param>
        ///// <returns></returns>
        //public string SaveClientBuildingClient(Buildings model, string CreatedBy, string TransactionId)
        //{
        //    DateTime dt = DateTime.Now;
        //    var CurrentYear = dt.ToString("yyyy");
        //    SPErrorViewModel objStatus = new SPErrorViewModel();
        //    model.BuildingId = CurrentYear + objUtil.generateUniqueCode("Building");
        //    var DealBuildingId = CurrentYear + objUtil.generateUniqueCode("DealBuilding");
        //    var BuildingCategoryId = CurrentYear + objUtil.generateUniqueCode("BuildingCategory");
        //    var parBuildingId = new SqlParameter("@BuildingId", model.BuildingId);
        //    var parTitle = new SqlParameter("@Title", model.Title);
        //    var parSubTitle = new SqlParameter("@SubTitle", model.SubTitle == null ? "" : model.SubTitle);
        //    var parPhone = new SqlParameter("@Phone", model.Phone == null ? "" : model.Phone);
        //    var parEmail = new SqlParameter("@Email", model.Email == null ? "" : model.Email);
        //    var parWeb = new SqlParameter("@WebsiteLink", model.WebsiteLink == null ? "" : model.WebsiteLink);
        //    var parAbout = new SqlParameter("@About", model.About == null ? "" : model.About);
        //    var parBuildingImage = new SqlParameter("@BuildingImage", DBNull.Value);
        //    var parLocation = new SqlParameter("@Location", model.Location == null ? "" : model.Location);
        //    if (model.BuildingImage != null)
        //        parBuildingImage = new SqlParameter("@BuildingImage", model.BuildingId.ToString() + model.BuildingImage.ToString());
        //    var parCreatedBy = new SqlParameter("@CreatedBy", CreatedBy);
        //    var parCategory_Id = new SqlParameter("@Category_Name", model.Category_Id);
        //    var parTransactionId = new SqlParameter("@TransactionId", TransactionId);
        //    var parDealBuildingId = new SqlParameter("@DealBuildingId", DealBuildingId);
        //    var parBuildingCategory = new SqlParameter("@BuildingCategoryId", BuildingCategoryId);
        //    objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspBuildingInsertClientBuildingClient @BuildingId,@Title,@SubTitle,@Category_Name,@Phone,@Email,@About,@BuildingImage,@Location,@CreatedBy,@WebsiteLink,@TransactionId,@DealBuildingId,@BuildingCategoryId", parBuildingId, parTitle, parSubTitle, parCategory_Id, parPhone, parEmail, parAbout, parBuildingImage, parLocation, parCreatedBy, parWeb, parTransactionId, parDealBuildingId, parBuildingCategory).FirstOrDefault();
        //    return objStatus.ErrorMessage;
        //}

        public string Edit(Buildings model, List<Rules1> Rules, string CreatedBy)
        {
            SPErrorViewModel objStatus = new SPErrorViewModel();
            var parBuildingId = new SqlParameter("@BuildingId", model.BuildingId);
            var parName = new SqlParameter("@Name", model.Name);
            var parNeighbourhoodId = new SqlParameter("@Neighborhood_Id", Convert.ToInt32(model.Neighborhood_Id));
            var parAddress = new SqlParameter("@Address", model.Address == null ? "" : model.Address);
            var parNumberOfUnits = new SqlParameter("@NumberOfUnits", Convert.ToInt32(model.NumberOfUnits));
            var parFrontDeskPhone = new SqlParameter("@FrontDeskPhone", model.FrontDeskPhone == null ? "" : model.FrontDeskPhone);
            var parVendors = new SqlParameter("@Vendors", model.Vendors == null ? "" : model.Vendors);
            var parCreatedBy = new SqlParameter("@UpdatedBy", CreatedBy);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspBuildingUpdate @BuildingId,@Name,@Neighborhood_Id,@Address,@NumberOfUnits,@FrontDeskPhone,@Vendors,@UpdatedBy", parBuildingId, parName, parNeighbourhoodId, parAddress, parNumberOfUnits, parFrontDeskPhone, parVendors, parCreatedBy).FirstOrDefault();
            objDB.Database.SqlQuery<Rules1>("select RuleId[Rule_Id],Name from Rules order by Name").ToList();
            DeleteBuildingRules(model.BuildingId);
            foreach (var c in Rules)
            {
                if (c.IsChecked == true)
                {
                    var parBuilding_Id = new SqlParameter("@Building_Id", model.BuildingId);
                    var parRule_Id = new SqlParameter("@Rule_Id", c.Rule_Id);
                    var parRuleCreatedBy = new SqlParameter("@CreatedBy", CreatedBy);
                    objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspBuildingRulesInsert @Building_Id,@Rule_Id,@CreatedBy", parBuilding_Id, parRule_Id, parRuleCreatedBy).FirstOrDefault();
                }
            }
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }

        ///// <summary>
        ///// added by sonika
        ///// </summary>
        ///// <returns></returns>
        ///// 
        //public string EditBuildingClient(Buildings model, string CreatedBy, string TransactionId, string DealBuildingId)
        //{
        //    DateTime dt = DateTime.Now;
        //    var CurrentYear = dt.ToString("yyyy");
        //    SPErrorViewModel objStatus = new SPErrorViewModel();
        //    var parBuildingId = new SqlParameter("@BuildingId", model.BuildingId);
        //    var BuildingCategoryId = CurrentYear + objUtil.generateUniqueCode("BuildingCategory");
        //    var parTitle = new SqlParameter("@Title", model.Title);
        //    var parSubTitle = new SqlParameter("@SubTitle", model.SubTitle == null ? "" : model.SubTitle);
        //    var parPhone = new SqlParameter("@Phone", model.Phone == null ? "" : model.Phone);
        //    var parEmail = new SqlParameter("@Email", model.Email == null ? "" : model.Email);
        //    var parWeb = new SqlParameter("@WebsiteLink", model.WebsiteLink == null ? "" : model.WebsiteLink);
        //    var parAbout = new SqlParameter("@About", model.About == null ? "" : model.About);
        //    var parCreatedBy = new SqlParameter("@CreatedBy", CreatedBy);
        //    var parCategory_Id = new SqlParameter("@Category_Name", model.Category_Id);
        //    var parBuildingImage = new SqlParameter("@BuildingImage", DBNull.Value);
        //    var parLocation = new SqlParameter("@Location", model.Location == null ? "" : model.Location);
        //    var parDealBuildingId = new SqlParameter("@DealBuildingId", DealBuildingId);
        //    var parTransactionId = new SqlParameter("@TransactionId", TransactionId);
        //    if (model.BuildingImage != null)
        //        parBuildingImage = new SqlParameter("@BuildingImage", model.BuildingImage.ToString());
        //    var parBuildingCategory = new SqlParameter("@BuildingCategoryId", BuildingCategoryId);
        //    objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspBuildingUpdateClient @BuildingId,@Title,@SubTitle,@Category_Name,@Phone,@Email,@About,@BuildingImage,@Location,@CreatedBy,@WebsiteLink,@BuildingCategoryId,@DealBuildingId,@TransactionId", parBuildingId, parTitle, parSubTitle, parCategory_Id, parPhone, parEmail, parAbout, parBuildingImage, parLocation, parCreatedBy, parWeb, parBuildingCategory, parDealBuildingId, parTransactionId).FirstOrDefault();
        //    return objStatus.ErrorMessage;
        //}

        public List<NeighborhoodDropDown> GetNeighborhood()
        {
            List<NeighborhoodDropDown> model = new List<NeighborhoodDropDown>();
            model = objDB.Database.SqlQuery<NeighborhoodDropDown>("select StateID,CityID[Neighborhood_Id],CityName[Name] from utblMstNeighborhoodCities order by name").ToList();
            return model;
        }

        public List<BuildingAttachments1> GetBuildingsAttachments()
        {
            List<BuildingAttachments1> model = new List<BuildingAttachments1>();
            model = objDB.Database.SqlQuery<BuildingAttachments1>("select BuildingAttachmentId[BuildingAttachment_Id],Name from BuildingAttachments order by Name").ToList();
            return model;
        }

        public List<Rules1> GetRules()
        {
            List<Rules1> model = new List<Rules1>();
            model = objDB.Database.SqlQuery<Rules1>("select RuleId[Rule_Id],Name from Rules order by Name").ToList();
            return model;
        }

        public List<Rules1> GetRulesByBuilding(int BuildingId)
        {
            List<Rules1> model = new List<Rules1>();
            var parBuildingId = new SqlParameter("@BuildingId", BuildingId);
            model = objDB.Database.SqlQuery<Rules1>("GetRulesByBuildingID @BuildingId", parBuildingId).ToList();
            return model;
        }

        public Buildings GetBuildingByID(int BuildingId)
        {
            Buildings objDetails = new Buildings();
            var parBuildingId = new SqlParameter("@BuildingId", BuildingId);
            objDetails = objDB.Database.SqlQuery<Buildings>("udspGetBuildingByID @BuildingId", parBuildingId).FirstOrDefault();
            return objDetails;
        }

        //public BuildingView GetBuildingByIDClient(string BuildingId)
        //{
        //    BuildingView objDetails = new BuildingView();
        //    var parBuildingId = new SqlParameter("@BuildingId", BuildingId);
        //    objDetails = objDB.Database.SqlQuery<BuildingView>("udspGetBuildingByID @BuildingId", parBuildingId).FirstOrDefault();
        //    return objDetails;
        //}
        //for client paging Building category single
        public BuildingPaging GetBuildingPrevNext(int BuildingId)
        {
            BuildingPaging objDetails = new BuildingPaging();
            var parBuildingId = new SqlParameter("@BuildingId", BuildingId);
            objDetails = objDB.Database.SqlQuery<BuildingPaging>("udspBuildingGetPaging @parBuildingId", parBuildingId).FirstOrDefault();
            return objDetails;
        }
        public string Delete(int BuildingId)
        {
            SPErrorViewModel objStatus = new SPErrorViewModel();
            var parBuildingId = new SqlParameter("@BuildingId", BuildingId);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspBuildingDelete @BuildingId", parBuildingId).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }

        public string DeleteBuildingRules(int BuildingId)
        {
            SPErrorViewModel objStatus = new SPErrorViewModel();
            var parBuildingId = new SqlParameter("@BuildingId", BuildingId);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspBuildingDeleteBuildingRules @BuildingId", parBuildingId).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }
    
        public string ChangeStatus(string BuildingId)
        {
            SPErrorViewModel objStatus = new SPErrorViewModel();
            var parBuildingId = new SqlParameter("@BuildingId", BuildingId);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspBuildingChangeStatus @BuildingId", parBuildingId).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }
        public IEnumerable<BuildingView> GetBuildingByBuildingType(string BuildingType)
        {
            List<BuildingView> objList = new List<BuildingView>();
            var parCategory = new SqlParameter("@Category", BuildingType);
            objList = objDB.Database.SqlQuery<BuildingView>("udspGetBuildingByBuildingType @Category", parCategory).ToList();
            return objList;
        }

        public IEnumerable<SearchAutoCompleteViewModel> BuildingAutoComplete(string searchTerm)
        {
            List<SearchAutoCompleteViewModel> obj = new List<SearchAutoCompleteViewModel>();
            var searchParam = new SqlParameter("@SearchTerm", searchTerm);
            obj = objDB.Database.SqlQuery<SearchAutoCompleteViewModel>("udspBuildingGetPagedAutoComplete @SearchTerm", searchParam).ToList();
            return obj;
        }



        //---------------- Building Documents------------------------//

        public IEnumerable<BuildingDocuments> GetBuildingDocuments(int BuildingId)
        {
            List<BuildingDocuments> objList = new List<BuildingDocuments>();
            var parCategory = new SqlParameter("@BuildingId", BuildingId);
            objList = objDB.Database.SqlQuery<BuildingDocuments>("udspGetBuildingDocuments @BuildingId", parCategory).ToList();
            return objList;
        }

        public string SaveBuildingDocument(BuildingDocuments model)
        {

            var parBuilding_Id = new SqlParameter("@Building_Id", model.Building_Id);
            var parBuildingAttachment_Id = new SqlParameter("@BuildingAttachment_Id", model.BuildingAttachment_Id);
            var parAttachmentFile = new SqlParameter("@AttachmentFile", model.AttachmentFile);
            var parCreatedBy = new SqlParameter("@CreatedBy", model.CreatedBy);
            var parDocumentName = new SqlParameter("@DocumentName", model.DocumentName);
            SPErrorViewModel objStatus = new SPErrorViewModel();
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("BuildingDocumentInsert @Building_Id,@BuildingAttachment_Id,@AttachmentFile,@CreatedBy,@DocumentName", parBuilding_Id, parBuildingAttachment_Id, parAttachmentFile, parCreatedBy, parDocumentName).FirstOrDefault();
            return objStatus.ErrorMessage;
        }
        
        public string GetBuildingFileName(int BuildingDocId)
        {

            var parClientDocId = new SqlParameter("@BuildingDocId", BuildingDocId);
            string Filename = objDB.Database.SqlQuery<string>("GetBuildingDocFileName @BuildingDocId", parClientDocId).FirstOrDefault();
            return Filename;
        }

        public string DeleteBuildingDoc(int BuildingAttachmentId)
        {
            SPErrorViewModel objStatus = new SPErrorViewModel();
            var parId = new SqlParameter("@ID", BuildingAttachmentId);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("BuildingDocumentDelete @ID", parId).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }

        //---------------- Building Documents------------------------//


        //---------------- Building Vendors------------------------//

        public IEnumerable<BuildingVendorView> GetBuildingVendorList( int BuildingId)
        {
            List<BuildingVendorView> objList = new List<BuildingVendorView>();
            var parTransactionID = new SqlParameter("@Building_Id", BuildingId);

            objList = objDB.Database.SqlQuery<BuildingVendorView>("udspGetBuildingVendors @Building_Id", parTransactionID).ToList();
            return objList;
        }


        public string SaveBuildingVendor(Vendor model, string CreatedBy, int BuildingId,string CreatedByEmail)
        {
            DateTime dt = DateTime.Now;
            var CurrentYear = dt.ToString("yyyy");
            SPErrorViewModel objStatus = new SPErrorViewModel();
            model.VendorId = CurrentYear + objUtil.generateUniqueCode("Vendor");
            var vendorCategoryId = CurrentYear + objUtil.generateUniqueCode("VendorCategory");
            var parVendorId = new SqlParameter("@VendorId", model.VendorId);
            var parTitle = new SqlParameter("@Title", model.Title);
            var parSubTitle = new SqlParameter("@SubTitle", model.SubTitle == null ? "" : model.SubTitle);
            var parPhone = new SqlParameter("@Phone", model.Phone == null ? "" : model.Phone);
            var parEmail = new SqlParameter("@Email", model.Email == null ? "" : model.Email);
            var parWeb = new SqlParameter("@WebsiteLink", model.WebsiteLink == null ? "" : model.WebsiteLink);
            var parAbout = new SqlParameter("@About", model.About == null ? "" : model.About);
            var parVendorImage = new SqlParameter("@VendorImage", DBNull.Value);
            var parLocation = new SqlParameter("@Location", model.Location == null ? "" : model.Location);
            if (model.VendorImage != null)
                parVendorImage = new SqlParameter("@VendorImage", model.VendorId.ToString() + model.VendorImage.ToString());
            var parCreatedBy = new SqlParameter("@CreatedBy", CreatedBy);
            var parCategory_Id = new SqlParameter("@Category_Name", model.Category_Id);
            var parTransactionId = new SqlParameter("@BuildingId", BuildingId);
            var parvendorCategory = new SqlParameter("@VendorCategoryId", vendorCategoryId);
            var parCreatedByEmail = new SqlParameter("@CreatedByEmail", CreatedByEmail);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspBuildingVendorInsert @VendorId,@Title,@SubTitle,@Category_Name,@Phone,@Email,@About,@VendorImage,@Location,@CreatedBy,@WebsiteLink,@BuildingId,@VendorCategoryId,@CreatedByEmail", parVendorId, parTitle, parSubTitle, parCategory_Id, parPhone, parEmail, parAbout, parVendorImage, parLocation, parCreatedBy, parWeb, parTransactionId,  parvendorCategory,parCreatedByEmail).FirstOrDefault();
            return objStatus.ErrorMessage;
        }

        public VendorView GetVendorByIDBuilding(string VendorId)
        {
            VendorView objDetails = new VendorView();
            var parVendorId = new SqlParameter("@VendorId", VendorId);
            objDetails = objDB.Database.SqlQuery<VendorView>("udspGetVendorByIDBuilding @VendorId", parVendorId).FirstOrDefault();
            return objDetails;
        }

        public string EditBuildingVendor(Vendor model, string CreatedBy, int BuildingId, int BuildingVendorId)
        {
            DateTime dt = DateTime.Now;
            var CurrentYear = dt.ToString("yyyy");
            SPErrorViewModel objStatus = new SPErrorViewModel();
            var parVendorId = new SqlParameter("@VendorId", model.VendorId);
            var vendorCategoryId = CurrentYear + objUtil.generateUniqueCode("VendorCategory");
            var parTitle = new SqlParameter("@Title", model.Title);
            var parSubTitle = new SqlParameter("@SubTitle", model.SubTitle == null ? "" : model.SubTitle);
            var parPhone = new SqlParameter("@Phone", model.Phone == null ? "" : model.Phone);
            var parEmail = new SqlParameter("@Email", model.Email == null ? "" : model.Email);
            var parWeb = new SqlParameter("@WebsiteLink", model.WebsiteLink == null ? "" : model.WebsiteLink);
            var parAbout = new SqlParameter("@About", model.About == null ? "" : model.About);
            var parCreatedBy = new SqlParameter("@CreatedBy", CreatedBy);
            var parCategory_Id = new SqlParameter("@Category_Name", model.Category_Id);
            var parVendorImage = new SqlParameter("@VendorImage", DBNull.Value);
            var parLocation = new SqlParameter("@Location", model.Location == null ? "" : model.Location);
            var parDealVendorId = new SqlParameter("@BuildingVendorId", BuildingVendorId);
            var parTransactionId = new SqlParameter("@BuildingId", BuildingId);
            if (model.VendorImage != null)
                parVendorImage = new SqlParameter("@VendorImage", model.VendorImage.ToString());
            var parvendorCategory = new SqlParameter("@VendorCategoryId", vendorCategoryId);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspBuildingVendorUpdate @VendorId,@Title,@SubTitle,@Category_Name,@Phone,@Email,@About,@VendorImage,@Location,@CreatedBy,@WebsiteLink,@VendorCategoryId,@BuildingVendorId,@BuildingId", parVendorId, parTitle, parSubTitle, parCategory_Id, parPhone, parEmail, parAbout, parVendorImage, parLocation, parCreatedBy, parWeb, parvendorCategory, parDealVendorId, parTransactionId).FirstOrDefault();
            return objStatus.ErrorMessage;
        }

        public string DeleteBuildingVendor(string VendorId)
        {
            SPErrorViewModel objStatus = new SPErrorViewModel();
            var parVendorId = new SqlParameter("@BuildingVendorId", VendorId);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspBuildingVendorDelete @BuildingVendorId", parVendorId).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }
        //---------------- Building Vendors------------------------//

        //---------------- Building Gallery------------------------//


        public IEnumerable<GalleryView> GetGalleryList(int BuildingId)
        {
            List<GalleryView> objList = new List<GalleryView>();
            var parTransactionID = new SqlParameter("@Building_Id", BuildingId);

            objList = objDB.Database.SqlQuery<GalleryView>("udspGetBuildingGallery @Building_Id", parTransactionID).ToList();
            return objList;
        }

        public string SaveGallery(GalleryView model)
        {
       
            SPErrorViewModel objStatus = new SPErrorViewModel();
            var parBuildingId = new SqlParameter("@BuildingId", model.Building_Id);
            var parDescription = new SqlParameter("@Description", model.Description == null ? "" : model.Description);
            var parImage = new SqlParameter("@Image", DBNull.Value);
            if (model.Image != null)
                parImage = new SqlParameter("@Image",model.Image.ToString());
            var parCreatedBy = new SqlParameter("@CreatedBy", model.CreatedBy);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("BuildingGalleryInsert @BuildingId,@Description,@Image,@CreatedBy", parBuildingId, parDescription, parImage, parCreatedBy).FirstOrDefault();
            return objStatus.ErrorMessage;
        }

        public string DeleteGallery(int BuildingGalleryId)
        {
            SPErrorViewModel objStatus = new SPErrorViewModel();
            var parId = new SqlParameter("@BuildingGalleryId", BuildingGalleryId);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspBuildingGalleryDelete @BuildingGalleryId", parId).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }

        public GalleryView GetImageByBuildingGalleryId(int BuildingGalleryId)
        {
            GalleryView objDetails = new GalleryView();
            var parId = new SqlParameter("@BuildingGalleryId", BuildingGalleryId);
            objDetails = objDB.Database.SqlQuery<GalleryView>("udspGetGalleryByIDBuilding @BuildingGalleryId", parId).FirstOrDefault();
            return objDetails;
        }
        //---------------- Building Gallery------------------------//


        public BuildingView GetBuildingClientSide(int BuildingId)
        {

            BuildingView objDetails = new BuildingView();
            var parBuildingId = new SqlParameter("@BuildingId", BuildingId);
            objDetails = objDB.Database.SqlQuery<BuildingView>("GetBuildingClientSide @BuildingId", parBuildingId).FirstOrDefault();
            return objDetails;
        }

        //---------------- Transaction Builsing------------------------//
        public string SaveTransactionBuilding(DealBuildingView model)
        {
            SPErrorViewModel objStatus = new SPErrorViewModel();
            
            var parBuildingId = new SqlParameter("@BuildingId", model.Building_Id);
            var parTransactionId = new SqlParameter("@TransactionId", model.Transaction_ID);
            var parCreatedBy= new SqlParameter("@CreatedBy", model.CreatedBy);
            
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("TransactionBuildingInsert @BuildingId,@TransactionId,@CreatedBy", parBuildingId,parTransactionId,parCreatedBy).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }

        public string DeleteTransactionBuilding(int TransactionBuildingId)
        {
            SPErrorViewModel objStatus = new SPErrorViewModel();
            var parId = new SqlParameter("@TransactionBuildingId", TransactionBuildingId);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspTransactionBuildingDelete @TransactionBuildingId", parId).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }

        public BuildingView GetBuildingByTransaction(string TransactionId)
        {
            BuildingView objDetails = new BuildingView();
            var parBuildingId = new SqlParameter("@TransactionId", TransactionId);
            objDetails = objDB.Database.SqlQuery<BuildingView>("GetBuildingByTransactionId @TransactionId", parBuildingId).FirstOrDefault();
            return objDetails;
        }


        public List<BuildingDropdown> GetBuildings()
        {
            List<BuildingDropdown> model = new List<BuildingDropdown>();
            model = objDB.Database.SqlQuery<BuildingDropdown>("select BuildingId[BuildingId],Name from buildings order by Name").ToList();
            return model;
        }
    }
}
