using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Authora.Domain.Entities
{

    public class Group
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public ICollection<UserGroup> UserGroups { get; set; } = new List<UserGroup>();
        public ICollection<Permission> Permissions { get; set; } = new List<Permission>();
    }
}
