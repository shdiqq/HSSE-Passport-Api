using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTK.HSSEPassport.Api.Utilities.Base
{
    public  class BaseDTModel
    {
        public int Id { get; set; }
        public string? IsActive { get; set; }
        public DateTime? CreatedDT { get; set; }
    }

	public class FileDT
	{
		public int FileId { get; set; }
		public string? FileName { get; set; }
	}
}
