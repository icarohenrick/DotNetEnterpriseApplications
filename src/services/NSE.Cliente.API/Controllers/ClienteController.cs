using Microsoft.AspNetCore.Mvc;
using NFE.WepAPI.Core.Controllers;
using NSE.Clientes.API.Application.Commands;
using NSE.Core.Mediator;
using System;
using System.Threading.Tasks;

namespace NSE.Clientes.API.Controllers
{
    public class ClienteController : MainController
    {
        private readonly IMediatorHandler _mediatorHandler;

        public ClienteController(IMediatorHandler mediatorHandler) => _mediatorHandler = mediatorHandler;

        [HttpGet("clientes")]
        public async Task<IActionResult> Index()
        {
            var result = await _mediatorHandler.EnviarComando(new RegistrarClienteCommand(Guid.NewGuid(), "Icaro", "icaro@ihtech.com", "76327851088"));

            return CustomResponse(result);
        }
    }
}