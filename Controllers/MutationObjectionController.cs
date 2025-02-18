﻿using BBMPCITZAPI.Models;
using BBMPCITZAPI.Services;
using BBMPCITZAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Reporting.Map.WebForms.BingMaps;
using Newtonsoft.Json;
using NUPMS_BA;
using NUPMS_BO;
using Org.BouncyCastle.Tls;
using System.Data;
using System.Data.Common;
using static BBMPCITZAPI.Models.KaveriData;
using static BBMPCITZAPI.Models.ObjectionModels;

namespace BBMPCITZAPI.Controllers
{

    [Route("v1/MutationObjectionAPI")]
    [ApiController]
    [Authorize]
    public class MutationObjectionController : ControllerBase
    {

        private readonly ILogger<EKYCController> _logger;
        private readonly KaveriSettings _kaveriSettings;
        private readonly INameMatchingService _nameMatchingService;
        private readonly IMutationObjectionService _MutationObjectionService;
        private readonly IErrorLogService _errorLogService;

        public MutationObjectionController(ILogger<EKYCController> logger,
            INameMatchingService NameMatching, IMutationObjectionService MutationObjectionService, IErrorLogService errorLogService)
        {
            _logger = logger;
           
            _nameMatchingService = NameMatching;
            _MutationObjectionService = MutationObjectionService;
            _errorLogService = errorLogService;

        }
        NUPMS_BA.ObjectionModuleBA obj = new NUPMS_BA.ObjectionModuleBA();


        [HttpPost("INS_NCL_MUTATION_OBJECTION_TEMP_WITH_EKYCDATA")]
        public ActionResult<DataSet> INS_NCL_MUTATION_OBJECTION_TEMP_WITH_EKYCDATA(string MOBILENUMBER, string MOBILEVERIFY, string loginId, string EMAIL, EKYCDetailsBO objEKYCDetailsBO)
        {
            _logger.LogInformation("GET request received at INS_NCL_MUTATION_OBJECTION_TEMP_WITH_EKYCDATA");
            try
            {

                var dataSet = _MutationObjectionService.INS_NCL_MUTATION_OBJECTION_TEMP_WITH_EKYCDATA(MOBILENUMBER, MOBILEVERIFY, EMAIL, loginId, objEKYCDetailsBO);
                string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented);

                return Ok(json);
            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "INS_NCL_PROPERTY_SEARCH_TEMP_WITH_EKYCDATA");
                _logger.LogError(ex, "Error occurred while executing stored procedure INS_NCL_PROPERTY_SEARCH_TEMP_WITH_EKYCDATA.");
                throw;
            }
        }

        [HttpPost("INS_NCL_MUTATION_OBJECTION_FINAL_SUBMIT")]
        public ActionResult<DataSet> INS_NCL_MUTATION_OBJECTION_FINAL_SUBMIT(INS_NCL_PROPERTY_MUTATION_OBJECTION_FINAL_SUBMIT final)
        {
            _logger.LogInformation("GET request received at INS_NCL_MUTATION_OBJECTION_FINAL_SUBMIT");
            try
            {

                var dataSet = _MutationObjectionService.INS_NCL_MUTATION_OBJECTION_FINAL_SUBMIT(final);
                string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented);

                return Ok(json);
            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "INS_NCL_PROPERTY_SEARCH_FINAL_SUBMIT");
                _logger.LogError(ex, "Error occurred while executing stored procedure INS_NCL_PROPERTY_SEARCH_FINAL_SUBMIT.");
                throw;
            }
        }
        [HttpGet("Get_Pending_Mutation_Details")]
        public ActionResult<DataSet> Get_Pending_Mutation_Details(string TypeOfSearch,string PropertyEPID,int PageNo,int PageCount)
        {
            _logger.LogInformation("GET request received at INS_NCL_MUTATION_OBJECTION_FINAL_SUBMIT");
            try
            {

                var dataSet = _MutationObjectionService.Get_Pending_Mutation_Details(TypeOfSearch, PropertyEPID, PageNo,  PageCount);
                string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented);

                return Ok(json);
            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "INS_NCL_PROPERTY_SEARCH_FINAL_SUBMIT");
                _logger.LogError(ex, "Error occurred while executing stored procedure INS_NCL_PROPERTY_SEARCH_FINAL_SUBMIT.");
                throw;
            }
        }

    }
}