using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLLoader
{
    internal class PurchaseDetails
    {
        public int DetailsId {  get; set; }
        public int PurchaseId {  get; set; }
        public int ProductId { get; set; }
        public int Quantity {  get; set; }
    }
}
