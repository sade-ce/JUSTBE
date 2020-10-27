using RealEstate.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RealEstate.Entities.Models;
using RealEstate.Entities.Utility;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Globalization;

namespace RealEstate.Entities.DataAccess
{
    public class dalTrackDeal
    {
        public EFDBContext objDB = new EFDBContext();
        RealEstateUtility objUtil = new RealEstateUtility();

        public IEnumerable<utblMstTrackDeal> MstDealList
        {
            get
            {
                return objDB.utblMstTrackDeals;
            }
        }
        public IEnumerable<MstStatusDDL> GetStatusDDL()
        {
            List<MstStatusDDL> objList = new List<MstStatusDDL>();
            objList = objDB.Database.SqlQuery<MstStatusDDL>("udspStatusSelect").ToList();
            return objList;
        }

        public IEnumerable<MstClientDDL> GetClientDDL()
        {
            List<MstClientDDL> objList = new List<MstClientDDL>();
            objList = objDB.Database.SqlQuery<MstClientDDL>("udspClientSelect").ToList();
            return objList;
        }

        public bool emailPreferences(string Email)
        {
            var parEmail = new SqlParameter("@Email", Email);
            var result = objDB.Database.SqlQuery<bool>("select StatusTimeline from AspNetUsers where UserName=@Email", parEmail).FirstOrDefault();
            return result;
        }
        public UserStatusSelect GetClientStatusfromEmail(string Email,int StatusID)
        {
            UserStatusSelect obj = new UserStatusSelect();
            var parEmail = new SqlParameter("@Email", Email);
            var parStatusID = new SqlParameter("@StatusID", StatusID);

            obj = objDB.Database.SqlQuery<UserStatusSelect>("udspUserClientNameSelect @Email,@StatusID", parEmail, parStatusID).FirstOrDefault();
            return obj;
        }

        public UserStatusSelect SelectBulkStatus(string Email, int StatusID)
        {
            UserStatusSelect obj = new UserStatusSelect();
            var parEmail = new SqlParameter("@Email", Email);
            var parStatusID = new SqlParameter("@StatusID", StatusID);
            obj = objDB.Database.SqlQuery<UserStatusSelect>("udspUserClientNameSelect @Email,@StatusID", parEmail, parStatusID).FirstOrDefault();
            return obj;
        }

        //NewVersion
        public List<UserStatusSelect> SelectBulkStatusVersion2(string Email,string TransactionID)
        {
            List<UserStatusSelect> obj = new List<UserStatusSelect>();
            var parEmail = new SqlParameter("@Email", Email);
            var parTransactionID = new SqlParameter("@TransactionID", TransactionID);
            obj = objDB.Database.SqlQuery<UserStatusSelect>("SelectBulkStatusVersion2 @Email,@TransactionID", parEmail, parTransactionID).ToList();
            return obj;
        }
        //NewVersion
        public List<UserStatusSelect> SelectSellerBulkStatusVersion2(string Email, string TransactionID)
        {
            List<UserStatusSelect> obj = new List<UserStatusSelect>();
            var parEmail = new SqlParameter("@Email", Email);
            var parTransactionID = new SqlParameter("@TransactionID", TransactionID);
            obj = objDB.Database.SqlQuery<UserStatusSelect>("SelectSellerBulkStatusVersion2 @Email,@TransactionID", parEmail, parTransactionID).ToList();
            return obj;
        }
        public string DeleteSendEmail(string TransactionID)
        {
            SPErrorViewModel objStatus = new SPErrorViewModel();
            var parID = new SqlParameter("@TransactionID", TransactionID);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("DeleteSendEmail @TransactionID", parID).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }

        public MstAgentClientNameSelect GetNameEmail(string TransactionID)
        {
            MstAgentClientNameSelect obj = new MstAgentClientNameSelect();
            var parTransactionID = new SqlParameter("@TransactionID", TransactionID);
            obj = objDB.Database.SqlQuery<MstAgentClientNameSelect>("udspUserAgentNameSelect @TransactionID", parTransactionID).FirstOrDefault();
            return obj;
        }
        public utblMstStatus GetStatusByID(int ID)
        {
            utblMstStatus objItem = objDB.utblMstStatus.FirstOrDefault(p => p.StatusID == ID);
            return objItem;
        }


        //Old Version Save Steps
        public string Save(utblMstTrackDeal Item,IEnumerable<MstContingenciesView> Contengencies,string Agent)
        {
            SPErrorViewModel objStatus = new SPErrorViewModel();

            foreach ( var itemData in Contengencies)
            { 
                if(itemData.StatusID!=0)
                {
                    DateTime dt = DateTime.Now;
                    var CurrentYear = dt.ToString("yyyy");
                    Item.TrackingID = CurrentYear + objUtil.generateUniqueCode("utblMstTrackDeals");
                    var parTrackingID = new SqlParameter("@TrackingID", Item.TrackingID);
                    var parStatusID = new SqlParameter("@StatusID", itemData.StatusID);
                    var parClientID = new SqlParameter("@ClientID", Item.ClientID);
                    var parTransactionID = new SqlParameter("@TransactionID", Item.TransactionID);
                    var parIsApplicable = new SqlParameter("@IsApplicable", itemData.IsApplicable);
                    var parEmail = new SqlParameter("@Email", Item.Email);
                    var parUpdatedOn = new SqlParameter("@UpdatedOn","");
                    if (itemData.IsContingencies == true)
                    {
                        if (itemData.IsApplicable == true)
                        {
                            parUpdatedOn = new SqlParameter("@UpdatedOn", Item.UpdatedOn.AddDays(itemData.NoOfDays).AddHours(18));

                        }
                        else
                        {
                            parUpdatedOn = new SqlParameter("@UpdatedOn", System.DateTime.Now);

                        }
                    }
                    else
                    {
                        parUpdatedOn = new SqlParameter("@UpdatedOn", itemData.UpdatedOn);

                    }
                    if (itemData.IsApplicable==true)
                    {
                        var parAppointmentTrackingID = new SqlParameter("@TrackingID", Item.TrackingID);
                        var parAppointmentTTransactionID = new SqlParameter("@TransactionID", Item.TransactionID);//Added by Neha
                        var parClientEmail = new SqlParameter("@Email", Item.Email);
                        var parAgentEmail = new SqlParameter("@AgentEmail", Agent);
                        var parIsContingency = new SqlParameter("@IsContingency", itemData.IsApplicable);
                        var parDescription = new SqlParameter("@StatusID", itemData.StatusID);
                        var parStartDate = new SqlParameter("@StartDate", Item.UpdatedOn.AddDays(itemData.NoOfDays).AddHours(17));
                        var parEndDate = new SqlParameter("@EndDate", Item.UpdatedOn.AddDays(itemData.NoOfDays).AddHours(18));
                        var parcolor = new SqlParameter("@Color", "#962e06");
                        objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspCalenderContingencyInsertNew @Email,@AgentEmail, @IsContingency,@StatusID,@StartDate,@EndDate,@Color,@TrackingID,@TransactionID", parClientEmail, parAgentEmail, parIsContingency, parDescription, parStartDate, parEndDate, parcolor, parAppointmentTrackingID,parAppointmentTTransactionID).FirstOrDefault();
                    }
                    objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspMstInsert @TrackingID, @StatusID,@ClientID,@TransactionID,@Email,@IsApplicable,@UpdatedOn", parTrackingID, parStatusID, parClientID, parTransactionID, parEmail, parIsApplicable, parUpdatedOn).FirstOrDefault();
                }

         
            }
            return objStatus.ErrorCode + objStatus.ErrorMessage;

        }

        //New Version Save Steps
        public string SaveVersion2(DealTracker Item, IEnumerable<MstContingenciesViewVersion2> Contengencies, string Agent)
        {
            SPErrorViewModel objStatus = new SPErrorViewModel();

            foreach (var itemData in Contengencies)
            {
                //if (itemData.IsShowClient == true)
                //{
                DateTime dt = DateTime.Now;
                var CurrentYear = dt.ToString("yyyy");
                if (String.IsNullOrEmpty(itemData.TrackingID))
                    Item.TrackingID = CurrentYear + objUtil.generateUniqueCode("utblMstTrackDeals");
                else
                    Item.TrackingID = itemData.TrackingID;
                var parTrackingID = new SqlParameter("@TrackingID", Item.TrackingID);
                var parStatusID = new SqlParameter("@StatusID", itemData.StatusID);
                var parClientID = new SqlParameter("@ClientID", Item.ClientID);
                var parTransactionID = new SqlParameter("@TransactionID", Item.TransactionID);
                var parEmail = new SqlParameter("@Email", Item.Email);
                var parUpdatedOn = new SqlParameter("@UpdatedOn", "");
                if (!string.IsNullOrEmpty(itemData.Time))
                {
                    DateTime timeValue = Convert.ToDateTime(itemData.Time);
                    double hours = Convert.ToDouble(timeValue.ToString("HH"));
                    parUpdatedOn = new SqlParameter("@UpdatedOn", itemData.UpdatedOn.AddHours(hours));
                }
                else
                {
                    parUpdatedOn = new SqlParameter("@UpdatedOn", itemData.UpdatedOn);
                }
                //if (itemData.IsContingencies == true)
                //{
                //    if (itemData.IsApplicable == true)
                //    {
                //        parUpdatedOn = new SqlParameter("@UpdatedOn", Item.UpdatedOn.AddDays(itemData.NoOfDays).AddHours(18));

                //    }
                //    else
                //    {
                //        parUpdatedOn = new SqlParameter("@UpdatedOn", System.DateTime.Now);

                //    }
                //}

                //else
                //{
                //    parUpdatedOn = new SqlParameter("@UpdatedOn", itemData.UpdatedOn);

                //}
                var parIsShowClient = new SqlParameter("@IsShowClient", itemData.IsShowClient);
                var parNoOfDays = new SqlParameter("@NoOfDays", itemData.NoOfDays);
                var parAdditionalInfo = new SqlParameter("@AdditionalInfo", string.IsNullOrEmpty(itemData.AdditionalInfo) ? "" : itemData.AdditionalInfo);
                var parIsCompleted = new SqlParameter("@IsCompleted", itemData.IsCompleted);
                var parItems = new SqlParameter("@Items", string.IsNullOrEmpty(Item.CheckListItems) ? "" : Item.CheckListItems);
                var parNotes = new SqlParameter("@Notes", string.IsNullOrEmpty(itemData.Notes) ? "" : itemData.Notes);
                bool AddAsEvent = false;
                if ((itemData.StatusID == 7))
                {
                    if (itemData.IsCompleted == true)
                    {
                        AddAsEvent = true;
                        itemData.IsApplicable = false;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(itemData.TrackingID))
                            objStatus = RemoveDealEventVersion2(itemData.TrackingID, Item.Email, Item.TransactionID);
                    }

                }
                else
                {
                    if (itemData.IsContingencies == true)
                    {
                        //if (itemData.IsCompleted == true)
                        //{
                        if (itemData.NoOfDays > 0)
                        {
                            itemData.IsApplicable = true;
                            AddAsEvent = true;
                        }
                        else
                        {
                            itemData.IsApplicable = false;
                            AddAsEvent = false;
                            if (!string.IsNullOrEmpty(itemData.TrackingID))
                                objStatus = RemoveDealEventVersion2(itemData.TrackingID, Item.Email, Item.TransactionID);
                        }
                        //}
                        //else
                        //{
                        //    if (!string.IsNullOrEmpty(itemData.TrackingID))
                        //        objStatus = RemoveDealEventVersion2(itemData.TrackingID, Item.Email, Item.TransactionID);
                        //}

                    }
                }


                var parIsApplicable = new SqlParameter("@IsApplicable", itemData.IsApplicable);
                if (AddAsEvent == true)
                {


                    utblMstClosingDate RatifiedDetails = new utblMstClosingDate();
                    var parId = new SqlParameter("@TransactionID", Item.TransactionID);
                    RatifiedDetails = objDB.Database.SqlQuery<utblMstClosingDate>("GetRatifiedDetails @TransactionID", parId).FirstOrDefault();
                    //int loop = 0;
                    //if ((itemData.StatusID == 8 || itemData.StatusID == 9) && !string.IsNullOrEmpty(itemData.AdditionalInfo))
                    //{
                    //    loop = 1;
                    //}
                    //for (int i = 0; i <= loop; i++) { 
                    DateTime timeValue = Convert.ToDateTime(itemData.Time);
                    double hours = Convert.ToDouble(timeValue.ToString("HH"));
                    var parAppointmentTrackingID = new SqlParameter("@TrackingID", Item.TrackingID);
                    var parAppointmentTTransactionID = new SqlParameter("@TransactionID", Item.TransactionID);//Added by Neha
                    var parClientEmail = new SqlParameter("@Email", Item.Email);
                    var parAgentEmail = new SqlParameter("@AgentEmail", Agent);
                    var parIsContingency = new SqlParameter("@IsContingency", itemData.IsApplicable);
                    var parDescription = new SqlParameter("@StatusID", itemData.StatusID);
                    var parStartDate = new SqlParameter("@StartDate", System.DateTime.Now);
                    var parEndDate = new SqlParameter("@EndDate", System.DateTime.Now);
                    if (itemData.StatusID == 7)
                    {
                        double Hours = 0;
                        if (RatifiedDetails.StateId == 1 || RatifiedDetails.StateId == 3)
                        {
                            Hours = 18;
                        }
                        if (RatifiedDetails.StateId == 2)
                        {
                            Hours = 21;
                        }
                        parStartDate = new SqlParameter("@StartDate", Convert.ToDateTime(RatifiedDetails.ClosingDate).AddHours(Hours - 1));
                        parEndDate = new SqlParameter("@EndDate", Convert.ToDateTime(RatifiedDetails.ClosingDate).AddHours(Hours));
                    }
                    else
                    {

                        if (itemData.StatusID == 16)
                        {
                            parStartDate = new SqlParameter("@StartDate", Convert.ToDateTime(RatifiedDetails.ClosingDate).AddDays(-itemData.NoOfDays).AddHours(hours));

                            parEndDate = new SqlParameter("@EndDate", Convert.ToDateTime(RatifiedDetails.ClosingDate).AddHours(hours));
                        }
                        else if (itemData.StatusID == 17)
                        {
                            parStartDate = new SqlParameter("@StartDate", Convert.ToDateTime(RatifiedDetails.ClosingDate).AddHours(hours));

                            parEndDate = new SqlParameter("@EndDate", Convert.ToDateTime(RatifiedDetails.ClosingDate).AddDays(itemData.NoOfDays).AddHours(hours));
                        }
                        else
                        {

                            parStartDate = new SqlParameter("@StartDate", itemData.UpdatedOn.AddHours(hours - 1));

                            parEndDate = new SqlParameter("@EndDate", itemData.UpdatedOn.AddHours(hours));
                        }

                    }
                    //var parStartDate = new SqlParameter("@StartDate", Item.UpdatedOn.AddDays(itemData.NoOfDays).AddHours(17));
                    //var parEndDate = new SqlParameter("@EndDate", Item.UpdatedOn.AddDays(itemData.NoOfDays).AddHours(18));
                    //var parStartDate = new SqlParameter("@StartDate", itemData.UpdatedOn.AddHours(-1));
                    //var parEndDate = new SqlParameter("@EndDate", itemData.UpdatedOn);
                    var parcolor = new SqlParameter("@Color", "#962e06");

                    objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspCalenderContingencyInsertVersion2 @Email,@AgentEmail, @IsContingency,@StatusID,@StartDate,@EndDate,@Color,@TrackingID,@TransactionID", parClientEmail, parAgentEmail, parIsContingency, parDescription, parStartDate, parEndDate, parcolor, parAppointmentTrackingID, parAppointmentTTransactionID).FirstOrDefault();
               // }
                

            }
                    objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspMstInsertVersion2 @TrackingID, @StatusID,@ClientID,@TransactionID,@Email,@IsApplicable,@UpdatedOn,@IsShowClient,@NoOfDays,@AdditionalInfo,@IsCompleted,@Items,@Notes", parTrackingID, parStatusID, parClientID, parTransactionID, parEmail, parIsApplicable, parUpdatedOn,parIsShowClient,parNoOfDays,parAdditionalInfo,parIsCompleted,parItems,parNotes).FirstOrDefault();
               


            }
            return objStatus.ErrorCode + objStatus.ErrorMessage;

        }

        //New Version Save Steps
        public string SaveSellerVersion2(DealTracker Item, IEnumerable<MstSellerContingenciesViewVersion2> Contengencies, string Agent)
        {
            SPErrorViewModel objStatus = new SPErrorViewModel();

            foreach (var itemData in Contengencies)
            {
            
                DateTime dt = DateTime.Now;
                var CurrentYear = dt.ToString("yyyy");
                if (String.IsNullOrEmpty(itemData.TrackingID))
                    Item.TrackingID = CurrentYear + objUtil.generateUniqueCode("utblMstSellerTrackDeals");
                else
                    Item.TrackingID = itemData.TrackingID;
                var parTrackingID = new SqlParameter("@TrackingID", Item.TrackingID);
                var parStatusID = new SqlParameter("@StatusID", itemData.SellerStatusID);
                var parClientID = new SqlParameter("@ClientID", Item.ClientID);
                var parTransactionID = new SqlParameter("@TransactionID", Item.TransactionID);
                var parEmail = new SqlParameter("@Email", Item.Email);
                var parUpdatedOn = new SqlParameter("@UpdatedOn", "");
                if (!string.IsNullOrEmpty(itemData.Time))
                {
                    DateTime timeValue = Convert.ToDateTime(itemData.Time);
                    double hours = Convert.ToDouble(timeValue.ToString("HH"));
                    parUpdatedOn = new SqlParameter("@UpdatedOn", itemData.UpdatedOn.AddHours(hours));
                }
                else
                {
                    parUpdatedOn = new SqlParameter("@UpdatedOn", itemData.UpdatedOn);
                }
      
                var parIsShowClient = new SqlParameter("@IsShowClient", itemData.IsShowClient);
                var parNoOfDays = new SqlParameter("@NoOfDays", itemData.NoOfDays);
                var parAdditionalInfo = new SqlParameter("@AdditionalInfo", string.IsNullOrEmpty(itemData.AdditionalInfo) ? "" : itemData.AdditionalInfo);
                var parIsCompleted = new SqlParameter("@IsCompleted", itemData.IsCompleted);
                var parItems = new SqlParameter("@Items", string.IsNullOrEmpty(Item.CheckListItems) ? "" : Item.CheckListItems);
                var parNotes = new SqlParameter("@Notes", string.IsNullOrEmpty(itemData.Notes) ? "" : itemData.Notes);
                bool AddAsEvent = false;
                //if ((itemData.SellerStatusID == 6))
               // {
                //    if (itemData.IsCompleted == true)
                //    {
                //        AddAsEvent = true;
                //        itemData.IsApplicable = false;
                //    }
                //    else
                //    {
                //        if (!string.IsNullOrEmpty(itemData.TrackingID))
                //            objStatus = RemoveSellerDealEventVersion2(itemData.TrackingID, Item.Email, Item.TransactionID);
                //    }

                //}
                //else
                //{
                    if (itemData.IsContingencies == true)
                    {
                    
                        if (itemData.NoOfDays > 0)
                        {
                            itemData.IsApplicable = true;
                            AddAsEvent = true;
                        }
                        else
                        {
                            itemData.IsApplicable = false;
                            AddAsEvent = false;
                            if (!string.IsNullOrEmpty(itemData.TrackingID))
                                objStatus = RemoveSellerDealEventVersion2(itemData.TrackingID, Item.Email, Item.TransactionID);
                        }
                 

                    }
               // }


                var parIsApplicable = new SqlParameter("@IsApplicable", itemData.IsApplicable);
                if (AddAsEvent == true)
                {


                    utblMstClosingDate RatifiedDetails = new utblMstClosingDate();
                    var parId = new SqlParameter("@TransactionID", Item.TransactionID);
                  //  RatifiedDetails = objDB.Database.SqlQuery<utblMstClosingDate>("GetRatifiedDetails @TransactionID", parId).FirstOrDefault();
                 
                        DateTime timeValue = Convert.ToDateTime(itemData.Time);
                        double hours = Convert.ToDouble(timeValue.ToString("HH"));
                        var parAppointmentTrackingID = new SqlParameter("@TrackingID", Item.TrackingID);
                        var parAppointmentTTransactionID = new SqlParameter("@TransactionID", Item.TransactionID);//Added by Neha
                        var parClientEmail = new SqlParameter("@Email", Item.Email);
                        var parAgentEmail = new SqlParameter("@AgentEmail", Agent);
                        var parIsContingency = new SqlParameter("@IsContingency", itemData.IsApplicable);
                        var parDescription = new SqlParameter("@StatusID", itemData.SellerStatusID);
                        var parStartDate = new SqlParameter("@StartDate", System.DateTime.Now);
                        var parEndDate = new SqlParameter("@EndDate", System.DateTime.Now);
                        //if (itemData.SellerStatusID == 6)
                        //{
                        //    double Hours = 0;
                        //    if (RatifiedDetails.StateId == 1 || RatifiedDetails.StateId == 3)
                        //    {
                        //        Hours = 18;
                        //    }
                        //    if (RatifiedDetails.StateId == 2)
                        //    {
                        //        Hours = 21;
                        //    }
                        //    parStartDate = new SqlParameter("@StartDate", RatifiedDetails.ClosingDate.AddHours(Hours - 1));
                        //    parEndDate = new SqlParameter("@EndDate", RatifiedDetails.ClosingDate.AddHours(Hours));
                        //}
                        //else
                        //{
                        

                                parStartDate = new SqlParameter("@StartDate", itemData.UpdatedOn.AddHours(hours - 1));

                                parEndDate = new SqlParameter("@EndDate", itemData.UpdatedOn.AddHours(hours));
                        

                        //}
                        var parcolor = new SqlParameter("@Color", "#962e06");

                        objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspCalenderContingencySellerInsertVersion2 @Email,@AgentEmail, @IsContingency,@StatusID,@StartDate,@EndDate,@Color,@TrackingID,@TransactionID", parClientEmail, parAgentEmail, parIsContingency, parDescription, parStartDate, parEndDate, parcolor, parAppointmentTrackingID, parAppointmentTTransactionID).FirstOrDefault();
             
                }
                objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspMstSellerInsertVersion2 @TrackingID, @StatusID,@ClientID,@TransactionID,@Email,@IsApplicable,@UpdatedOn,@IsShowClient,@NoOfDays,@AdditionalInfo,@IsCompleted,@Items,@Notes", parTrackingID, parStatusID, parClientID, parTransactionID, parEmail, parIsApplicable, parUpdatedOn, parIsShowClient, parNoOfDays, parAdditionalInfo, parIsCompleted, parItems, parNotes).FirstOrDefault();



            }
            return objStatus.ErrorCode + objStatus.ErrorMessage;

        }



        public string SaveClosingConfiguration(utblMstClosingDate Item,int StatusID,string TransactionID,string AgentEmail,List<MstContingenciesView> ItemData)
        {
            DateTime dt = DateTime.Now;
            var CurrentYear = dt.ToString("yyyy");
            SPErrorViewModel objStatus = new SPErrorViewModel();
            var parMLSID = new SqlParameter("@MLSID", "");
            var parURL = new SqlParameter("@URL", "");
            var TrackingID = CurrentYear + objUtil.generateUniqueCode("utblMstTrackDeals");
            var parTrackingID = new SqlParameter("@TrackingID", TrackingID);
            var parStatusID = new SqlParameter("@StatusID", StatusID);
            var parEmail = new SqlParameter("@Email", Item.Email);
            var parClosingDate = new SqlParameter("@ClosingDate", Item.ClosingDate);
            if (Item.MLSID != null)
                parMLSID = new SqlParameter("@MLSID", Item.MLSID);

            var parListingTypeID = new SqlParameter("@ListingTypeID", Item.ListingTypeID);
            var parAddress = new SqlParameter("@Address", Item.Address);
            if (Item.URL != null)
                parURL = new SqlParameter("@URL", Item.URL);
            var parClientID = new SqlParameter("@ClientID", Item.ClientID);
            var parTransactionID = new SqlParameter("@TransactionID",TransactionID);


            var parUpdatedOn = new SqlParameter("@UpdatedOn", System.DateTime.Now);

            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspMstClosingDateConfigurationInsert @TrackingID, @StatusID,@Email,@ClosingDate,@MLSID,@ListingTypeID,@Address,@URL,@ClientID,@TransactionID,@UpdatedOn", parTrackingID, parStatusID, parEmail, parClosingDate,parMLSID,parListingTypeID,parAddress,parURL, parClientID,parTransactionID, parUpdatedOn).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;

        
    }
        public string SaveClosingConfigurationVersion2(utblMstClosingDate Item, int StatusID, string TransactionID, string AgentEmail, List<MstContingenciesViewVersion2> ItemData)
        {
            var Step = ItemData.Where(m => m.StatusID == StatusID).FirstOrDefault();
            DateTime dt = DateTime.Now;
            var CurrentYear = dt.ToString("yyyy");
            SPErrorViewModel objStatus = new SPErrorViewModel();
            var parMLSID = new SqlParameter("@MLSID", "");
            var parURL = new SqlParameter("@URL", "");
            //var TrackingID = CurrentYear + objUtil.generateUniqueCode("utblMstTrackDeals");
            //var parTrackingID = new SqlParameter("@TrackingID", TrackingID);
            var parStatusID = new SqlParameter("@StatusID", StatusID);
            var parEmail = new SqlParameter("@Email", Item.Email);
            var parClosingDate = new SqlParameter("@ClosingDate", Item.ClosingDate);
            if (Item.MLSID != null)
                parMLSID = new SqlParameter("@MLSID", Item.MLSID);

            var parListingTypeID = new SqlParameter("@ListingTypeID", Item.ListingTypeID);
            var parAddress = new SqlParameter("@Address", Item.Address);
            if (Item.URL != null)
                parURL = new SqlParameter("@URL", Item.URL);
            var parClientID = new SqlParameter("@ClientID", Item.ClientID);
            var parTransactionID = new SqlParameter("@TransactionID", TransactionID);
            

            var parUpdatedOn = new SqlParameter("@UpdatedOn", Convert.ToDateTime(System.DateTime.Now));
            var parHomeType = new SqlParameter("@HomeType", Item.HomeType==null?0:Item.HomeType);
            var parStateId = new SqlParameter("@StateID", Item.StateId == null ? 0 : Item.StateId);
            var parDays = new SqlParameter("@NoOfDays", Step.NoOfDays);
            var parType = new SqlParameter("@ContingencyType", Item.ContingencyType == null ? 0 : Item.ContingencyType);
            var parlots = new SqlParameter("@lots", Item.lots == null ? 0 : Item.lots);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspMstClosingDateConfigurationInsertVersion2  @StatusID,@Email,@ClosingDate,@MLSID,@ListingTypeID,@Address,@URL,@ClientID,@TransactionID,@UpdatedOn,@HomeType,@StateID,@NoOfDays,@ContingencyType,@lots", parStatusID, parEmail, parClosingDate, parMLSID, parListingTypeID, parAddress, parURL, parClientID, parTransactionID, parUpdatedOn,parHomeType,parStateId,parDays, parType, parlots).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;

            
        }

        public string SaveSellerClosingConfigurationVersion2(utblMstClosingDate Item, int StatusID, string TransactionID, string AgentEmail, List<MstSellerContingenciesViewVersion2> ItemData)
        {
            var Step = ItemData.Where(m => m.SellerStatusID == StatusID).FirstOrDefault();
            DateTime dt = DateTime.Now;
            var CurrentYear = dt.ToString("yyyy");
            SPErrorViewModel objStatus = new SPErrorViewModel();
            var parMLSID = new SqlParameter("@MLSID", "");
            var parURL = new SqlParameter("@URL", "");
            //var TrackingID = CurrentYear + objUtil.generateUniqueCode("utblMstTrackDeals");
            //var parTrackingID = new SqlParameter("@TrackingID", TrackingID);
            var parStatusID = new SqlParameter("@StatusID", StatusID);
            var parEmail = new SqlParameter("@Email", Item.Email);
            var parClosingDate = new SqlParameter("@ClosingDate", System.DateTime.Now);
            if (Item.MLSID != null)
                parMLSID = new SqlParameter("@MLSID", Item.MLSID);

            var parListingTypeID = new SqlParameter("@ListingTypeID", Item.ListingTypeID);
            var parAddress = new SqlParameter("@Address", Item.Address);
            if (Item.URL != null)
                parURL = new SqlParameter("@URL", Item.URL);
            var parClientID = new SqlParameter("@ClientID", Item.ClientID);
            var parTransactionID = new SqlParameter("@TransactionID", TransactionID);


            var parUpdatedOn = new SqlParameter("@UpdatedOn", Convert.ToDateTime(System.DateTime.Now));
            var parHomeType = new SqlParameter("@HomeType", Item.HomeType == null ? 0 : Item.HomeType);
            var parStateId = new SqlParameter("@StateID", Item.StateId == null ? 0 : Item.StateId);
            var parDays = new SqlParameter("@NoOfDays", Step.NoOfDays);
            var parType = new SqlParameter("@ContingencyType", Item.ContingencyType == null ? 0 : Item.ContingencyType);
            var parlots = new SqlParameter("@lots", Item.lots == null ? 0 : Item.lots);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspMstSellerClosingDateConfigurationInsertVersion2  @StatusID,@Email,@ClosingDate,@MLSID,@ListingTypeID,@Address,@URL,@ClientID,@TransactionID,@UpdatedOn,@HomeType,@StateID,@NoOfDays,@ContingencyType,@lots", parStatusID, parEmail, parClosingDate, parMLSID, parListingTypeID, parAddress, parURL, parClientID, parTransactionID, parUpdatedOn, parHomeType, parStateId, parDays, parType, parlots).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;


        }



        public string SaveBulk(utblMstClosingDate Item, int StatusID, string TransactionID, string AgentEmail, List<MstContingenciesView> ItemData)
        {
            DateTime dt = DateTime.Now;
            var CurrentYear = dt.ToString("yyyy");
            SPErrorViewModel objStatus = new SPErrorViewModel();
           
            foreach (var item in ItemData)
            {
                if ((item.StatusID != 0))
                {
                   
                    var TrackingID = CurrentYear + objUtil.generateUniqueCode("utblMstTrackDeals");
                    var parTrackingID = new SqlParameter("@TrackingID", TrackingID);
                    var parEmail = new SqlParameter("@Email", Item.Email);
                    var parClientID = new SqlParameter("@ClientID", Item.ClientID);
                    var parTransactionID = new SqlParameter("@TransactionID", TransactionID);
                    var parUpdatedOn = new SqlParameter("@UpdatedOn",DateTime.Now);

                    if (item.StatusID==StatusID)
                    {
                         parUpdatedOn = new SqlParameter("@UpdatedOn", Item.UpdatedOn);

                    }
                    var parStatusID = new SqlParameter("@StatusID", item.StatusID);
                    var parIsApplicable = new SqlParameter("@IsApplicable", item.IsApplicable);
                    objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspMstInsert @TrackingID, @StatusID,@ClientID,@TransactionID,@Email,@IsApplicable,@UpdatedOn", parTrackingID, parStatusID, parClientID, parTransactionID, parEmail, parIsApplicable, parUpdatedOn).FirstOrDefault();

                    //if (item.IsApplicable == true)
                    //{
                    //    objStatus.ErrorMessage= SaveContengencies(Item, StatusID, TransactionID, AgentEmail, ItemData);
                    //}
                }
            }
            return objStatus.ErrorCode + objStatus.ErrorMessage;

        }



        //public string SaveContengencies(utblMstClosingDate Item, int StatusID, string TransactionID, string AgentEmail, List<MstContingenciesView> ItemData)
        //{
        //    DateTime dt = DateTime.Now;
        //    var CurrentYear = dt.ToString("yyyy");
        //    SPErrorViewModel objStatus = new SPErrorViewModel();

        //    foreach (var item in ItemData)
        //    {
        //        if ((item.StatusID != 0) && (item.StatusID != StatusID))
        //        {
        //            if (item.IsApplicable == true)
        //            {
        //                var parEmail = new SqlParameter("@Email", Item.Email);
        //                var parStatusID = new SqlParameter("@StatusID", item.StatusID);
        //                var parAgentEmail = new SqlParameter("@AgentEmail", AgentEmail);
        //                var parIsContingency = new SqlParameter("@IsContingency", item.IsApplicable);
        //                var parStartDate = new SqlParameter("@StartDate", item.StartDate);
        //                var parEndDate = new SqlParameter("@EndDate", item.StartDate.AddDays(item.NoOfDays));
        //                var parcolor = new SqlParameter("@Color", "#962e06");
        //                objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspCalenderContingencyInsert @Email,@AgentEmail, @IsContingency,@StatusID,@StartDate,@EndDate,@Color", parEmail, parAgentEmail, parIsContingency, parStatusID, parStartDate, parEndDate, parcolor).FirstOrDefault();
        //            }
        //        }
        //    }
        //    return objStatus.ErrorCode + objStatus.ErrorMessage;

        //}

        public string RemoveDeal(string TrackingID, string Email,int StatusID,string TransactionID)
        {
            SPErrorViewModel objStatus = new SPErrorViewModel();
            var parTrackingID = new SqlParameter("@TrackingID", TrackingID);
            var parEmail = new SqlParameter("@Email", Email);
            var parStatusID = new SqlParameter("@StatusID", StatusID);
            var parTransactionID = new SqlParameter("@TransactionID", TransactionID);

            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspMstLiveDealDelete @TrackingID,@Email,@StatusID,@TransactionID", parTrackingID, parEmail, parStatusID, parTransactionID).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }

        public utblMstTrackDeal GetLiveDealDetailsByID(string ID, string TransactionID)
        {
            utblMstTrackDeal obj = new utblMstTrackDeal();
            if (ID == null || ID == "") { }
            else
            {
               
                var parTrackingID = new SqlParameter("@TrackingID", ID);
                var parTransactionID = new SqlParameter("@TransactionID", TransactionID);
                obj = objDB.Database.SqlQuery<utblMstTrackDeal>("udspMstBuyerDocumentFirstRowSelect @TrackingID,@TransactionID", parTrackingID, parTransactionID).FirstOrDefault();
              
            }
            return obj;
        }

        public DealTracker GetLiveDealDetailsByIDVersion2(string ID, string TransactionID)
        {
            DealTracker obj = new DealTracker();
            if (ID == null || ID == "") { }
            else
            {

                var parTrackingID = new SqlParameter("@TrackingID", ID);
                var parTransactionID = new SqlParameter("@TransactionID", TransactionID);
                obj = objDB.Database.SqlQuery<DealTracker>("udspMstBuyerDocumentFirstRowSelect @TrackingID,@TransactionID", parTrackingID, parTransactionID).FirstOrDefault();

            }
            return obj;
        }


        public DealTracker GetLiveSellerDealDetailsByID(string ID, string TransactionID)
        {
            DealTracker obj = new DealTracker();
            if (ID == null || ID == "") { }
            else
            {
                var parSellerTrackingID = new SqlParameter("@SellerTrackingID", ID);
                var parTransactionID = new SqlParameter("@TransactionID", TransactionID);
                obj = objDB.Database.SqlQuery<DealTracker>("udspMstSellerDocumentFirstRowSelect @SellerTrackingID,@TransactionID", parSellerTrackingID, parTransactionID).FirstOrDefault();
            }
                return obj;

        }

        public utblMstTrackDealDoc GetLiveDealDocDetailsByID(string ID)
        {
            utblMstTrackDealDoc objItem = objDB.utblMstTrackDealDocs.FirstOrDefault(p => p.DealTrackDocID == ID);
            return objItem;
        }


        public IEnumerable<MstBuyerDocumentListView> BuyerDocumentListView(string Email, string TransactionID)
        {
            List<MstBuyerDocumentListView> objList = new List<MstBuyerDocumentListView>();
            var parEmail = new SqlParameter("@Email", Email);
            var parTransactionID = new SqlParameter("@TransactionID", TransactionID);

            objList = objDB.Database.SqlQuery<MstBuyerDocumentListView>("udspMstBuyerDocumentSelect @Email,@TransactionID", parEmail, parTransactionID).ToList();
            return objList;
        }
        public IEnumerable<MstBuyerDocumentListView> BuyerDocumentListViewVersion2(string Email, string TransactionID)
        {
            List<MstBuyerDocumentListView> objList = new List<MstBuyerDocumentListView>();
            var parEmail = new SqlParameter("@Email", Email);
            var parTransactionID = new SqlParameter("@TransactionID", TransactionID);

            objList = objDB.Database.SqlQuery<MstBuyerDocumentListView>("udspMstBuyerDocumentSelectVersion2 @Email,@TransactionID", parEmail, parTransactionID).ToList();
            return objList;
        }

        public IEnumerable<MstSellerDocumentListView> SellerDocumentListViewVersion2(string Email, string TransactionID)
        {
            List<MstSellerDocumentListView> objList = new List<MstSellerDocumentListView>();
            var parEmail = new SqlParameter("@Email", Email);
            var parTransactionID = new SqlParameter("@TransactionID", TransactionID);

            objList = objDB.Database.SqlQuery<MstSellerDocumentListView>("udspMstSellerDocumentSelectVersio2 @Email,@TransactionID", parEmail, parTransactionID).ToList();
            return objList;
        }

        public IEnumerable<utblMstTrackDealDoc> GetLiveDealDocList(string ID)
        {
            List<utblMstTrackDealDoc> objList = new List<utblMstTrackDealDoc>();
            objList = objDB.utblMstTrackDealDocs.Where(x => x.TrackingID == ID).ToList();
            return objList;
        }

        public string SaveTrackDealDoc(utblMstTrackDealDoc ItemData)
        {
            DateTime dt = DateTime.Now;
            var CurrentYear = dt.ToString("yyyy");
            SPErrorViewModel objStatus = new SPErrorViewModel();
            ItemData.DealTrackDocID = CurrentYear + objUtil.generateUniqueCode("utblMstTrackDealDocs");
            var parDealTrackDocID = new SqlParameter("@DealTrackDocID", ItemData.DealTrackDocID);
            var parTrackingID = new SqlParameter("@TrackingID", ItemData.TrackingID);
            var parTransactionID = new SqlParameter("@TransactionID", ItemData.TransactionID);

            var parDocID = new SqlParameter("@DocID", ItemData.DocID);
            var parStatusID = new SqlParameter("@StatusID", ItemData.StatusID);


            var parTrackDocFile = new SqlParameter("@TrackDocFile", ItemData.TrackDocFile ?? "");
            var parEmail = new SqlParameter("@Email", ItemData.Email);
            var parUpdatedOn = new SqlParameter("@UpdatedOn", System.DateTime.Now);

            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspMstLiveDealDocInsert @DealTrackDocID, @TrackingID,@TransactionID,@DocID,@StatusID, @TrackDocFile,@Email,@UpdatedOn", parDealTrackDocID, parTrackingID, parTransactionID, parDocID, parStatusID, parTrackDocFile,parEmail, parUpdatedOn).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }

        public string SaveTrackDealDocVersion2(utblMstTrackDealDoc ItemData, utblMstBuyerDocument BuyerDoc)
        {
            DateTime dt = DateTime.Now;
            var CurrentYear = dt.ToString("yyyy");
            SPErrorViewModel objStatus = new SPErrorViewModel();
            ItemData.DealTrackDocID = CurrentYear + objUtil.generateUniqueCode("utblMstTrackDealDocs");
            var parDealTrackDocID = new SqlParameter("@DealTrackDocID", ItemData.DealTrackDocID);
            var parTrackingID = new SqlParameter("@TrackingID", ItemData.TrackingID);
            var parTransactionID = new SqlParameter("@TransactionID", ItemData.TransactionID);

            var parDocID = new SqlParameter("@DocID", ItemData.DocID);
            var parStatusID = new SqlParameter("@StatusID", ItemData.StatusID);


            var parTrackDocFile = new SqlParameter("@TrackDocFile", ItemData.TrackDocFile ?? "");
            var parEmail = new SqlParameter("@Email", ItemData.Email);
            var parUpdatedOn = new SqlParameter("@UpdatedOn", System.DateTime.Now);
            var parTitle = new SqlParameter("@Title",string.IsNullOrEmpty(BuyerDoc.Title)?"":BuyerDoc.Title);
            var parDescription = new SqlParameter("@Description", string.IsNullOrEmpty(BuyerDoc.Description) ? "" : BuyerDoc.Description);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspMstLiveDealDocInsertVersion2 @DealTrackDocID, @TrackingID,@TransactionID,@DocID,@StatusID, @TrackDocFile,@Email,@UpdatedOn,@Title,@Description", parDealTrackDocID, parTrackingID, parTransactionID, parDocID, parStatusID, parTrackDocFile, parEmail, parUpdatedOn, parTitle, parDescription).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }


        public string SaveSellerDocVersion2(utblMstSellerTrackDealDoc ItemData, utblMstSellerDocument SellerDoc)
        {
            DateTime dt = DateTime.Now;
            var CurrentYear = dt.ToString("yyyy");
            SPErrorViewModel objStatus = new SPErrorViewModel();
            ItemData.SellerDealDocID = CurrentYear + objUtil.generateUniqueCode("utblMstSellerTrackDealDocs");
            var parSellerDealDocID = new SqlParameter("@SellerDealDocID", ItemData.SellerDealDocID);
            var parSellerTrackingID = new SqlParameter("@SellerTrackingID", ItemData.SellerTrackingID);
            var parTransactionID = new SqlParameter("@TransactionID", ItemData.TransactionID);
            var parSellerDocID = new SqlParameter("@SellerDocID", ItemData.SellerDocID);
            var parSellerStatusID = new SqlParameter("@SellerStatusID", ItemData.SellerStatusID);
            var parTrackDocFile = new SqlParameter("@TrackDocFile", ItemData.TrackDocFile ?? "");
            var parEmail = new SqlParameter("@Email", ItemData.Email);
            var parUpdatedOn = new SqlParameter("@UpdatedOn", System.DateTime.Now);
            var parTitle = new SqlParameter("@Title", string.IsNullOrEmpty(SellerDoc.Title) ? "" : SellerDoc.Title);
            var parDescription = new SqlParameter("@Description", string.IsNullOrEmpty(SellerDoc.Description) ? "" : SellerDoc.Description);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspMstSellerDealDocInsertVersion2 @SellerDealDocID, @SellerTrackingID,@TransactionID,@SellerDocID,@SellerStatusID, @TrackDocFile,@Email,@UpdatedOn,@Title,@Description", parSellerDealDocID, parSellerTrackingID, parTransactionID, parSellerDocID, parSellerStatusID, parTrackDocFile, parEmail, parUpdatedOn, parTitle, parDescription).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }

        public IEnumerable<MstLiveDealView> DealGetPaged(int PageNo, int PageSize, out int TotalRows, string SearchTerm = "")
        {
            List<MstLiveDealView> objList = new List<MstLiveDealView>();
            if (SearchTerm != null)
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
            objList = objDB.Database.SqlQuery<MstLiveDealView>("udspMstLiveDealGetPaged @Start,@PageSize,@SearchTerm,@TotalCount out", parStart, parPageSize, parSearchTerm, spOutput).ToList();
            TotalRows = int.Parse(spOutput.Value.ToString());
            return objList;
        }


        public utblMstTrackDealDoc GetDocDetailsByID(string ID)
        {
            utblMstTrackDealDoc objItem = objDB.utblMstTrackDealDocs.FirstOrDefault(p => p.DealTrackDocID == ID);
            return objItem;
        }


        public string DeleteLiveDealDocument(string DealTrackDocID, string TrackingID)
        {
            SPErrorViewModel objStatus = new SPErrorViewModel();
            var parDealTrackDocID = new SqlParameter("@DealTrackDocID", DealTrackDocID);

            var parTrackingID = new SqlParameter("@TrackingID", TrackingID);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspMstLiveDealDocumentDelete @DealTrackDocID,@TrackingID", parDealTrackDocID, parTrackingID).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }

        public utblMstSellerTrackDealDoc GetSellerDocDetailsByID(string SellerDealDocID)
        {
            utblMstSellerTrackDealDoc objItem = objDB.utblMstSellerTrackDealDocs.FirstOrDefault(p => p.SellerDealDocID == SellerDealDocID);
            return objItem;
        }

        public string DeleteSellerLiveDealDocument(string SellerDealDocID, string SellerTrackingID)
        {
            SPErrorViewModel objStatus = new SPErrorViewModel();
            var parSellerDealDocID = new SqlParameter("@SellerDealDocID", SellerDealDocID);
            var parSellerTrackingID = new SqlParameter("@SellerTrackingID", SellerTrackingID);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspMstSellerDocumentDelete @SellerDealDocID,@SellerTrackingID", parSellerDealDocID, parSellerTrackingID).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }

        public IEnumerable<MstBuyerDocList> BuyerDocumentList(string Email, string TransactionID)
        {
            List<MstBuyerDocList> objList = new List<MstBuyerDocList>();
            var parEmail = new SqlParameter("@Email", Email);
            var parTransactionID = new SqlParameter("@TransactionID", TransactionID);

            objList = objDB.Database.SqlQuery<MstBuyerDocList>("udspBuyerDocumentList @Email,@TransactionID", parEmail, parTransactionID).ToList();
            return objList;
        }
        public IEnumerable<SearchDocumentAutoCompleteViewModel> BuyerDocumentListVersion2(string searchTerm,string Email, string TransactionID)
        {
            List<SearchDocumentAutoCompleteViewModel> objList = new List<SearchDocumentAutoCompleteViewModel>();
            var searchParam = new SqlParameter("@SearchTerm", searchTerm);
            var parEmail = new SqlParameter("@Email", Email);
            var parTransactionID = new SqlParameter("@TransactionID", TransactionID);

            objList = objDB.Database.SqlQuery<SearchDocumentAutoCompleteViewModel>("udspBuyerDocumentListVersion2 @SearchTerm,@Email,@TransactionID", searchParam, parEmail, parTransactionID).ToList();
            return objList;
        }

        public IEnumerable<SearchDocumentAutoCompleteViewModel> SellerDocumentListViewVersion2(string searchTerm, string Email, string TransactionID)
        {
            List<SearchDocumentAutoCompleteViewModel> objList = new List<SearchDocumentAutoCompleteViewModel>();
            var searchParam = new SqlParameter("@SearchTerm", searchTerm);
            var parEmail = new SqlParameter("@Email", Email);
            var parTransactionID = new SqlParameter("@TransactionID", TransactionID);

            objList = objDB.Database.SqlQuery<SearchDocumentAutoCompleteViewModel>("udspMstSellerDocumentSelectVersion2 @SearchTerm,@Email,@TransactionID", searchParam, parEmail, parTransactionID).ToList();
            return objList;
        }

        public UserProfileView GetUserDetails(string ClientID)
        {
            var model = new UserProfileView();
            var parClientID = new SqlParameter("@ClientID", ClientID);
            model = objDB.Database.SqlQuery<UserProfileView>("udspDealUserProfileSelect @ClientID", parClientID).FirstOrDefault();
            return model;
        }
        //added by sonika
        public string RemoveDealEvent(int Id, string Email, string TransactionID)
        {
            SPErrorViewModel objStatus = new SPErrorViewModel();
            var parId = new SqlParameter("@Id", Id);
            var parEmail = new SqlParameter("@Email", Email);
            var parTransactionID = new SqlParameter("@TransactionID", TransactionID);

            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspMstLiveDealDeletevent @Id,@Email,@TransactionID", parId, parEmail, parTransactionID).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }

        public SPErrorViewModel RemoveDealEventVersion2(string Id, string Email, string TransactionID)
        {
            SPErrorViewModel objStatus = new SPErrorViewModel();
            var parId = new SqlParameter("@Id", Id);
            var parEmail = new SqlParameter("@Email", Email);
            var parTransactionID = new SqlParameter("@TransactionID", TransactionID);

            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspMstLiveDealDeleteventVersion2 @Id,@Email,@TransactionID", parId, parEmail, parTransactionID).FirstOrDefault();
            return objStatus;
        }

        public SPErrorViewModel RemoveSellerDealEventVersion2(string Id, string Email, string TransactionID)
        {
            SPErrorViewModel objStatus = new SPErrorViewModel();
            var parId = new SqlParameter("@Id", Id);
            var parEmail = new SqlParameter("@Email", Email);
            var parTransactionID = new SqlParameter("@TransactionID", TransactionID);

            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspMstLiveSellerDealDeleteventVersion2 @Id,@Email,@TransactionID", parId, parEmail, parTransactionID).FirstOrDefault();
            return objStatus;
        }

        //Client Document
        //public IEnumerable<ClientDealDocuments> GetClientDealDocumentById(string TransactionID)
        //{
        //    List<ClientDealDocuments> objList = new List<ClientDealDocuments>();
        //    var parTransactionID = new SqlParameter("@TransactionID", TransactionID);
        //    objList = objDB.Database.SqlQuery<ClientDealDocuments>("udspMstClientDealDocumentSelect @TransactionID", parTransactionID).ToList();
        //    return objList;
        //}

        //Client Document
        public IEnumerable<ClientDealDocumentsView> GetClientDealDocumentById(string TransactionID)
        {
            List<ClientDealDocumentsView> objList = new List<ClientDealDocumentsView>();
            var parTransactionID = new SqlParameter("@TransactionID", TransactionID);
            objList = objDB.Database.SqlQuery<ClientDealDocumentsView>("udspMstClientDealDocumentSelectNew @TransactionID", parTransactionID).ToList();
            return objList;
        }

        //New Version
        public IEnumerable<ClientDealDocumentsView> GetClientDealDocumentByIdVersion2(string TransactionID)
        {
            List<ClientDealDocumentsView> objList = new List<ClientDealDocumentsView>();
            var parTransactionID = new SqlParameter("@TransactionID", TransactionID);
            objList = objDB.Database.SqlQuery<ClientDealDocumentsView>("udspMstClientDealDocumentSelectVersion2 @TransactionID", parTransactionID).ToList();
            return objList;
        }

        public string DeleteClientDealDocument(string DocId)
        {
            SPErrorViewModel objStatus = new SPErrorViewModel();
            var parDealTrackDocID = new SqlParameter("@ClientDocId", DocId);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("ClientDealDocumentDeleteNew @ClientDocId", parDealTrackDocID).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }

        //public ClientDealDocuments GetClientDocDetailsByID(int ID)
        //{
        //    ClientDealDocuments objItem = objDB.ClientDealDocuments.FirstOrDefault(p => p.Id == ID);
        //    return objItem;
        //}

        public ClientDealDocmuent GetClientDocDetailsByID(string ID)
        {
            ClientDealDocmuent objItem = objDB.ClientDealDocmuent.FirstOrDefault(p => p.ClientDocId == ID);
            return objItem;
        }


        public string GetClientFileName(string ID)
        {

            var parClientDocId = new SqlParameter("@ClientDocId", ID);
            string Filename = objDB.Database.SqlQuery<string>("GetClientDocFileName @ClientDocId", parClientDocId).FirstOrDefault();
            return Filename;
        }


        public IEnumerable<SearchAutoCompleteViewModel> VendorTypeAutoComplete(string searchTerm)
        {
            List<SearchAutoCompleteViewModel> obj = new List<SearchAutoCompleteViewModel>();
            var searchParam = new SqlParameter("@SearchTerm", searchTerm);
            obj = objDB.Database.SqlQuery<SearchAutoCompleteViewModel>("udspVendorCategoryGetPagedAutoComplete @SearchTerm", searchParam).ToList();
            return obj;
        }

        public IEnumerable<SearchMultiAutoCompleteViewModel> ClientVendorAutoComplete(string searchTerm, string VendorType)
        {
            List<SearchMultiAutoCompleteViewModel> obj = new List<SearchMultiAutoCompleteViewModel>();
            var searchParam = new SqlParameter("@SearchTerm", searchTerm);
            var parVendorType = new SqlParameter("@CategoryName", VendorType);
            obj = objDB.Database.SqlQuery<SearchMultiAutoCompleteViewModel>("udspGetClientVendorGAutoCompleteVersion2 @SearchTerm,@CategoryName", searchParam, parVendorType).ToList();
            return obj;
        }

    
        public List<VendorContactsAutocompleteViewModel> GetVendorContact(string searchTerm, string VendorId)
        {
            List<VendorContactsAutocompleteViewModel> obj = new List<VendorContactsAutocompleteViewModel>();
            var searchParam = new SqlParameter("@SearchTerm", searchTerm);
            var parVendor = new SqlParameter("@VendorId", VendorId);
            obj = objDB.Database.SqlQuery<VendorContactsAutocompleteViewModel>("udspVendorContactAutocompleteVersion2 @SearchTerm,@VendorId", searchParam, parVendor).ToList();
            return obj;
        }

        public IEnumerable<DealVendorView> GetDealVendorList(string Email, string TransactionID)
        {
            List<DealVendorView> objList = new List<DealVendorView>();
            var parEmail = new SqlParameter("@Email", Email);
            var parTransactionID = new SqlParameter("@Transaction_Id", TransactionID);

            objList = objDB.Database.SqlQuery<DealVendorView>("udspGetDealVendorsVersion2 @Email,@Transaction_Id", parEmail, parTransactionID).ToList();
            return objList;
        }

        //public DealVendor GetDealVendorByID(string DealVendorId)
        //{
        //    DealVendor objItem = objDB.DealVendor.FirstOrDefault(p => p.DealVendorId == DealVendorId);
        //    return objItem;
        //}

        public string DeleteDealVendor(string DealVendorId)
        {
            SPErrorViewModel objStatus = new SPErrorViewModel();
            var parDealVendorId = new SqlParameter("@DealVendorId", DealVendorId);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspDealVendorDelete @DealVendorId", parDealVendorId).FirstOrDefault();
            return objStatus.ErrorCode + objStatus.ErrorMessage;
        }

        
        public string SaveDealVendorVersion2(Vendor model,VendorContacts contacts, string CreatedBy,string ClientEmail, string TransactionId)
        {
            DateTime dt = DateTime.Now;
            var CurrentYear = dt.ToString("yyyy");
            SPErrorViewModel objStatus = new SPErrorViewModel();
            model.VendorId = CurrentYear + objUtil.generateUniqueCode("Vendor");
            var DealVendorId = CurrentYear + objUtil.generateUniqueCode("DealVendor");
            var vendorCategoryId = CurrentYear + objUtil.generateUniqueCode("VendorCategory");
            var parVendorId = new SqlParameter("@VendorId", model.VendorId);
            var parTitle = new SqlParameter("@Title", model.Title);
            //var parSubTitle = new SqlParameter("@SubTitle", model.SubTitle == null ? "" : model.SubTitle);
            var parPhone = new SqlParameter("@Phone", model.Phone == null ? "" : model.Phone);
            var parEmail = new SqlParameter("@Email", model.Email == null ? "" : model.Email);
            //var parWeb = new SqlParameter("@WebsiteLink", model.WebsiteLink == null ? "" : model.WebsiteLink);
            //var parAbout = new SqlParameter("@About", model.About == null ? "" : model.About);
            //var parVendorImage = new SqlParameter("@VendorImage", DBNull.Value);
            //var parLocation = new SqlParameter("@Location", model.Location == null ? "" : model.Location);
            //if (model.VendorImage != null)
            //    parVendorImage = new SqlParameter("@VendorImage", model.VendorId.ToString() + model.VendorImage.ToString());
            var parCreatedBy = new SqlParameter("@CreatedBy", CreatedBy);
            var parCategory_Id = new SqlParameter("@Category_Name", model.Category_Id);
            var parTransactionId = new SqlParameter("@TransactionId", TransactionId);
            var parDealVendorId = new SqlParameter("@DealVendorId", DealVendorId);
            var parvendorCategory = new SqlParameter("@VendorCategoryId", vendorCategoryId);
            var parVendorContactId = new SqlParameter("@VendorContactId", contacts.VendorContactId);
            var parContactName = new SqlParameter("@ContactName", contacts.ContactName == null ? "" : contacts.ContactName);
            var parContactTitle = new SqlParameter("@ContactTitle", contacts.ContactTitle == null ? "" : contacts.ContactTitle);
            var parContactPhone = new SqlParameter("@ContactPhone", contacts.ContactPhone == null ? "" : contacts.ContactPhone);
            var parClientEmail = new SqlParameter("@ClientEmail", ClientEmail);
            objStatus = objDB.Database.SqlQuery<SPErrorViewModel>("udspVendorInsertDealVendorVersion2 @VendorId,@Title,@Category_Name,@Phone,@Email,@CreatedBy,@TransactionId,@DealVendorId,@VendorCategoryId,@VendorContactId,@ContactName,@ContactTitle,@ContactPhone,@ClientEmail", parVendorId, parTitle, parCategory_Id, parPhone, parEmail, parCreatedBy, parTransactionId, parDealVendorId, parvendorCategory,parVendorContactId,parContactName,parContactTitle,parContactPhone,parClientEmail).FirstOrDefault();
            return objStatus.ErrorMessage;
        }
    }

}
