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
    [Route("api/v1/orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;

        public OrdersController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        /// <summary>
        /// Get list orders
        /// </summary>
        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult> GetOrder(int pageIndex, int pageSize)
        {

                var result = Ok(await _orderRepository.GetListOrders(pageIndex, pageSize));
            
                return Ok(result);
        }
        /// <summary>
        /// Get order by id with pagination
        /// </summary>
        //GET: api/v1/orderById?pageIndex=1&pageSize=3
        [HttpGet("{id}")]
        public async Task<ActionResult> GetOrderDetail(string id)
        {
            var pro = await _orderRepository.GetOrdersById(id);
            if (pro == null)
                return NotFound();
            return Ok(pro);
        }
        /// <summary>
        /// Create a order (customer web)
        /// </summary>
        //POST: api/v1/order
        [HttpPost]
        public async Task<ActionResult> CreatNewOrder(OrderDto order)
        {
            try
            {
                var result = await _orderRepository.CreatNewOrder(order);
                return Ok(result);
            }
            catch
            {
                return Conflict();
            }
        }
    }
}
