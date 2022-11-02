using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Workshop05.Models
{
    public class Advertisement
    {
        [Key]
        [StringLength(100)]
        public string Uid { get; set; }

        [StringLength(100)]
        public string CarModel { get; set; }

        public int Price { get; set; }

        public string UserId { get; set; }

        [NotMapped]
        public virtual IdentityUser User { get; set; }

        [StringLength(100)]
        public string PhotoUrl { get; set; }

        public Advertisement()
        {
            Uid = Guid.NewGuid().ToString();
        }
    }
}
