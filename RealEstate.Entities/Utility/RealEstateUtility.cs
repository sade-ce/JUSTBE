using RealEstate.Entities.DataAccess;
using RealEstate.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.Utility
{
    public class RealEstateUtility
    {
        EFDBContext objDB = new EFDBContext();

        public string generateUniqueCode(string tablename)
        {
            int SlNo = 0;
            string UniqueCode = "";
            int RangeAssci = 0;

            switch (tablename)
            {
                case "utblMstTrackDeals":
                    utblGenCodeSeed objTrackDeals = objDB.utblGenCodeSeeds.FirstOrDefault(p => p.TableName == tablename);
                    if (objTrackDeals != null)
                    {
                        SlNo = objTrackDeals.CodeSlNo;
                        UniqueCode = objTrackDeals.CharRange;
                        RangeAssci = System.Convert.ToInt32(char.Parse(objTrackDeals.CharRange));
                    }
                    SlNo = SlNo + 1;
                    UniqueCode = UniqueCode + SlNo.ToString("00000");

                    if (SlNo == 999)
                    {
                        SlNo = 0;
                        RangeAssci = RangeAssci + 1;
                        objTrackDeals.CodeSlNo = SlNo;
                        objTrackDeals.CharRange = Char.ConvertFromUtf32(RangeAssci).ToString();
                    }
                    else
                    {
                        objTrackDeals.CodeSlNo = SlNo;
                    }
                    objDB.SaveChanges();
                    return UniqueCode;

                case "utblMstTrackDealDocs":
                    utblGenCodeSeed objTrackDealDoc = objDB.utblGenCodeSeeds.FirstOrDefault(p => p.TableName == tablename);
                    if (objTrackDealDoc != null)
                    {
                        SlNo = objTrackDealDoc.CodeSlNo;
                        UniqueCode = objTrackDealDoc.CharRange;
                        RangeAssci = System.Convert.ToInt32(char.Parse(objTrackDealDoc.CharRange));
                    }
                    SlNo = SlNo + 1;
                    UniqueCode = UniqueCode + SlNo.ToString("00000");

                    if (SlNo == 999)
                    {
                        SlNo = 0;
                        RangeAssci = RangeAssci + 1;
                        objTrackDealDoc.CodeSlNo = SlNo;
                        objTrackDealDoc.CharRange = Char.ConvertFromUtf32(RangeAssci).ToString();
                    }
                    else
                    {
                        objTrackDealDoc.CodeSlNo = SlNo;
                    }
                    objDB.SaveChanges();
                    return UniqueCode;

                case "utblMstClientTrackDealDocs":
                    utblGenCodeSeed objClientTrackDealDoc = objDB.utblGenCodeSeeds.FirstOrDefault(p => p.TableName == tablename);
                    if (objClientTrackDealDoc != null)
                    {
                        SlNo = objClientTrackDealDoc.CodeSlNo;
                        UniqueCode = objClientTrackDealDoc.CharRange;
                        RangeAssci = System.Convert.ToInt32(char.Parse(objClientTrackDealDoc.CharRange));
                    }
                    SlNo = SlNo + 1;
                    UniqueCode = UniqueCode + SlNo.ToString("00000");

                    if (SlNo == 999)
                    {
                        SlNo = 0;
                        RangeAssci = RangeAssci + 1;
                        objClientTrackDealDoc.CodeSlNo = SlNo;
                        objClientTrackDealDoc.CharRange = Char.ConvertFromUtf32(RangeAssci).ToString();
                    }
                    else
                    {
                        objClientTrackDealDoc.CodeSlNo = SlNo;
                    }
                    objDB.SaveChanges();
                    return UniqueCode;

                case "utblMstClientGalleries":
                    utblGenCodeSeed utblMstClientGalleries = objDB.utblGenCodeSeeds.FirstOrDefault(p => p.TableName == tablename);
                    if (utblMstClientGalleries != null)
                    {
                        SlNo = utblMstClientGalleries.CodeSlNo;
                        UniqueCode = utblMstClientGalleries.CharRange;
                        RangeAssci = System.Convert.ToInt32(char.Parse(utblMstClientGalleries.CharRange));
                    }
                    SlNo = SlNo + 1;
                    UniqueCode = UniqueCode + SlNo.ToString("00000");

                    if (SlNo == 999)
                    {
                        SlNo = 0;
                        RangeAssci = RangeAssci + 1;
                        utblMstClientGalleries.CodeSlNo = SlNo;
                        utblMstClientGalleries.CharRange = Char.ConvertFromUtf32(RangeAssci).ToString();
                    }
                    else
                    {
                        utblMstClientGalleries.CodeSlNo = SlNo;
                    }
                    objDB.SaveChanges();
                    return UniqueCode;

                case "utblBlogs":
                    utblGenCodeSeed utblBlog = objDB.utblGenCodeSeeds.FirstOrDefault(p => p.TableName == tablename);
                    if (utblBlog != null)
                    {
                        SlNo = utblBlog.CodeSlNo;
                        UniqueCode = utblBlog.CharRange;
                        RangeAssci = System.Convert.ToInt32(char.Parse(utblBlog.CharRange));
                    }
                    SlNo = SlNo + 1;
                    UniqueCode = UniqueCode + SlNo.ToString("00000");

                    if (SlNo == 999)
                    {
                        SlNo = 0;
                        RangeAssci = RangeAssci + 1;
                        utblBlog.CodeSlNo = SlNo;
                        utblBlog.CharRange = Char.ConvertFromUtf32(RangeAssci).ToString();
                    }
                    else
                    {
                        utblBlog.CodeSlNo = SlNo;
                    }
                    objDB.SaveChanges();
                    return UniqueCode;


                case "utblMstTrackDealMasters":
                    utblGenCodeSeed utblDealMasters = objDB.utblGenCodeSeeds.FirstOrDefault(p => p.TableName == tablename);
                    if (utblDealMasters != null)
                    {
                        SlNo = utblDealMasters.CodeSlNo;
                        UniqueCode = utblDealMasters.CharRange;
                        RangeAssci = System.Convert.ToInt32(char.Parse(utblDealMasters.CharRange));
                    }
                    SlNo = SlNo + 1;
                    UniqueCode = UniqueCode + SlNo.ToString("00000");

                    if (SlNo == 999)
                    {
                        SlNo = 0;
                        RangeAssci = RangeAssci + 1;
                        utblDealMasters.CodeSlNo = SlNo;
                        utblDealMasters.CharRange = Char.ConvertFromUtf32(RangeAssci).ToString();
                    }
                    else
                    {
                        utblDealMasters.CodeSlNo = SlNo;
                    }
                    objDB.SaveChanges();
                    return UniqueCode;


                case "utblMstSellerTrackDeals":
                    utblGenCodeSeed utblSellerTrackDeals = objDB.utblGenCodeSeeds.FirstOrDefault(p => p.TableName == tablename);
                    if (utblSellerTrackDeals != null)
                    {
                        SlNo = utblSellerTrackDeals.CodeSlNo;
                        UniqueCode = utblSellerTrackDeals.CharRange;
                        RangeAssci = System.Convert.ToInt32(char.Parse(utblSellerTrackDeals.CharRange));
                    }
                    SlNo = SlNo + 1;
                    UniqueCode = UniqueCode + SlNo.ToString("00000");

                    if (SlNo == 999)
                    {
                        SlNo = 0;
                        RangeAssci = RangeAssci + 1;
                        utblSellerTrackDeals.CodeSlNo = SlNo;
                        utblSellerTrackDeals.CharRange = Char.ConvertFromUtf32(RangeAssci).ToString();
                    }
                    else
                    {
                        utblSellerTrackDeals.CodeSlNo = SlNo;
                    }
                    objDB.SaveChanges();
                    return UniqueCode;


                case "utblMstSellerTrackDealDocs":
                    utblGenCodeSeed utblSellerTrackDealDocs = objDB.utblGenCodeSeeds.FirstOrDefault(p => p.TableName == tablename);
                    if (utblSellerTrackDealDocs != null)
                    {
                        SlNo = utblSellerTrackDealDocs.CodeSlNo;
                        UniqueCode = utblSellerTrackDealDocs.CharRange;
                        RangeAssci = System.Convert.ToInt32(char.Parse(utblSellerTrackDealDocs.CharRange));
                    }
                    SlNo = SlNo + 1;
                    UniqueCode = UniqueCode + SlNo.ToString("00000");

                    if (SlNo == 999)
                    {
                        SlNo = 0;
                        RangeAssci = RangeAssci + 1;
                        utblSellerTrackDealDocs.CodeSlNo = SlNo;
                        utblSellerTrackDealDocs.CharRange = Char.ConvertFromUtf32(RangeAssci).ToString();
                    }
                    else
                    {
                        utblSellerTrackDealDocs.CodeSlNo = SlNo;
                    }
                    objDB.SaveChanges();
                    return UniqueCode;

                case "utblMstSellerCounterOffers":
                    utblGenCodeSeed utblMstSellerCounterOffers = objDB.utblGenCodeSeeds.FirstOrDefault(p => p.TableName == tablename);
                    if (utblMstSellerCounterOffers != null)
                    {
                        SlNo = utblMstSellerCounterOffers.CodeSlNo;
                        UniqueCode = utblMstSellerCounterOffers.CharRange;
                        RangeAssci = System.Convert.ToInt32(char.Parse(utblMstSellerCounterOffers.CharRange));
                    }
                    SlNo = SlNo + 1;
                    UniqueCode = UniqueCode + SlNo.ToString("00000");

                    if (SlNo == 999)
                    {
                        SlNo = 0;
                        RangeAssci = RangeAssci + 1;
                        utblMstSellerCounterOffers.CodeSlNo = SlNo;
                        utblMstSellerCounterOffers.CharRange = Char.ConvertFromUtf32(RangeAssci).ToString();
                    }
                    else
                    {
                        utblMstSellerCounterOffers.CodeSlNo = SlNo;
                    }
                    objDB.SaveChanges();
                    return UniqueCode;


                case "utblMstClientHomeGalleries":
                    utblGenCodeSeed utblMstClientHomeGalleries = objDB.utblGenCodeSeeds.FirstOrDefault(p => p.TableName == tablename);
                    if (utblMstClientHomeGalleries != null)
                    {
                        SlNo = utblMstClientHomeGalleries.CodeSlNo;
                        UniqueCode = utblMstClientHomeGalleries.CharRange;
                        RangeAssci = System.Convert.ToInt32(char.Parse(utblMstClientHomeGalleries.CharRange));
                    }
                    SlNo = SlNo + 1;
                    UniqueCode = UniqueCode + SlNo.ToString("00000");

                    if (SlNo == 999)
                    {
                        SlNo = 0;
                        RangeAssci = RangeAssci + 1;
                        utblMstClientHomeGalleries.CodeSlNo = SlNo;
                        utblMstClientHomeGalleries.CharRange = Char.ConvertFromUtf32(RangeAssci).ToString();
                    }
                    else
                    {
                        utblMstClientHomeGalleries.CodeSlNo = SlNo;
                    }
                    objDB.SaveChanges();
                    return UniqueCode;

                case "VendorCategory":
                    utblGenCodeSeed VendorCategory = objDB.utblGenCodeSeeds.FirstOrDefault(p => p.TableName == tablename);
                    if (VendorCategory != null)
                    {
                        SlNo = VendorCategory.CodeSlNo;
                        UniqueCode = VendorCategory.CharRange;
                        RangeAssci = System.Convert.ToInt32(char.Parse(VendorCategory.CharRange));
                    }
                    SlNo = SlNo + 1;
                    UniqueCode = UniqueCode + SlNo.ToString("00000");

                    if (SlNo == 999)
                    {
                        SlNo = 0;
                        RangeAssci = RangeAssci + 1;
                        VendorCategory.CodeSlNo = SlNo;
                        VendorCategory.CharRange = Char.ConvertFromUtf32(RangeAssci).ToString();
                    }
                    else
                    {
                        VendorCategory.CodeSlNo = SlNo;
                    }
                    objDB.SaveChanges();
                    return UniqueCode;

                case "Vendor":
                    utblGenCodeSeed Vendor = objDB.utblGenCodeSeeds.FirstOrDefault(p => p.TableName == tablename);
                    if (Vendor != null)
                    {
                        SlNo = Vendor.CodeSlNo;
                        UniqueCode = Vendor.CharRange;
                        RangeAssci = System.Convert.ToInt32(char.Parse(Vendor.CharRange));
                    }
                    SlNo = SlNo + 1;
                    UniqueCode = UniqueCode + SlNo.ToString("00000");

                    if (SlNo == 999)
                    {
                        SlNo = 0;
                        RangeAssci = RangeAssci + 1;
                        Vendor.CodeSlNo = SlNo;
                        Vendor.CharRange = Char.ConvertFromUtf32(RangeAssci).ToString();
                    }
                    else
                    {
                        Vendor.CodeSlNo = SlNo;
                    }
                    objDB.SaveChanges();
                    return UniqueCode;

                case "DealVendor":
                    utblGenCodeSeed DealVendor = objDB.utblGenCodeSeeds.FirstOrDefault(p => p.TableName == tablename);
                    if (DealVendor != null)
                    {
                        SlNo = DealVendor.CodeSlNo;
                        UniqueCode = DealVendor.CharRange;
                        RangeAssci = System.Convert.ToInt32(char.Parse(DealVendor.CharRange));
                    }
                    SlNo = SlNo + 1;
                    UniqueCode = UniqueCode + SlNo.ToString("00000");

                    if (SlNo == 999)
                    {
                        SlNo = 0;
                        RangeAssci = RangeAssci + 1;
                        DealVendor.CodeSlNo = SlNo;
                        DealVendor.CharRange = Char.ConvertFromUtf32(RangeAssci).ToString();
                    }
                    else
                    {
                        DealVendor.CodeSlNo = SlNo;
                    }
                    objDB.SaveChanges();
                    return UniqueCode;
                case "TeamMembers":
                    utblGenCodeSeed TeamMembers = objDB.utblGenCodeSeeds.FirstOrDefault(p => p.TableName == tablename);
                    if (TeamMembers != null)
                    {
                        SlNo = TeamMembers.CodeSlNo;
                        UniqueCode = TeamMembers.CharRange;
                        RangeAssci = System.Convert.ToInt32(char.Parse(TeamMembers.CharRange));
                    }
                    SlNo = SlNo + 1;
                    UniqueCode = UniqueCode + SlNo.ToString("00000");

                    if (SlNo == 999)
                    {
                        SlNo = 0;
                        RangeAssci = RangeAssci + 1;
                        TeamMembers.CodeSlNo = SlNo;
                        TeamMembers.CharRange = Char.ConvertFromUtf32(RangeAssci).ToString();
                    }
                    else
                    {
                        TeamMembers.CodeSlNo = SlNo;
                    }
                    objDB.SaveChanges();
                    return UniqueCode;
                case "ClientDealDocument":
                    utblGenCodeSeed ClientDocument = objDB.utblGenCodeSeeds.FirstOrDefault(p => p.TableName == tablename);
                    if (ClientDocument != null)
                    {
                        SlNo = ClientDocument.CodeSlNo;
                        UniqueCode = ClientDocument.CharRange;
                        RangeAssci = System.Convert.ToInt32(char.Parse(ClientDocument.CharRange));
                    }
                    SlNo = SlNo + 1;
                    UniqueCode = UniqueCode + SlNo.ToString("00000");

                    if (SlNo == 999)
                    {
                        SlNo = 0;
                        RangeAssci = RangeAssci + 1;
                        ClientDocument.CodeSlNo = SlNo;
                        ClientDocument.CharRange = Char.ConvertFromUtf32(RangeAssci).ToString();
                    }
                    else
                    {
                        ClientDocument.CodeSlNo = SlNo;
                    }
                    objDB.SaveChanges();
                    return UniqueCode;
            }
            return null;
        }
    }
}
