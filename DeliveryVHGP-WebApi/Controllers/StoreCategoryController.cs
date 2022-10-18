﻿using DeliveryVHGP_WebApi.IRepositories;
using DeliveryVHGP_WebApi.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryVHGP_WebApi.Controllers
{
    [Route("api/v1/storeCategories")]
    [ApiController]
    public class StoreCategoryController : ControllerBase
    {
        private readonly IStoreCategoryRepository _storeCategoryRepository;

        public StoreCategoryController(IStoreCategoryRepository storeCategoryRepository)
        {
            _storeCategoryRepository = storeCategoryRepository;
        }
        /// <summary>
        /// Get list all storeCategory with pagination
        /// </summary>
        //GET: api/v1/storeCategory?pageIndex=1&pageSize=3
        [HttpGet]
        public async Task<ActionResult> GetAll(int pageIndex, int pageSize)
        {
            return Ok(await _storeCategoryRepository.GetAll(pageIndex, pageSize));
        }
        /// <summary>
        /// Create a storeCategory
        /// </summary>
        //POST: api/v1/storeCategory
        [HttpPost]
        public async Task<ActionResult> CreateStoreCategory(StoreCategoryDto storeCate)
        {
            try
            {
                var result = await _storeCategoryRepository.CreateStoreCategory(storeCate);
                return Ok(result);
            }
            catch
            {
                return Conflict();
            }


        }

    }
}
