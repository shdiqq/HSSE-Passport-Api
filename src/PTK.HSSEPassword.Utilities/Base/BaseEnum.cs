using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTK.HSSEPassport.Api.Utilities.Base
{
    public class BaseEnum
    {
        public enum DatabaseEnums
        {
            HSSEPasspostDb,
        }

        public enum TrxFlagEnums
        {
            ADM,
            KKR,
            OSR,
            PO,
            MSP,
            STS
        }

		public enum StatusEnums
        {
            DRAFT,
            SUBMITED,
            APPROVED,
        }
    }
}
