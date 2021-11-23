﻿using ECO.Sample.Application.Utils;
using ECO.Sample.Presentation.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ECO.Sample.Presentation.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMediator _Mediator;

        private readonly ILogger<HomeController> _logger;        

        public HomeController(IMediator mediator, ILogger<HomeController> logger)
        {
            _Mediator = mediator;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> PutFakeData()
        {
            await _Mediator.Send(new CreateFakeData.Command());
            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
