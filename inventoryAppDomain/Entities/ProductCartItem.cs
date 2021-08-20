using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inventoryAppDomain.Entities
{
    public class ProductCartItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int Amount { get; set; }
        public int PrescribedAmount { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int ProductCartId { get; set; }

        public ProductCart ProductCart { get; set; }
    }
}
