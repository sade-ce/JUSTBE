using HtmlAgilityPack;
using RealEstate.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace RealEstate.Web.Areas.Coordinator.Controllers
{
    [Authorize]

    public class ToolKitController : Controller
    {
        // GET: Coordinator/ToolKit
        MstAffordabilityCalculator ObjModel = new MstAffordabilityCalculator();

        public ActionResult NetProceeds()
        {
            ViewBag.ActiveURL = "/Coordinator/ToolKit/NetProceeds";

            List<PropertyType> ObjPropertyType = new List<PropertyType>()
            {
                new PropertyType { ddlPropertyType="single"},
                new PropertyType { ddlPropertyType="condo"}
            };

            List<TranferTaxesPayer> ObjTax = new List<TranferTaxesPayer>()
            {
                new TranferTaxesPayer {ddlTranferTaxesPayingBy="Split" },
                new TranferTaxesPayer {ddlTranferTaxesPayingBy="Buyer" },
                new TranferTaxesPayer {ddlTranferTaxesPayingBy="Seller" }

            };
            List<MortgageNumber> ObjMortgage = new List<MortgageNumber>()
            {
                new MortgageNumber {ddlNumberMortgages=1 },
                new MortgageNumber {ddlNumberMortgages=2 },
                new MortgageNumber {ddlNumberMortgages=3 },
                new MortgageNumber {ddlNumberMortgages=4 },
                new MortgageNumber {ddlNumberMortgages=5 }

            };

            List<Radio> ObjFRPTA = new List<Radio>()
            {
                new Radio {FIRPTA="yes" },
                new Radio {FIRPTA="No" }

            };
            ObjModel.NetProceed = new NetProceed();
            ObjModel.NetProceed.__EVENTTARGET = "ddlState";
            ObjModel.NetProceed.__EVENTARGUMENT = "";
            ObjModel.NetProceed.__LASTFOCUS = "";
            ObjModel.NetProceed.__VIEWSTATE = "/wEPDwULLTIwMjc2MDYyMzAPZBYCAgEPZBYCAgEPZBYGAgQPEA8WAh4LXyFEYXRhQm91bmRnZBAVBQotIHNlbGVjdCAtFERpc3RyaWN0IG9mIENvbHVtYmlhCE1hcnlsYW5kCFZpcmdpbmlhB0Zsb3JpZGEVBQAkZjg1YjY1MzYtZmVjMy00NTFhLWI0OWMtMzdjZDdiNjllYmUyJDIyYWQwMzRiLTQ0OGQtNGRlNC04OWM3LWQ4NTNlMmU5ZWQ2NiQyMzc4MWRkNi0yOGRiLTQ5ZjMtOWZhMC0wZGQxZDU1MzEwNmIkNzhhZDgwOTctOWFiMC00MDFmLWI3ZDMtMzQzM2Y5ZjhhMzAxFCsDBWdnZ2dnFgFmZAIaDxBkZBYBAgFkAlIPFgIeB1Zpc2libGVoZBgBBR5fX0NvbnRyb2xzUmVxdWlyZVBvc3RCYWNrS2V5X18WAwUMcmJ0RklSUFRBWWVzBQxyYnRGSVJQVEFZZXMFC3JidEZJUlBUQU5vVqS/tkKQWWlxrLAzUeZHnpkqsSiNNyATTBzc3PcGasI=";
            ObjModel.NetProceed.__VIEWSTATEGENERATOR = "A4881394";
            ObjModel.NetProceed.__EVENTVALIDATION = "/wEdACLK2/IfdSu0c085qRs0Ns02AKqfG/9TC1GPPf140Mq8x5jTzyT+aaMw5CkIc7OLwMSzNqoLoeZo9b9soyFviHVBBmScCHeXh9j/NRJ6X7snCI0sp87Zck6sa84tfzUWbH1wwt/F76q0s/A5oCAgD8pwHa/7iBjkC1dzFpoOjwB7hJbc1L4UEla2inBqS9V5s71pKczixdbCsGRvN0HWF33/zyFtOd8zpavHUK+/6mx8ghVN7CpYz2TIysvPrk5f7vTCy4JobGCAEuP7CpzhgkPatpQxUj4CIiW6Y/RR5kEzgAGe9+PDPQYQd/iI9JjKFbkcFgf0Pj9jQHT4z62rl+Gk3Ow9bqPbOmnARzRLQlzYytliJyKc04quxOpnBO0as930aeu1okMnVFERCgxhKU6UJv5DxE5rCy7Sg1VSVU49Wjl6xZiNuFwC1l8jrp9pqCH2xrDyJYv1ceNcXGLz+QrC+v9a6sbeX5PfmcBkigmsqbULnVPInL+py+NFoDymtqXWJGB85X+Kevl28EdB2xWIckevubHiyc4DZdpfHlMyzlBqCgg3QwfidKUOMhQi/TW8b6cuEnwxH1ZP3bpiLjpPD+GwMarsaJH/XKbHbsElkhoxEgKa4rXcafHQwcF52jgDYVSdZHsXww/T/svUUlFHTJDzxHvvfizjB2V5d79Da8Pt6lo5RoWeATW1fML9hij+FRaBdAtxB8/Y28n4+CdQHqbBc0CSS40NAFYhOasPNxA9GFOGw9cCf+7r9aP/nCc=";
            ObjModel.NetProceed.ddlState = "f85b6536-fec3-451a-b49c-37cd7b69ebe2";

            ObjModel.PropertyType = ObjPropertyType;
            ObjModel.TranferTaxesPayer = ObjTax;
            ObjModel.MortgageNumber = ObjMortgage;
            ObjModel.FIRPTA = ObjFRPTA;
            return View(ObjModel);
        }

        [HttpPost]
        public ActionResult NetProceeds(MstAffordabilityCalculator ItemData)
        {
            ViewBag.ActiveURL = "/Coordinator/ToolKit/NetProceeds";

            if (ModelState.IsValid)
            {
                var Data = DataProcessor(ItemData);
                TempData["HTML"] = Data;
                if (string.IsNullOrEmpty(Data))
                {
                    ModelState.AddModelError("please check your entries and try again", "");

                }
                else
                {
                    return RedirectToAction("NetProceedEstimate");
                }

            }
            List<PropertyType> ObjPropertyType = new List<PropertyType>()
            {
                new PropertyType { ddlPropertyType="single"},
                new PropertyType { ddlPropertyType="condo"}
            };

            List<TranferTaxesPayer> ObjTax = new List<TranferTaxesPayer>()
            {
                new TranferTaxesPayer {ddlTranferTaxesPayingBy="Split" },
                new TranferTaxesPayer {ddlTranferTaxesPayingBy="Buyer" },
                new TranferTaxesPayer {ddlTranferTaxesPayingBy="Seller" }

            };
            List<MortgageNumber> ObjMortgage = new List<MortgageNumber>()
            {
                new MortgageNumber {ddlNumberMortgages=1 },
                new MortgageNumber {ddlNumberMortgages=2 },
                new MortgageNumber {ddlNumberMortgages=3 },
                new MortgageNumber {ddlNumberMortgages=4 },
                new MortgageNumber {ddlNumberMortgages=5 }

            };

            List<Radio> ObjFRPTA = new List<Radio>()
            {
                new Radio {FIRPTA="yes" },
                new Radio {FIRPTA="No" }

            };
            ItemData.PropertyType = ObjPropertyType;
            ItemData.TranferTaxesPayer = ObjTax;
            ItemData.MortgageNumber = ObjMortgage;
            ItemData.FIRPTA = ObjFRPTA;

            return View(ItemData);
        }


        public string DataProcessor(MstAffordabilityCalculator ItemData)
        {
            string responsebody = "";

            try
            {
                using (WebClient wc = new WebClient())
                {
                    var reqparm = new System.Collections.Specialized.NameValueCollection();

                    reqparm.Add("__EVENTTARGET", "ddlState");
                    reqparm.Add("__EVENTARGUMENT", "");
                    reqparm.Add("__LASTFOCUS", "");
                    reqparm.Add("__VIEWSTATE", "/wEPDwULLTIwMjc2MDYyMzAPZBYCAgEPZBYCAgEPZBYGAgQPEA8WAh4LXyFEYXRhQm91bmRnZBAVBQotIHNlbGVjdCAtFERpc3RyaWN0IG9mIENvbHVtYmlhCE1hcnlsYW5kCFZpcmdpbmlhB0Zsb3JpZGEVBQAkZjg1YjY1MzYtZmVjMy00NTFhLWI0OWMtMzdjZDdiNjllYmUyJDIyYWQwMzRiLTQ0OGQtNGRlNC04OWM3LWQ4NTNlMmU5ZWQ2NiQyMzc4MWRkNi0yOGRiLTQ5ZjMtOWZhMC0wZGQxZDU1MzEwNmIkNzhhZDgwOTctOWFiMC00MDFmLWI3ZDMtMzQzM2Y5ZjhhMzAxFCsDBWdnZ2dnFgFmZAIaDxBkZBYBAgFkAlIPFgIeB1Zpc2libGVoZBgBBR5fX0NvbnRyb2xzUmVxdWlyZVBvc3RCYWNrS2V5X18WAwUMcmJ0RklSUFRBWWVzBQxyYnRGSVJQVEFZZXMFC3JidEZJUlBUQU5vVqS/tkKQWWlxrLAzUeZHnpkqsSiNNyATTBzc3PcGasI=");
                    reqparm.Add("__VIEWSTATEGENERATOR", "A4881394");
                    reqparm.Add("__EVENTVALIDATION", "/wEdACLK2/IfdSu0c085qRs0Ns02AKqfG/9TC1GPPf140Mq8x5jTzyT+aaMw5CkIc7OLwMSzNqoLoeZo9b9soyFviHVBBmScCHeXh9j/NRJ6X7snCI0sp87Zck6sa84tfzUWbH1wwt/F76q0s/A5oCAgD8pwHa/7iBjkC1dzFpoOjwB7hJbc1L4UEla2inBqS9V5s71pKczixdbCsGRvN0HWF33/zyFtOd8zpavHUK+/6mx8ghVN7CpYz2TIysvPrk5f7vTCy4JobGCAEuP7CpzhgkPatpQxUj4CIiW6Y/RR5kEzgAGe9+PDPQYQd/iI9JjKFbkcFgf0Pj9jQHT4z62rl+Gk3Ow9bqPbOmnARzRLQlzYytliJyKc04quxOpnBO0as930aeu1okMnVFERCgxhKU6UJv5DxE5rCy7Sg1VSVU49Wjl6xZiNuFwC1l8jrp9pqCH2xrDyJYv1ceNcXGLz+QrC+v9a6sbeX5PfmcBkigmsqbULnVPInL+py+NFoDymtqXWJGB85X+Kevl28EdB2xWIckevubHiyc4DZdpfHlMyzlBqCgg3QwfidKUOMhQi/TW8b6cuEnwxH1ZP3bpiLjpPD+GwMarsaJH/XKbHbsElkhoxEgKa4rXcafHQwcF52jgDYVSdZHsXww/T/svUUlFHTJDzxHvvfizjB2V5d79Da8Pt6lo5RoWeATW1fML9hij+FRaBdAtxB8/Y28n4+CdQHqbBc0CSS40NAFYhOasPNxA9GFOGw9cCf+7r9aP/nCc=");
                    reqparm.Add("ddlState", "f85b6536-fec3-451a-b49c-37cd7b69ebe2");
                    reqparm.Add("ddlPropertyType", ItemData.NetProceed.ddlPropertyType);
                    reqparm.Add("txtSalePrice", ItemData.NetProceed.txtSalePrice.ToString());
                    reqparm.Add("txtLoanPayOffs", ItemData.NetProceed.txtLoanPayOffs.ToString());
                    reqparm.Add("txtLoanPayOffs2", ItemData.NetProceed.txtLoanPayOffs2.ToString());
                    reqparm.Add("ddlNumberMortgages", ItemData.NetProceed.ddlNumberMortgages.ToString());
                    reqparm.Add("ddlTranferTaxesPayingBy", ItemData.NetProceed.ddlTranferTaxesPayingBy);
                    reqparm.Add("txtSellerCredit", ItemData.NetProceed.txtSellerCredit.ToString());
                    reqparm.Add("txtAdminFee", ItemData.NetProceed.txtAdminFee.ToString());
                    reqparm.Add("txtCommission", ItemData.NetProceed.txtCommission.ToString());
                    reqparm.Add("txtSettlementDate", ItemData.NetProceed.txtSettlementDate);
                    reqparm.Add("txtRealEstatePropertyTax", ItemData.NetProceed.txtRealEstatePropertyTax.ToString());
                    reqparm.Add("txtTermiteInspection", ItemData.NetProceed.txtTermiteInspection.ToString() == "" ? "0" : ItemData.NetProceed.txtTermiteInspection.ToString());
                    reqparm.Add("txtWarranty", ItemData.NetProceed.txtWarranty.ToString() == "" ? "0" : ItemData.NetProceed.txtWarranty.ToString());
                    reqparm.Add("txtOtherCharges", ItemData.NetProceed.txtOtherCharges.ToString());
                    reqparm.Add("txtHOAFee", ItemData.NetProceed.txtHOAFee.ToString());
                    reqparm.Add("FIRPTA", ItemData.NetProceed.FIRPTA == "yes" ? "rbtFIRPTAYes" : "rbtFIRPTANo");
                    reqparm.Add("btnGenerateQuote", "Get Quote");
                    byte[] responsebytes = wc.UploadValues("https://tools.federaltitle.com/titleagents/estimate/Default.aspx", "POST", reqparm);
                    responsebody = Encoding.UTF8.GetString(responsebytes);
                }


            }
            catch (Exception)
            {
                responsebody = "";
            }

            return responsebody;
        }


        public ActionResult NetProceedEstimate()
        {
            ViewBag.ActiveURL = "/Coordinator/ToolKit/NetProceeds";

            if (TempData["HTML"] == null)
            {
                return RedirectToAction("NetProceeds");
            }

            //ViewBag.HTML = Helpers.RemoveUnwantedHtmlTags(TempData["HTML"].ToString(), Tag);

            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(TempData["HTML"].ToString());


            var content = document.DocumentNode.SelectSingleNode("//div[@id='pnlResults']") == null ? document.DocumentNode.InnerHtml : document.DocumentNode.SelectSingleNode("//div[@id='pnlResults']").InnerHtml;
            ViewBag.HTML = content;
            return View();
        }

       


    }
}