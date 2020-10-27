using RealEstate.Entities.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Entities.ViewModels
{
    public class ClientDealDocumentsViewModel
    {
        public ClientDealDocuments ClientDealDocuments { get; set; }
        public IEnumerable<ClientDealDocuments> ClientDealDocumentList { get; set; }
    }
}
