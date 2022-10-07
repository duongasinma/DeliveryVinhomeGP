﻿using Microsoft.AspNetCore.Mvc;
using DeliveryVHGP_WebApi.IRepositories;
using DeliveryVHGP_WebApi.ViewModels;
using DeliveryVHGP_WebApi.Models;

namespace DeliveryVHGP_WebApi.Controllers
{
    [Route("api/v1/stores")]
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
        [HttpGet("search/{brandName}")]
        public async Task<ActionResult> GetListStoreByBrand( string brandName, int pageIndex, int pageSize)
        {
            return Ok(await _storeRepository.GetListStoreInBrand(brandName, pageIndex, pageSize));
        } /// <summary>
        /// Get list all store by brand with pagination
        /// </summary>
        //GET: api/v1/storeByBrand?pageIndex=1&pageSize=3
        [HttpGet("search/{storeName}")]
        public async Task<ActionResult> GetListStoreByName( string storeName, int pageIndex, int pageSize)
        {
            return Ok(await _storeRepository.GetListStoreByName(storeName, pageIndex, pageSize));
        }
    }

}
