using Microsoft.AspNetCore.Mvc;
using PTK.HSSEPassport.Api.Domain.Interfaces;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;

namespace PTK.HSSEPassport.Api.Controllers
{
    public class BaseController : ControllerBase
    {
        private protected IAppLogService _appLog;
        private static readonly string EncryptionKey = "HSSEPASSPORTPASS_ENCRYPT!@#123"; // Replace with your own encryption key
        private static readonly string Salt = "HSSEPASSPORTSALT!@#123"; // Replace with your own salt

        public BaseController(IAppLogService appLog)
        {
            _appLog = appLog;
        }

        protected string GetCurrentMethod([CallerMemberName] string methodName = "")
            => methodName.ToUpper();

        protected async Task SaveAppLog(string methodName, string trxId, string status, CancellationToken cancellationToken, string remark = null, string errorMessage = null, string info = null, string username = null)
            => await _appLog.SaveAppLog(
                controllerName: ControllerContext.RouteData.Values["controller"].ToString().ToUpper(),
                methodName: methodName,
                userName: username ?? "",
                trxId: trxId,
                status: status,
                cancellationToken: cancellationToken,
                remark: remark,
                errorMessage: errorMessage,
                info: info);

        public static string EncryptPassword(string password)
        {
            using Aes aesAlg = Aes.Create();
            Rfc2898DeriveBytes keyDerivationFunction = new Rfc2898DeriveBytes(EncryptionKey, Encoding.UTF8.GetBytes(Salt));

            aesAlg.Key = keyDerivationFunction.GetBytes(aesAlg.KeySize / 8);
            aesAlg.IV = aesAlg.BlockSize == 128 ? aesAlg.Key.Take(16).ToArray() : aesAlg.Key.Take(8).ToArray();

            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            using MemoryStream msEncrypt = new MemoryStream();
            using (CryptoStream csEncrypt = new(msEncrypt, encryptor, CryptoStreamMode.Write))
            {
                using StreamWriter swEncrypt = new StreamWriter(csEncrypt);
                swEncrypt.Write(password);
            }
            return Convert.ToBase64String(msEncrypt.ToArray());
        }

        public static string DecryptPassword(string encryptedPassword)
        {
            using (Aes aesAlg = Aes.Create())
            {
                Rfc2898DeriveBytes keyDerivationFunction = new Rfc2898DeriveBytes(EncryptionKey, Encoding.UTF8.GetBytes(Salt));

                aesAlg.Key = keyDerivationFunction.GetBytes(aesAlg.KeySize / 8);
                aesAlg.IV = aesAlg.BlockSize == 128 ? aesAlg.Key.Take(16).ToArray() : aesAlg.Key.Take(8).ToArray();

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(encryptedPassword));
                using CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
                using StreamReader srDecrypt = new StreamReader(csDecrypt);
                return srDecrypt.ReadToEnd();
            }
        }
    }
}
