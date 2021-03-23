using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreWithOutEF.Models
{
    public class BookModelView
    {

        [Key]
        public int BookId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Author { get; set; }
        [Range(1,int.MaxValue,ErrorMessage ="Should be Greater than or equal to 1")]
        public int Price { get; set; }
    }
}
