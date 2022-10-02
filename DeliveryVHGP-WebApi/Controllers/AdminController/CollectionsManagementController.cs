﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DeliveryVHGP_WebApi.Models;
using DeliveryVHGP_WebApi.IRepositories;
using DeliveryVHGP_WebApi.ViewModels;

namespace DeliveryVHGP_WebApi.Controllers
{
    [Route("api/v1/collection-management/collections")]
    [ApiController]
    public class CollectionsManagementController : ControllerBase
    {
        private readonly ICollectionRepository _collectionRepository;

        public CollectionsManagementController(ICollectionRepository collectionRepository)
        {
            _collectionRepository = collectionRepository;
        }

        /// <summary>
        /// Get list all collection with pagination
        /// </summary>
        //GET: api/v1/collection?pageIndex=1&pageSize=3
        [HttpGet]
        public async Task<ActionResult> GetAll(int pageIndex, int pageSize)
        {
            return Ok(await _collectionRepository.GetAll(pageIndex, pageSize));
        }
        /// <summary>
        /// Create a collection
        /// </summary>
        //POST: api/v1/collection
        [HttpPost]
        public async Task<ActionResult> CreateCollection(CollectionModel collection)
        {
            try
            {
                var result = await _collectionRepository.CreateCollection(collection);
                return Ok(result);
            }
            catch
            {
                return Conflict();
            }
        }
    }
}
