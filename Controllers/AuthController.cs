using BBMPCITZAPI.Database;
using BBMPCITZAPI.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Net;
using NUPMS_BA;
using NUPMS_DA;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using static BBMPCITZAPI.Controllers.AuthController;
using BBMPCITZAPI.Models;
using Microsoft.Extensions.Options;
using Org.BouncyCastle.Utilities.Encoders;
using System.Text.Json.Nodes;
using System.Runtime.InteropServices.JavaScript;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace BBMPCITZAPI.Controllers
{
    [Route("v1/Auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IConfiguration _configuration;
        private readonly DatabaseService _databaseService;
        private readonly TokenService _tokenService;
        private readonly IBBMPBookModuleService _IBBMPBOOKMODULE;
        private readonly KaveriSettings _kaveriSettings;
        private readonly IErrorLogService _errorLogService;

        public AuthController(ILogger<AuthController> logger, TokenService tokenService, IConfiguration configuration,
             IOptions<KaveriSettings> kaveriSettings, DatabaseService databaseService, IBBMPBookModuleService IBBMPBOOKMODULE, IErrorLogService errorLogService)
        {
            _logger = logger;
            _tokenService = tokenService;
            _configuration = configuration;
            _databaseService = databaseService;
            _IBBMPBOOKMODULE = IBBMPBOOKMODULE;
            _errorLogService = errorLogService;
            _kaveriSettings = kaveriSettings.Value;
        }
        NUPMS_BA.CitizenBA Citz = new NUPMS_BA.CitizenBA();

        public class TokenService
        {
            private readonly string _key;
            private readonly string _issuer;
            private readonly string _audience;
            private readonly string _ServerIP;
          

            public TokenService(IConfiguration configuration)
            {
                _key = configuration["Jwt:Key"];
                _issuer = configuration["Jwt:Issuer"];
                _audience = configuration["Jwt:Audience"];
                _ServerIP = configuration["Jwt:ServerIP"];
               

            }

            public string GenerateToken(string username)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_key);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                new Claim(ClaimTypes.Name, username)
                    }),
                    Expires = DateTime.UtcNow.AddMinutes(5),
                    Issuer = _issuer,
                    Audience = _audience,
                    
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
        }



        [HttpPost("CitizenLogin")]
        public async Task<IActionResult> CitizenLogin(string userId, string password, bool isOtpGenerated)
        {
            try
            {
                if (isOtpGenerated)
                {
                    //var dsUserDetails = Citz.getUserdata(userId);
                    //if (dsUserDetails != null && dsUserDetails.Tables.Count > 0 && dsUserDetails.Tables[0].Rows.Count > 0)
                    //{

                        var token = _tokenService.GenerateToken(password);
                        return Ok(new { Token = token });
                   // }
                  //  else
                  //  {
                  //      return Ok(false);
                  //  }
                }
                else
                {
                    var salt = CreateSalt(5);
                    var isAuthenticated = IsAuthenticatedUser(userId, password, salt);
                    if (isAuthenticated)
                    {
                        var token = _tokenService.GenerateToken(userId);
                        return Ok(new { Token = token });
                    }
                    else
                    {
                        return Ok(false);
                    }
                }
            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "CitizenLogin");
                _logger.LogError(ex, "Error occurred while executing the login process.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error.");
            }
        }
        [HttpPost("CitizenAuthorization")]
        public IActionResult CitizenAuthorization(string tokens)
        {
            try
            {
                if (_kaveriSettings.ApiToken == tokens)
                {
                    var token = _tokenService.GenerateToken(tokens);
                    return Ok(new { Token = token });
                }
                else
                {
                    return StatusCode(StatusCodes.Status401Unauthorized, "UnAuthorized");
                }
            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "CitizenAuthorization");
                _logger.LogError(ex, "Error occurred while executing the login process.");
                return StatusCode(StatusCodes.Status401Unauthorized, "UnAuthorized");
            }
        }


        [HttpGet("GetCitzMobileNumber")]
        public ActionResult<string> GetMobileNumber(string UserId)
        {
            try
            {

                DataSet dsUserDetails = Citz.getUserdata(UserId);
                if (dsUserDetails != null && dsUserDetails.Tables.Count > 0 && dsUserDetails.Tables[0].Rows.Count > 0)
                {

                    DataTable userTable = dsUserDetails.Tables[0];
                    DataRow userRow = userTable.Rows[0];

                    if (userTable.Columns.Contains("MOBILENO") && userRow["MOBILENO"] != DBNull.Value)
                    {
                        string mobileNumber = userRow["MOBILENO"].ToString();
                        return mobileNumber;
                    }
                    else
                    {
                        return "Mobile number is not available.";
                    }
                }
                else
                {
                    return "INVALID USER ID";
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
                _errorLogService.LogError(oEx, "CitizenLogin");
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
                _errorLogService.LogError(ex, "CitizenLogin");
                _logger.LogError(ex, "Error occurred while creating salt");
                throw;
            }
        }
        [HttpGet("GetServerIpAddress")]
        public IActionResult GetServerIp()
        {
            string ipParts = _kaveriSettings.ServerIP;
            var maskedIp = $"{ipParts}.XXX.XXX.XXX";
            return Ok(new { ip = maskedIp });
        }
        public class encryptedJson
        {


            public string? UserId { get; set; }
            public string?  PropertyCode  {get;set;}
            public string?  PropertyEPID {get;set;}
            public string? SessionValues {get;set;}
            public string? ExecTime {get;set;}
            public bool IsLogin { get; set; }
        }

        //[HttpPost("EncryptJsons")]
        //public string EncryptJson(string jsonObject)
        //{
        //    try
        //    {
        //        jsonObject.ExecTime = DateTime.UtcNow;
        //        string keyToEncrypt = "7c1aae83fef846aab09758d4a7d455de208667f2968344db8317a0f0871f10d6";

               
        //        JsonSerializerSettings settings = new JsonSerializerSettings
        //        {
        //            DateFormatString = "yyyyMMddTHH:mm:ss"
        //        };

                
        //        string jsonString = JsonConvert.SerializeObject(jsonObject, settings);

               
        //        string encodedJsonString = jsonString;
               
        //        RijndaelManaged aes = new RijndaelManaged
        //        {
        //            BlockSize = 128,
        //            KeySize = 256,
        //            Mode = CipherMode.CBC,
        //            Padding = PaddingMode.PKCS7
        //        };

        //        byte[] keyArr = Convert.FromBase64String(keyToEncrypt);
        //        byte[] KeyArrBytes32Value = new byte[32];
        //        Array.Copy(keyArr, KeyArrBytes32Value, 32);

        //        byte[] ivArr = { 1, 2, 3, 4, 5, 6, 6, 5, 4, 3, 2, 1, 7, 7, 7, 7 };
        //        byte[] IVBytes16Value = new byte[16];
        //        Array.Copy(ivArr, IVBytes16Value, 16);

        //        aes.Key = KeyArrBytes32Value;
        //        aes.IV = IVBytes16Value;

        //        ICryptoTransform encrypto = aes.CreateEncryptor();
        //        byte[] plainTextByte = ASCIIEncoding.UTF8.GetBytes(encodedJsonString);
        //        byte[] CipherText = encrypto.TransformFinalBlock(plainTextByte, 0, plainTextByte.Length);

        //        return System.Web.HttpUtility.UrlEncode(Convert.ToBase64String(CipherText).Replace("+", "---"));
        //    }
        //    catch(Exception ex)
        //    {
        //        _errorLogService.LogError(ex, "CitizenLogin");
        //        _logger.LogError(ex, "Error occurred while creating salt");
        //        throw;
        //    }
        //}

        //[HttpGet("DecryptJson")]
        //public encryptedJson DecryptJson(string encryptedJsonString)
        //{
        //    try
        //    {
        //        string keyToDecrypt = "7c1aae83fef846aab09758d4a7d455de208667f2968344db8317a0f0871f10d6";
        //        string decryptedJSONDecode = System.Web.HttpUtility.UrlDecode(encryptedJsonString);
        //        decryptedJSONDecode = decryptedJSONDecode.Replace("---", "+");
        //        RijndaelManaged aes = new RijndaelManaged
        //        {
        //            BlockSize = 128,
        //            KeySize = 256,
        //            Mode = CipherMode.CBC,
        //            Padding = PaddingMode.PKCS7
        //        };

        //        byte[] keyArr = Convert.FromBase64String(keyToDecrypt);
        //        byte[] KeyArrBytes32Value = new byte[32];
        //        Array.Copy(keyArr, KeyArrBytes32Value, 32);

        //        byte[] ivArr = { 1, 2, 3, 4, 5, 6, 6, 5, 4, 3, 2, 1, 7, 7, 7, 7 };
        //        byte[] IVBytes16Value = new byte[16];
        //        Array.Copy(ivArr, IVBytes16Value, 16);

        //        aes.Key = KeyArrBytes32Value;
        //        aes.IV = IVBytes16Value;

        //        ICryptoTransform decrypto = aes.CreateDecryptor();

        //        byte[] encryptedBytes = Convert.FromBase64CharArray(decryptedJSONDecode.ToCharArray(), 0, decryptedJSONDecode.Length);
        //        byte[] decryptedData = decrypto.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);

                
        //        string decryptedString = Encoding.UTF8.GetString(decryptedData);

                
        //        string decodedJsonString = System.Web.HttpUtility.HtmlDecode(decryptedString);

                
        //        JsonSerializerSettings settings = new JsonSerializerSettings();
        //        settings.Converters.Add(new CustomDateTimeConverter());

        //        encryptedJson decryptedJsonObject = JsonConvert.DeserializeObject<encryptedJson>(decodedJsonString, settings);

        //        return decryptedJsonObject;
        //    }
        //    catch(Exception ex)
        //    {
        //        _errorLogService.LogError(ex, "CitizenLogin");
        //        _logger.LogError(ex, "Error occurred while creating salt");
        //        throw;
        //    }
        //    }
        [HttpPost("EncryptJsons")]
        public string RijndaelManagedEncrypt(encryptedJson json)
        {
            try
            {
                string plainXML = "";
                if (json.IsLogin)
                {
                    plainXML = "{\"UserId\":\"" + Convert.ToString(json.UserId) + "\",\"PropertyCode\":\"" + Convert.ToString(json.PropertyCode) + "\",\"PropertyEPID\":\"" + Convert.ToString(json.PropertyEPID) + "\",\"SessionValues\":[],\"ExecTime\":\"" + json.ExecTime + "\"}";
                }
                else
                {
                    plainXML = "{\"UserId\":\"" + "" + "\",\"PropertyCode\":\"" + Convert.ToString(json.PropertyCode) + "\",\"PropertyEPID\":\"" + Convert.ToString(json.PropertyEPID) + "\",\"SessionValues\":[],\"ExecTime\":\"" + json.ExecTime + "\"}";
                }
                string keyToEncrypt = "7c1aae83fef846aab09758d4a7d455de208667f2968344db8317a0f0871f10d6";
                RijndaelManaged aes = new RijndaelManaged();
                aes.BlockSize = 128;
                aes.KeySize = 256;

                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                byte[] keyArr = Convert.FromBase64String(keyToEncrypt);
                byte[] KeyArrBytes32Value = new byte[32];
                Array.Copy(keyArr, KeyArrBytes32Value, 32);

                byte[] ivArr = { 1, 2, 3, 4, 5, 6, 6, 5, 4, 3, 2, 1, 7, 7, 7, 7 };
                byte[] IVBytes16Value = new byte[16];
                Array.Copy(ivArr, IVBytes16Value, 16);

                aes.Key = KeyArrBytes32Value;
                aes.IV = IVBytes16Value;

                ICryptoTransform encrypto = aes.CreateEncryptor();

                byte[] plainTextByte = ASCIIEncoding.UTF8.GetBytes(plainXML);
                byte[] CipherText = encrypto.TransformFinalBlock(plainTextByte, 0, plainTextByte.Length);

                string encryptedXMLencryptedXML = Convert.ToBase64String(CipherText).Replace("+", "---");

                return encryptedXMLencryptedXML;
            }catch(Exception ex)
            {
                _errorLogService.LogError(ex, "EncryptJsons react");
                        _logger.LogError(ex, "Error occurred while EncryptJsons react");
                        throw;
            }
        }
        [HttpGet("DecryptJson")]
        public string RijndaelManagedDecrypt(string encryptedXML)
        {
            try
            {


                string keyToDecrypt = "7c1aae83fef846aab09758d4a7d455de208667f2968344db8317a0f0871f10d6";
                encryptedXML = encryptedXML.Replace("---", "+");

                RijndaelManaged aes = new RijndaelManaged();
                aes.BlockSize = 128;
                aes.KeySize = 256;

                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                byte[] keyArr = Convert.FromBase64String(keyToDecrypt);
                byte[] KeyArrBytes32Value = new byte[32];
                Array.Copy(keyArr, KeyArrBytes32Value, 32);

                byte[] ivArr = { 1, 2, 3, 4, 5, 6, 6, 5, 4, 3, 2, 1, 7, 7, 7, 7 };
                byte[] IVBytes16Value = new byte[16];
                Array.Copy(ivArr, IVBytes16Value, 16);

                aes.Key = KeyArrBytes32Value;
                aes.IV = IVBytes16Value;

                ICryptoTransform decrypto = aes.CreateDecryptor();

                byte[] encryptedBytes = Convert.FromBase64CharArray(encryptedXML.ToCharArray(), 0, encryptedXML.Length);
                byte[] decryptedData = decrypto.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
                return ASCIIEncoding.UTF8.GetString(decryptedData);
            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "DecryptJson react");
                      _logger.LogError(ex, "Error occurred while DecryptJson react");
                        throw;
            }
        }
      
      







    }
}
