using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTK.HSSEPassport.Api.Utilities.Constants
{
    public class RoleConstant
    {
        public static string MANAGEMENT = nameof(MANAGEMENT);
        public static string HSSE = nameof(HSSE);
        public static string ADMIN = nameof(ADMIN);
        public static string PEMOHON = nameof(PEMOHON);
        public static string GUEST = nameof(GUEST);
    }
    public static class RoleStatus
    {
        public static string REQUEST = nameof(REQUEST);
        public static string SCHEDULE = nameof(SCHEDULE);
        public static string DEMOROOM = nameof(DEMOROOM);
        public static string APPROVE = nameof(APPROVE);
        public static string DONE = nameof(DONE);
    }
}
