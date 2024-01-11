﻿using ExchangeRateService.DAL.DatabaseStructures.Repositories;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ExchangeRateService.Controllers
{
    [Route("api/ExchangeRate")]
    [ApiController]
    public class ExchangeRateOnRequestController : ControllerBase
    {

        private IExchangeRateRepo _repo;

        public ExchangeRateOnRequestController(IExchangeRateRepo repo)
        {
            _repo = repo;
        }
    }
}
