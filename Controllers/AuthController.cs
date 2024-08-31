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

        public AuthController(ILogger<AuthController> logger, TokenService tokenService, IConfiguration configuration, DatabaseService databaseService, IBBMPBookModuleService IBBMPBOOKMODULE)
        {
            _logger = logger;
            _tokenService = tokenService;
            _configuration = configuration;
            _databaseService = databaseService;
            _IBBMPBOOKMODULE = IBBMPBOOKMODULE;
        }
        NUPMS_BA.CitizenBA Citz = new NUPMS_BA.CitizenBA();
        public class TokenService
        {
            private readonly string _key;
            private readonly string _issuer;
            private readonly string _audience;

            public TokenService(IConfiguration configuration)
            {
                _key = configuration["Jwt:Key"];
                _issuer = configuration["Jwt:Issuer"];
                _audience = configuration["Jwt:Audience"];
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
                    Expires = DateTime.UtcNow.AddHours(1),
                    Issuer = _issuer,
                    Audience = _audience,
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
        }



        [HttpPost("CitizenLogin")]
        public IActionResult CitizenLogin(string userId, string password, bool isOtpGenerated)
        {
            try
            {
                if (isOtpGenerated)
                {
                    var dsUserDetails = Citz.getUserdata(userId);
                    if (dsUserDetails != null && dsUserDetails.Tables.Count > 0 && dsUserDetails.Tables[0].Rows.Count > 0)
                    {
                        var token = _tokenService.GenerateToken(userId); 
                        return Ok(new { Token = token });
                    }
                    else
                    {
                        return Ok(false);
                    }
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
                _logger.LogError(ex, "Error occurred while executing the login process.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error.");
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
        [HttpGet("GetServerIpAddress")]
        public IActionResult GetServerIp()
        {
            var host = Dns.GetHostName();
            var ip = Dns.GetHostEntry(host)
                        .AddressList
                        .FirstOrDefault(ip => ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        ?.ToString();

            if (ip != null)
            {
                // Example: Expose the first three octets, mask the last octet
                var ipParts = ip.Split('.');
                var maskedIp = $"{ipParts[0]}.XXX.XXX.XXX";
                return Ok(new { ip = maskedIp });
            }

             return Ok(new { ip = "Not Found" });
        }

    }
   

}
