﻿using Microsoft.AspNetCore.Mvc;
using NSE.WebApp.MVC.Models;
using NSE.WebApp.MVC.Services;
using System.Threading.Tasks;

namespace NSE.WebApp.MVC.Extensions
{
    public class CarrinhoViewComponent : ViewComponent
    {
        private readonly IComprasBffService _comprasBffService;

        public CarrinhoViewComponent(IComprasBffService comprasBffService) => _comprasBffService = comprasBffService;

        public async Task<IViewComponentResult> InvokeAsync()
            => View(await _comprasBffService.ObterQuantidadeCarrinho());
    }
}