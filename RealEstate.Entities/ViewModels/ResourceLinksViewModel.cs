using RealEstate.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.ViewModels
{
    public class ResourceLinksViewModel
    {
        public ResourceLinks ResourceLinks { get; set; }
        public IEnumerable<ResourceTitlesView> ResourcesTitles { get; set; }
        public ResourceLinksView ResourceLinksView { get; set; }
        public IEnumerable<ResourceLinksView> ResourceLinksList { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public ResourceLinksPaging ResourceLinksPaging { get; set; }
        public ResourceTypeView ResourceTypeView { get; set; }
    }
}
