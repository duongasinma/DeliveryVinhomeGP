﻿using Microsoft.AspNetCore.Mvc;
using DeliveryVHGP.Core.Interfaces;

namespace DeliveryVHGP.WebApi.Controllers
{
    [Route("api/v2/brands")]
    [ApiController]
    public class BrandsController : ControllerBase
    {
        private readonly IRepositoryWrapper repository;
        public BrandsController(IRepositoryWrapper repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Get list all Brand with pagination
        /// </summary>
        //GET: api/v1/Brand?pageIndex=1&pageSize=3
        [HttpGet]
        public async Task<ActionResult> GetAll(int pageIndex, int pageSize)
        {
            return Ok(await repository.Brand.GetAll(pageIndex, pageSize));
        }

        /// <summary>
        /// Get a brand by id
        /// </summary>
        //GET: api/v1/brand/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(string id)
        {
            var brand = await repository.Brand.GetById(id);
            if (brand == null)
                return NotFound();
            return Ok(brand);
        }
       
    }
}