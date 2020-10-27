using RealEstate.Entities.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.DataAccess
{
    public class EFDBContext : DbContext
    {
        public DbSet<utblMstAgentDealSharing> utblMstAgentDealSharings { get; set; }
        public DbSet<AspNetUser> AspNetUsers { get; set; }
        public DbSet<utblMstClientActivityLog> utblMstClientActivityLogs { get; set; }

        public DbSet<utblMstSellerDocument> utblMstSellerDocuments { get; set; }
        public DbSet<utblMstAgentListing> utblMstAgentListings { get; set; }
        public DbSet<utblMstEventCalMaster> utblMstEventCalMasters { get; set; }
        public DbSet<utblUserDetail> utblUserDetails { get; set; }
        public DbSet<utblMstClientHomeGallerie> utblMstClientHomeGalleries { get; set; }
        public DbSet<utblMstSellerCounterOffer> utblMstSellerCounterOffers { get; set; }
        public DbSet<utblMstPotentialBuyer> utblMstPotentialBuyers { get; set; }
        public DbSet<utblMstSellerClosingConfigutation> utblMstSellerClosingConfigutations { get; set; }
        public DbSet<utblMstSellerTrackDealDoc> utblMstSellerTrackDealDocs { get; set; }
        public DbSet<utblMstSellerTrackDeal> utblMstSellerTrackDeals { get; set; }
        public DbSet<utblMstSellerStatus> utblMstSellerStatus { get; set; }
        public DbSet<utblMstHelpRequest> utblMstHelpRequests { get; set; }
        public DbSet<utblMstTrackDealMaster> utblMstTrackDealMasters { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<GoogleRefreshToken> GoogleRefreshTokens { get; set; }
        public DbSet<utblMstGmailToken> utblMstGmailTokens { get; set; }
        public DbSet<utblMstNeighborhoodState> utblMstNeighborhoodStates { get; set; }
        public DbSet<utblMstNeighborhoodCity> utblMstNeighborhoodCities { get; set; }
        public DbSet<utblMstCareerDepartment> utblMstCareerDepartments { get; set; }
        public DbSet<utblMstJobPosition> utblMstJobPositions { get; set; }
        public DbSet<utblMstKeyInfoLink> utblMstKeyInfoLinks { get; set; }
        public DbSet<utblMstClientGallerie> utblMstClientGalleries { get; set; }
        public DbSet<utblBlog> utblBlogs { get; set; }
        public DbSet<utblMstClosingDate> utblMstClosingDates { get; set; }
        public DbSet<utblMstClient> utblMstClients { get; set; }

        public DbSet<utblNeighborhood> utblNeighborhoods { get; set; }
        public DbSet<utblMstStatus> utblMstStatus { get; set; }
        public DbSet<utblMstTrackDeal> utblMstTrackDeals { get; set; }
        public DbSet<utblMstTrackDealDoc> utblMstTrackDealDocs { get; set; }
        public DbSet<utblGenCodeSeed> utblGenCodeSeeds { get; set; }
        public DbSet<utblMstContact> utblMstContacts { get; set; }

        public DbSet<utblMstEnquire> utblMstEnquires { get; set; }
        public DbSet<utblMstArticleEnquire> utblMstArticleEnquires { get; set; }

        public DbSet<utblMstClientTrackDealDoc> utblMstClientTrackDealDocs { get; set; }

        public DbSet<utblMstAppointment> utblMstAppointments { get; set; }

        public DbSet<utblMstCalenderConfiguration> utblMstCalenderConfigurations { get; set; }

        public DbSet<utblMstClosingConfigutation> utblMstClosingConfigutations { get; set; }

        public DbSet<utblMstBuyerDocument> utblMstBuyerDocuments { get; set; }
        public DbSet<utblMstClientHomeListing> utblMstClientHomeListings { get; set; }
        public DbSet<VendorCategory> VendorCategory { get; set; }
        public DbSet<Vendor> Vendor { get; set; }
        public DbSet<DealVendor> DealVendor { get; set; }
        public DbSet<TeamMembers> TeamMembers { get; set; }

        public DbSet<ClientDealDocuments> ClientDealDocuments { get; set; }

        public DbSet<ClientDocument> ClientDocument { get; set; }
        public DbSet<ClientDealDocmuent> ClientDealDocmuent { get; set; }
        public DbSet<ClientToDoList> ClientToDoList { get; set; }
        public DbSet<UserGallery> UserGallery { get; set; }
        public DbSet<VendorsReview> VendorsReview { get; set; }
        public DbSet<VendorContacts> VendorContacts { get; set; }
        public DbSet<Quiz> BuyerQuiz { get; set; }
        public DbSet<ResourceLinks> ResourceLinks { get; set; }
        public DbSet<ResourceType> ResourceType { get; set; }
        public DbSet<Comments> Comments { get; set; }
        public DbSet<bepetworth> bepetworth { get; set; }
       
    }
}
