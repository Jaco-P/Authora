using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authora.Domain.Entities
{
    public class UserGroup
    {
        public Guid UserId { get; set; }
        public User User { get; set; } = default!;

        public Guid GroupId { get; set; }
        public Group Group { get; set; } = default!;
    }
}
