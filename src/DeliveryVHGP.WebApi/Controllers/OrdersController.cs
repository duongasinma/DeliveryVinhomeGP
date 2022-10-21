﻿using Microsoft.AspNetCore.Mvc;
using DeliveryVHGP.Core.Models;
using DeliveryVHGP.Core.Interfaces;

namespace DeliveryVHGP.WebApi.Controllers
{
    [Route("api/v1/orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IRepositoryWrapper repository;
        public OrdersController(IRepositoryWrapper repository)
        {
            this.repository = repository;
        }
        /// <summary>
        /// Get list orders (customer web)
        /// </summary>
        // GET: api/Orders
        [HttpGet("{cusId}/customers")]
        public async Task<ActionResult> GetOrder(string cusId, int pageIndex, int pageSize)
        {
            var listOder = await repository.Order.GetListOrders(cusId, pageIndex, pageSize);
            if (cusId == null)
                return NotFound();
            return Ok(listOder);
        }
        /// <summary>
        /// Get list orders by store (customer web)
        /// </summary>
        // GET: api/Orders
        [HttpGet("stores/byStoreId")]
        public async Task<ActionResult> GetOrderByStore(string storeId, int pageIndex, int pageSize)
        {
            var listOder = await repository.Order.GetListOrdersByStore(storeId, pageIndex, pageSize);
            return Ok(listOder);
        }
        /// <summary>
        /// Get list orders by store (store web)
        /// </summary>
        // GET: api/Orders
        [HttpGet("stores/byStoreId/status/ByStatusId")]
        public async Task<ActionResult> GetOrderByStoreByStatus(string statusId ,string storeId, int pageIndex, int pageSize)
        {
            var listOder = await repository.Order.GetListOrdersByStoreByStatus(storeId, statusId, pageIndex, pageSize);
            if (storeId == null)
                return NotFound();
            return Ok(listOder);
        }
        /// <summary>
        /// Get order by id with pagination
        /// </summary>
        //GET: api/v1/orderById?pageIndex=1&pageSize=3
        [HttpGet("{id}")]
        public async Task<ActionResult> GetOrderDetail(string id)
        {
            var pro = await repository.Order.GetOrdersById(id);
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
                var result = await repository.Order.CreatNewOrder(order);
                return Ok(result);
            }
            catch
            {
                return Conflict();
            }
        }
        /// <summary>
        /// Update a status order (customer web)
        /// </summary>
        //POST: api/v1/order
        [HttpPut("{orderId}")]
        public async Task<ActionResult<OrderDto>> UpdateOrder(string orderId, OrderStatusModel order)
        {
            if (orderId != order.OrderId)
            {
                return BadRequest("Order ID mismatch");
            }
            if (order.OrderId == null)
            {
                return BadRequest("OrderID does not exist");
            }
            try
            {
                await repository.Order.OrderUpdateStatus(orderId, order);
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
            return Ok(order);
        }
    }
}