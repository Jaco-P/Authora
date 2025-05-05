using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authora.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }

        [Required]        
        public string Username { get; set; } = string.Empty;  // never null

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;      // never null

        public ICollection<UserGroup> UserGroups { get; set; } = new List<UserGroup>();
    }
}
