using BBMPCITZAPI.Database;
using BBMPCITZAPI.Services.Interfaces;
using Microsoft.AspNetCore.Http.Extensions;
using NUPMS_DA;
using Oracle.ManagedDataAccess.Client;
using System.Configuration;
using System.Data;

namespace BBMPCITZAPI.Services
{
    public class ErrorLoggingService : IErrorLogService
    {

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<ErrorLoggingService> _logger;

        private readonly string _connectionString;
        public ErrorLoggingService(IHttpContextAccessor httpContextAccessor, ILogger<ErrorLoggingService> logger, IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _connectionString = configuration.GetConnectionString("BBMPCITZAPIConnection")!;

        }
     //  string strConn = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=172.31.20.171)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=orcl))); User Id=scott_copy; Password=EAASTHI";

        //  string strConn = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=BBMPRAC-SCAN)(PORT=1522))(CONNECT_DATA=(SERVICE_NAME=bbmpee))); User Id=EAASTHI_TEMP; Password=bbmp1234";
        public void LogError(Exception oEx, string Id)
        {
            bool blLogCheck = true;
            if (blLogCheck)
            {
                LogException(oEx, Id);
            }
        }
        public void LogException(Exception ex, string ID)
        {
            var ctxObject = _httpContextAccessor.HttpContext;

            // Get the current date and time
            string logDateTime = DateTime.Now.ToString("g");

            // Get the request URL
            string strReqURL = ctxObject?.Request?.GetDisplayUrl() ?? String.Empty;

            // Get the query string
            string strReqQS = ctxObject?.Request?.QueryString.Value ?? String.Empty;

            // Get the HTTP referer (previous page)
            string strServerName = ctxObject?.Request?.Headers["Referer"].ToString() ?? String.Empty;

            // Get the user agent
            string strUserAgent = ctxObject?.Request?.Headers["User-Agent"].ToString() ?? String.Empty;

            // Get the user's IP address
            string strUserIP = ctxObject?.Connection?.RemoteIpAddress?.ToString() ?? String.Empty;

            // Get user authentication status
            string strUserAuthen = ctxObject?.User?.Identity?.IsAuthenticated.ToString() ?? "False";

            // Get the username
            string strUserName = ctxObject?.User?.Identity?.Name ?? String.Empty;

            // Initialize exception details
            string strMessage = string.Empty, strSource = string.Empty, strTargetSite = string.Empty, strStackTrace = string.Empty;

            // Traverse through the exceptions and log details
            while (ex != null)
            {
                strMessage = ex.Message + ID;
                strSource = ex.Source;
                strTargetSite = ex.TargetSite?.ToString();
                strStackTrace = ex.StackTrace;

                // Log exception details using ILogger (ASP.NET Core logging)
                _logger.LogError("Exception Logged: {Message}, Source: {Source}, TargetSite: {TargetSite}, StackTrace: {StackTrace}, Request URL: {RequestURL}, User: {User}, User IP: {UserIP}, User Authenticated: {UserAuthen}, QueryString: {QueryString}, Referer: {Referer}, UserAgent: {UserAgent}",
                    strMessage, strSource, strTargetSite, strStackTrace, strReqURL, strUserName, strUserIP, strUserAuthen, strReqQS, strServerName, strUserAgent);

                // Move to inner exception if available
                ex = ex.InnerException;
            }
            string strSQL;
            strSQL = "NPMUM.LOGEXCEPTION";
            OracleParameter[] pars = new OracleParameter[11];
            try
            {
                pars[0] = new OracleParameter();
                pars[0].OracleDbType = OracleDbType.Varchar2;
                pars[0].Direction = ParameterDirection.Input;
                pars[0].ParameterName = "P_SOURCE";
                pars[0].Value = strSource;

                pars[1] = new OracleParameter();
                pars[1].OracleDbType = OracleDbType.Varchar2;
                pars[1].Direction = ParameterDirection.Input;
                pars[1].ParameterName = "P_MESSAGE";
                pars[1].Value = strMessage;

                pars[2] = new OracleParameter();
                pars[2].OracleDbType = OracleDbType.NVarchar2;
                pars[2].Direction = ParameterDirection.Input;
                pars[2].ParameterName = "P_QUERYSTRING";
                pars[2].Value = strReqQS;

                pars[3] = new OracleParameter();
                pars[3].OracleDbType = OracleDbType.Varchar2;
                pars[3].Direction = ParameterDirection.Input;
                pars[3].ParameterName = "P_TARGETSITE";
                pars[3].Value = strTargetSite;

                pars[4] = new OracleParameter();
                pars[4].OracleDbType = OracleDbType.Varchar2;
                pars[4].Direction = ParameterDirection.Input;
                pars[4].ParameterName = "P_STACKTRACE";
                pars[4].Value = strStackTrace;

                pars[5] = new OracleParameter();
                pars[5].OracleDbType = OracleDbType.Varchar2;
                pars[5].Direction = ParameterDirection.Input;
                pars[5].ParameterName = "P_SERVERNAME";
                pars[5].Value = strServerName;

                pars[6] = new OracleParameter();
                pars[6].OracleDbType = OracleDbType.Varchar2;
                pars[6].Direction = ParameterDirection.Input;
                pars[6].ParameterName = "P_REQUESTURL";
                pars[6].Value = strReqURL;

                pars[7] = new OracleParameter();
                pars[7].OracleDbType = OracleDbType.Varchar2;
                pars[7].Direction = ParameterDirection.Input;
                pars[7].ParameterName = "P_USERAGENT";
                pars[7].Value = strUserAgent;

                pars[8] = new OracleParameter();
                pars[8].OracleDbType = OracleDbType.Varchar2;
                pars[8].Direction = ParameterDirection.Input;
                pars[8].ParameterName = "P_USERIP";
                pars[8].Value = strUserIP;

                pars[9] = new OracleParameter();
                pars[9].OracleDbType = OracleDbType.Varchar2;
                pars[9].Direction = ParameterDirection.Input;
                pars[9].ParameterName = "P_USERAUTHENTICATION";
                pars[9].Value = strUserAuthen;

                pars[10] = new OracleParameter();
                pars[10].OracleDbType = OracleDbType.Varchar2;
                pars[10].Direction = ParameterDirection.Input;
                pars[10].ParameterName = "P_USERNAME";
                pars[10].Value = strUserName;
                OracleHelper.ExecuteNonQuery(_connectionString, CommandType.StoredProcedure, strSQL, pars);

            }
            catch(Exception exes)
            {
                throw exes;
            }
            }
    }
}
