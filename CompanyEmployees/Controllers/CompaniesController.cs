using System;
using Contracts;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using System.Collections.Generic;
using Entities.DataTransferObjects;
using Entities.Models;
using System.Linq;
using CompanyEmployees.ModelBinders;

namespace CompanyEmployees.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private ILoggerManager _logger;
        private readonly IRepositroryManager _repositrory;
        private readonly IMapper _mapper;
        public CompaniesController(ILoggerManager logger, IRepositroryManager repositrory , IMapper mapper)
        {
            this._logger = logger;
            this._repositrory = repositrory;
            this._mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetCompanies()
        {
                var companies = _repositrory.Company.GetCompanies(trackChanges: false);
                var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);
                return Ok(companiesDto);
        }

        [HttpGet("{id}" , Name ="CompanyByID")]
        public IActionResult GetCompany(Guid id)
        {
            var comapny = _repositrory.Company.GetCompany(id, trackChanges: false);
            if (comapny==null)
            {
                _logger.LogInfo($"Company Wiht Id : {id} doesn't exist in the database");
                return NotFound();
            }
            else
            {
                var companyDto = _mapper.Map<CompanyDto>(comapny);
                return Ok(companyDto);
            }
        }

        [HttpGet("collection/({ids})" , Name ="CompanyCollection")]
        public IActionResult GetCompanyCollection(   [ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            if (ids ==null)
            {
                _logger.LogError("Parameter ids is null");
                return BadRequest("Parameter ids is null");
            }
            var companyEntites = _repositrory.Company.GetByIds(ids, trackChanges: false);
            // Id valid + id not valid ==> Collection ? 
            if (ids.Count() != companyEntites.Count())
            {
                _logger.LogError("Parameter ids is null");
                return NotFound();
            }
            var companyToReturn = _mapper.Map<IEnumerable<CompanyDto>>(companyEntites);
            return Ok(companyToReturn);
        }

        [HttpPost("collection")]
        public IActionResult CreateCompanyCollection([FromBody]IEnumerable<CompanyForCreateDto> companyCollection)
        {
            if (companyCollection == null)
            {
                _logger.LogError("Company Collection Object sent from client is null");
                return BadRequest("Company Collection Object is null");
            }
            var companyEntites = _mapper.Map<IEnumerable<Company>>(companyCollection);
            foreach (var company in companyEntites)
            {
                _repositrory.Company.CreateCompany(company);
            }
            _repositrory.Save();
            // return result ==> Saved 
            var companyCollectiontoReturn = _mapper.Map<IEnumerable<CompanyDto>>(companyEntites);
            var ids = String.Join(",", companyCollectiontoReturn.Select(c => c.Id));
            return CreatedAtRoute("CompanyCollection", new { ids }, companyCollectiontoReturn);
        }
        [HttpPost]
        public  IActionResult CreateCompany([FromBody]CompanyForCreateDto company)
        {
            if (company ==null)
            {
                _logger.LogError("CompanyForCreateDto Object sent from client is null");
                return BadRequest("CompanyForCreateDto Object is null");
            }
            var companyEntity = _mapper.Map<Company>(company);
            _repositrory.Company.CreateCompany(companyEntity);
            _repositrory.Save();
            // return result ==> Saved 
            var companytoReturn= _mapper.Map<CompanyDto>(companyEntity);
            return CreatedAtRoute("CompanyByID", new { id = companytoReturn.Id }, companytoReturn);
        }
    }
}