﻿using Microsoft.AspNetCore.Mvc;
using DeliveryVHGP_WebApi.IRepositories;
using DeliveryVHGP_WebApi.ViewModels;
using DeliveryVHGP_WebApi.Models;

namespace DeliveryVHGP_WebApi.Controllers
{
    [Route("api/v1/store")]
    [ApiController]
    public class StoresController : ControllerBase
    {
        private readonly IStoreRepository _storeRepository;
        private readonly IMenuRepository _menuRepository;
        public StoresController(IStoreRepository storeRepository, IMenuRepository menuRepository)
        {
            _storeRepository = storeRepository;
            _menuRepository = menuRepository;
        }

        /// <summary>
        /// Get list all store with pagination
        /// </summary>
        //GET: api/v1/store?pageIndex=1&pageSize=3
        [HttpGet]
        public async Task<ActionResult> GetAll( int pageIndex, int pageSize)
        {
            return Ok(await _storeRepository.GetListStore( pageIndex, pageSize));
        }
        /// <summary>
        /// Get list all store by brand with pagination
        /// </summary>
        //GET: api/v1/storeByBrand?pageIndex=1&pageSize=3
        [HttpGet("brand/{name}")]
        public async Task<ActionResult> GetListStoreByBrand( string name, int pageIndex, int pageSize)
        {
            return Ok(await _storeRepository.GetListStoreInBrand(name, pageIndex, pageSize));
        } /// <summary>
        /// Get list all store by brand with pagination
        /// </summary>
        //GET: api/v1/storeByBrand?pageIndex=1&pageSize=3
        [HttpGet("store/{name}")]
        public async Task<ActionResult> GetListStoreByName( string name, int pageIndex, int pageSize)
        {
            return Ok(await _storeRepository.GetListStoreByName(name, pageIndex, pageSize));
        }
    
        /// <summary>
        /// Create a product
        /// </summary>
        //POST: api/v1/product
        [HttpPost]
        public async Task<ActionResult> CreateProduct(ProductModel product)
        {
            try
            {
                var result = await _storeRepository.CreatNewProduct(product);
                return Ok(result);
            }
            catch
            {
                return Conflict();
            }
        }
    }
}
