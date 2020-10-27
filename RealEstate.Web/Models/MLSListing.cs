using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RealEstate.Web.Models
{
    public class MLSListing
    {
        public RootObject SearchResults { get; set; }
    }
        public class AdsResults
        {
            public int results_returned { get; set; }
            public List<object> search_results { get; set; }
            public int total_results { get; set; }
        }

        public class LogZips
        {
            public int __invalid_name__20854 { get; set; }
            public int __invalid_name__20175 { get; set; }
            public int __invalid_name__20016 { get; set; }
            public int __invalid_name__20105 { get; set; }
            public int __invalid_name__22101 { get; set; }
            public int __invalid_name__22639 { get; set; }
        }

        public class LogLocalities
        {
            public int Aldie { get; set; }
            public int Hume { get; set; }
            public int Potomac { get; set; }
            public int Catoctin { get; set; }
            public int Washington { get; set; }
            public int Leesburg { get; set; }
            public int Mclean { get; set; }
        }

        public class QueryParams
        {
            public string siteIdentifier { get; set; }
            public string phone { get; set; }
            public double minLat { get; set; }
            public string airCond { get; set; }
            public string oname { get; set; }
            public int minParkingSpaces { get; set; }
            public int minHalfBaths { get; set; }
            public int maxHOAFee { get; set; }
            public int numAd { get; set; }
            public string regionId { get; set; }
            public bool disableDefaultLocation { get; set; }
            public int minBaths { get; set; }
            public bool newConstruction { get; set; }
            public string neighborhood { get; set; }
            public string originIdRequirement { get; set; }
            public int maxLotSize { get; set; }
            public double minLng { get; set; }
            public int open_house_filter_end_utc { get; set; }
            public int maxFloors { get; set; }
            public string email { get; set; }
            public string high_school { get; set; }
            public int maxSqFt { get; set; }
            public string aptCom { get; set; }
            public string purchase_types { get; set; }
            public int minYearBuilt { get; set; }
            public List<string> orginIds { get; set; }
            public string mongoId { get; set; }
            public string middle_school { get; set; }
            public string region { get; set; }
            public int minHOAFee { get; set; }
            public int maxPrice { get; set; }
            public string aid { get; set; }
            public int maxParkingSpaces { get; set; }
            public int minSqFt { get; set; }
            public int availOn { get; set; }
            public string textSearch { get; set; }
            public int open_house_filter_start_utc { get; set; }
            public string oid { get; set; }
            public int maxHalfBaths { get; set; }
            public int minLotSize { get; set; }
            public string zip { get; set; }
            public string elementary_school { get; set; }
            public double maxLng { get; set; }
            public int maxYearBuilt { get; set; }
            public bool hasBoundingBox { get; set; }
            public int minFloors { get; set; }
            public int maxBaths { get; set; }
            public string agencyName { get; set; }
            public string servlet { get; set; }
            public int minBeds { get; set; }
            public double maxLat { get; set; }
            public string address { get; set; }
            public string agentName { get; set; }
            public string locality { get; set; }
            public string mlsID { get; set; }
            public int maxBeds { get; set; }
            public bool nonMls { get; set; }
            public int minPrice { get; set; }
        }

        public class Location
        {
            public string region { get; set; }
            public string postal { get; set; }
            public string county { get; set; }
            public string address { get; set; }
            public string neighborhood { get; set; }
            public string locality { get; set; }
            public string country { get; set; }
        }

        public class UncurData
        {
            public string building_name { get; set; }
            public string floor_location { get; set; }
            public string year_built_source { get; set; }
            public string new_construction { get; set; }
            public string lot_size_source { get; set; }
        }

        public class CurData
        {
            public string prop_type { get; set; }
            public string air_cond { get; set; }
            public string lt_sz { get; set; }
            public string desc { get; set; }
            public string dom { get; set; }
            public string hoa_fee { get; set; }
            public string sqft { get; set; }
            public string landing_page { get; set; }
            public string bhi_phone { get; set; }
            public string floors { get; set; }
            public string placester_prop_type { get; set; }
            public string avail_on { get; set; }
            public string year_blt { get; set; }
            public string pk_space { get; set; }
            public string baths { get; set; }
            public string logo { get; set; }
            public string headshot { get; set; }
            public string apts_com { get; set; }
            public string status { get; set; }
            public string bhi_plan_name { get; set; }
            public string site_link { get; set; }
            public string price { get; set; }
            public string beds { get; set; }
            public string half_baths { get; set; }
            public string bhi_subdivision_community_style { get; set; }
            public string non_mls { get; set; }
        }

        public class Schools
        {
            public List<object> middle_school { get; set; }
            public List<object> elementary_school { get; set; }
            public List<object> high_school { get; set; }
        }

        public class AgencyData
        {
            public string phone { get; set; }
            public string email { get; set; }
            public string name { get; set; }
        }

        public class Image
        {
            public int order { get; set; }
            public string url { get; set; }
        }

        public class Rets
        {
            public string detail_page_url { get; set; }
            public string updated_at { get; set; }
            public string oname { get; set; }
            public string ophone { get; set; }
            public string oid { get; set; }
            public string mls_id { get; set; }
            public string created_at { get; set; }
            public string aid { get; set; }
            public string aphone { get; set; }
            public string aname { get; set; }
        }

        public class SearchResult
        {
            public Location location { get; set; }
            public UncurData uncur_data { get; set; }
            public CurData cur_data { get; set; }
            public int featured_weight { get; set; }
            public string log_zips { get; set; }
            public List<string> log_localities { get; set; }
            public string mongo_id { get; set; }
            public string agency_id { get; set; }
            public string place_guids { get; set; }
            public string kwls_id { get; set; }
            public string origin_id { get; set; }
            public Schools schools { get; set; }
            public string purchase_types { get; set; }
            public AgencyData agency_data { get; set; }
            public List<Image> images { get; set; }
            public string longitude { get; set; }
            public string latitude { get; set; }
            public List<string> log_neighborhoods { get; set; }
            public Rets rets { get; set; }
        }

     



        public class DefaultedFormValues
        {
        }

        public class FormQueryValues
        {
            public DefaultedFormValues defaulted_form_values { get; set; }
            public string purchase_types { get; set; }
            public string country { get; set; }
        }

        public class DefaultedFormValues2
        {
        }

        public class FreeTextInferredQueries
        {
            public DefaultedFormValues2 defaulted_form_values { get; set; }
        }

        public class ParsedQuery
        {
            public bool using_default_location { get; set; }
            public bool disable_default_location { get; set; }
            public string full_solr_query { get; set; }
            public string data_source_id_requirement { get; set; }
            public string location_filter { get; set; }
            public string description_query { get; set; }
            public FormQueryValues form_query_values { get; set; }
            public FreeTextInferredQueries free_text_inferred_queries { get; set; }
            public string region_id { get; set; }
            public bool has_open_house_params { get; set; }
            public List<string> data_source_ids { get; set; }
            public bool has_mls_id { get; set; }
        }

        public class OrganicResults
        {
            public int results_returned { get; set; }
            public List<string> SEARCH_RESULT_IDS { get; set; }
            public LogZips log_zips { get; set; }
            public LogLocalities log_localities { get; set; }
            public QueryParams query_params { get; set; }
            public string LOG_VERSION { get; set; }
            public List<SearchResult> search_results { get; set; }
            public string query_lat_lng { get; set; }
            public ParsedQuery parsed_query { get; set; }
            public int total_results { get; set; }
        }

        public class RootObject
        {
            public int request_elapsed_time { get; set; }
            public AdsResults ads_results { get; set; }
            public long request_start_time { get; set; }
            public string region_id { get; set; }
            public string searchId { get; set; }
            public OrganicResults organic_results { get; set; }
            public string cid { get; set; }
        }
    }
