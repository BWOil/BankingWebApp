﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AdminWebAPI.Models.DataManager;
using DataModelLibrary.Models;
using Microsoft.AspNetCore.Authorization;


namespace AdminWebAPI.Controllers
{
    // pass the authentication token as header when calling from api/customers
    [Route("api/[controller]")]
    public class CustomersController : Controller
    {
        private readonly CustomerManager _repo;

        public CustomersController(CustomerManager repo)
        {
            _repo = repo;
        }

        // HTTP Method: GET
        // Url: api/customers
        // Can fetch all the customers in the database
        [HttpGet]
        [Authorize]
        public IActionResult Get()
        {
            var customers = _repo.GetAll();
            if (customers == null)
            {
                return NotFound(); // Return 404 Not Found if no customers found
            }
            return Ok(customers);
        }

        // HTTP Method: GET
        // Url: api/customers/{id}
        // Find a customer with an valid customer ID
        [HttpGet("{id}")]
        [Authorize]
        public IActionResult Get(int id)
        {
            var customer = _repo.Get(id);
            if (customer == null)
                return NotFound(); // Return 404 Not Found if customer not found
            return Ok(customer);
        }


        // HTTP Method: PUT
        // Url: api/customers/{id}
        // Update a customer's profile by customer ID
        [HttpPut("{id}")]
        [Authorize]
        public IActionResult Put(int id, [FromBody] Customer customer)
        {
            if (id != customer.CustomerID || customer == null)
                return BadRequest("Customer ID mismatch."); // Return 400 Bad Request if customer ID mismatch
            _repo.Update(id, customer);
            return Ok();
        }

        // HTTP Method: PUT
        // Url: api/customers/{id}
        // Toggle to lock or unlock a customer by customer ID
        [HttpPut("toggle-lock/{id}")]
        [Authorize]
        public IActionResult ToggleLock(int id)
        {
            var toggleCustomerId = _repo.ToggleLockCustomer(id);
            if (toggleCustomerId == 0)
                return NotFound(); // Return 404 Not Found if customer not found
            return Ok();
        }

    }
}

