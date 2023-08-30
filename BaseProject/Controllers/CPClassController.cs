﻿using AutoMapper;
using Core.Data.DTO;
using Microsoft.AspNetCore.Mvc;
using Repository.IRepository;
using Service.IService;
using Service.Service;


namespace BaseProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CPClassController : ControllerBase
    {
        private readonly ICPClassService _cpclassService;
        private readonly ICPClassRepository _cpclassRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public CPClassController(ICPClassService cpclassService, IHttpContextAccessor httpContextAccessor, ICPClassRepository cpclassRepository, IMapper mapper)
        {
            _cpclassService = cpclassService;
            _httpContextAccessor = httpContextAccessor;
            _cpclassRepository = cpclassRepository;
            _mapper = mapper;
        }
        [HttpGet("export")]
        public IActionResult Get(string? Search = null)
        {
            var result = _cpclassService.Export(Search);
            if (result.Success == false)
                return BadRequest(result);
            else
            {
                string FileName = ControllerContext.ActionDescriptor.ControllerName + "_" + DateTime.Now.ToString("dd/MMM/yyyy HH:mm:ss") + ".xlsx";
                Response.Headers.Add("Content-Disposition", "attachment;filename=" + FileName);
                Response.Headers.Add("Content-Type", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                return File((byte[])result.Data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            }
        }
        // GET: api/<CategoryController>
        [HttpGet]
        public IActionResult Get(int pageIndex = 1, int pageSize = int.MaxValue, string? Search = null)
        {
            //var list = _cpclassRepository.PagedList($"", pageIndex, pageSize).List;
            return Ok(_cpclassService.Get(pageIndex,pageSize,Search));
        }

        // GET api/<CategoryController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return Ok(_cpclassService.Get(id));
        }

        // POST api/<CategoryController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CPClassDTO model)
        {
            //var user = _httpContextAccessor.HttpContext.Request.Headers["UserId"];
            if (ModelState.IsValid)
                return Ok(await _cpclassService.CreateOrUpdate(model));
            return BadRequest();
        }
        
        // PUT api/<CategoryController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] CPClassDTO model)
        {
            if (ModelState.IsValid)
                return Ok(await _cpclassService.CreateOrUpdate(model));
            return BadRequest();
        }

        // DELETE api/<CategoryController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return Ok(_cpclassService.Delete(id));
        }

    }
}
