using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTK.HSSEPassport.Utilities.Base
{
    public class BaseReadFileModel
    {
        public byte[] FileContents { get; set; }

        public string ContentType { get; set; }

        public string FileName { get; set; }
    }
}
