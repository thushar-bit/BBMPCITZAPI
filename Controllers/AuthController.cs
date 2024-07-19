using BBMPCITZAPI.Database;
using BBMPCITZAPI.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NUPMS_BA;
using NUPMS_DA;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Security.Cryptography;
using System.Text;

namespace BBMPCITZAPI.Controllers
{
    [Route("v1/Auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IConfiguration _configuration;
        private readonly DatabaseService _databaseService;
        private readonly IBBMPBookModuleService _IBBMPBOOKMODULE;

        public AuthController(ILogger<AuthController> logger, IConfiguration configuration, DatabaseService databaseService, IBBMPBookModuleService IBBMPBOOKMODULE)
        {
            _logger = logger;
            _configuration = configuration;
            _databaseService = databaseService;
            _IBBMPBOOKMODULE = IBBMPBOOKMODULE;
        }
        NUPMS_BA.CitizenBA Citz = new NUPMS_BA.CitizenBA();
        
        [HttpPost("CitizenLogin")]
        public ActionResult<bool> CitizenLogin(string UserId, string Password, bool IsOTPGenerated)
        {
            try
            {
                if (IsOTPGenerated)
                {
                    DataSet dsUserDetails = Citz.getUserdata(UserId);
                    if (dsUserDetails != null && dsUserDetails.Tables.Count > 0 && dsUserDetails.Tables[0].Rows.Count > 0)
                    {
                        return true;
                    }
                    else
                    {
                       return false;
                    }
                }
                else
                {
                    var salt = CreateSalt(5);
                    var b = IsAuthenticatedUser(UserId, Password, salt);
                    return b;
                }
                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing stored procedure.");
                throw;
            }
        }
       
        private bool IsAuthenticatedUser(string userId, string password, string salt)
        {
            bool result = false;
         
            DataSet userCitizen;
            try
            {
                userCitizen = _IBBMPBOOKMODULE.GetUserCitizen(userId);
                if (userCitizen.Tables.Count > 0)
                {
                    if (userCitizen.Tables[0].Rows.Count > 0)
                    {
                        string text = userCitizen.Tables[0].Rows[0]["PASSWORD"].ToString().ToLower();
                        AppUtils appUtils = new AppUtils();
                        string text1 = MD5Hash(password + salt).ToLower();
                        string text2 = MD5Hash(text + salt).ToLower();
                        if (text2.Equals(text1))
                        {
                            result = true;
                        }
                    }
                    else
                    {
                        result = false;
                    }
                }
                else
                {
                    result = false;
                }
            }
            catch (OracleException oEx)
            {
              throw new Exception(oEx.Message);
            }
            catch (Exception oEx2)
            {
               throw oEx2;
            }

            userCitizen = null;
            return result;
        }

        private static string MD5Hash(string input)
        {
            StringBuilder hash = new StringBuilder();
            try
            {
                MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
                byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input));

                for (int i = 0; i < bytes.Length; i++)
                {
                    hash.Append(bytes[i].ToString("x2"));
                }
                return hash.ToString();
            }
            catch (Exception ex)
            {
                Alert.Show(ex.Message);
                return null;
            }
        }

        private string CreateSalt(int size)
        {
            try
            {
                RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
                byte[] buff = new byte[size];
                rng.GetBytes(buff);
                return Convert.ToBase64String(buff);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating salt");
                throw;
            }
        }

    }
   

}
