using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIWinforms.DataModel
{
    public class ProductModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public decimal Price { get; set; }

        public string? Image { get; set; }
        public string? Description { get; set; }
        public int CategoryId { get; set; }
    }
}
