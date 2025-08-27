using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using PTK.HSSEPassport.Utilities.Configurations;

namespace PTK.HSSEPassport.Api.Utilities.Constants
{
    public class GeneralConstant
    {
        public static string Dev = nameof(Dev);
        public static string Prod = nameof(Prod);
        public static string Local = nameof(Local);

        public static string PreTest = nameof(PreTest);
        public static string PostTest = nameof(PostTest);


        /* Activity Log */
        public const string FAILED = nameof(FAILED);
        public const string SUCCESS = nameof(SUCCESS);
        public const string NO_TRX_ID = nameof(NO_TRX_ID);

        public const string ROLE_GUEST = "Guest";

        //default password
        public const string LoginIdaman = nameof(LoginIdaman);
        public static string KODE_DEFAULT = "PraiseIdamanPertamina#29";

        /* Upload */
        //public const string URL_UPLOAD = @"\\ptmkpshare2.pertamina.com\ptmpisappprd01\HSSEPassportPTKDev";
        public const string URL_UPLOAD_PATH = @"\upload\";
        public const string DELETED_PATH = @"\upload\deleted\";
        public static string URL_UPLOAD { get; private set; }

        public static void Initialize(IConfiguration config)
        {
            var appSetting = config.GetSection("Database").Get<AppSetting>();

            var mode = appSetting.Mode;
            if (!appSetting.UrlUpload.TryGetValue(mode, out var uploadPath))
            {
                throw new Exception($"Upload path not defined for mode: {mode}");
            }
            URL_UPLOAD = uploadPath;
        }

        public static string CreateUploadPathNew(string path)
        {
            return Path.Combine(URL_UPLOAD, Path.Combine(@"upload\", Path.Combine(path, DateTime.UtcNow.ToString("yyyyMMdd"))));
        }

        public static string CreateUploadPathView(string path) => Path.Combine(Path.Combine(URL_UPLOAD_PATH, path), DateTime.UtcNow.ToString("yyyyMMdd")) + @"\";

        public static string ReplaceDeletedPath(string path)
        {
            return path.Replace(URL_UPLOAD_PATH, DELETED_PATH);
        }

        public static string CreateDeletedPath(string path)
        {
            return Path.Combine(URL_UPLOAD, path);
        }

        public static string GetLocalIPAddress()
        {
            string firstMacAddress = NetworkInterface
               .GetAllNetworkInterfaces()
               .Where(nic => nic.OperationalStatus == OperationalStatus.Up && nic.NetworkInterfaceType != NetworkInterfaceType.Loopback)
               .Select(nic => nic.GetPhysicalAddress().ToString())
               .FirstOrDefault();
            var host = Dns.GetHostEntry(Dns.GetHostName());

            string ipAddressAll = string.Empty;
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    ipAddressAll = ipAddressAll + ip.ToString() + ",";
                }

            }

            if (ipAddressAll == string.Empty)
            {
                throw new Exception("No network adapters with an IPv4 address in the system!");
            }

            return $"MAC_ADDRESS = {firstMacAddress} :: IP_ADDRESS = {ipAddressAll}";

        }
    }
}
