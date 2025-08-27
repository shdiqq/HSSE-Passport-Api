using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTK.HSSEPassport.Utilities.Base
{
    public class BaseDao
    {
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;


        public DateTime? UpdateDt { get; set; }

        public string CreatedBy { get; set; }

        public string? UpdateBy { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
