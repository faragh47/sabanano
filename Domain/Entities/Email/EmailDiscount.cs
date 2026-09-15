using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Domain.Entities.Email
{
    public class EmailDiscount : BaseEntity<int>
    {
        [MaxLength(100)]
        public string Email { get; set; }
    }
}
