using Microsoft.AspNetCore.Mvc;
using NSE.Core.Mediator;
using NSE.Pedidos.API.Application.Commands;
using NSE.Pedidos.API.Application.Queries;
using NSE.WepAPI.Core.Controllers;
using NSE.WepAPI.Core.Usuario;
using System.Threading.Tasks;

namespace NSE.Pedidos.API.Controllers
{
    public class PedidoController : MainController
    {
        private readonly IMediatorHandler _mediator;
        private readonly IAspNetUser _user;
        private readonly IPedidoQueries _pedidoQueries;

        public PedidoController(
            IMediatorHandler mediator, 
            IAspNetUser user, 
            IPedidoQueries pedidoQueries)
        {
            _mediator = mediator;
            _user = user;
            _pedidoQueries = pedidoQueries;
        }

        [HttpPost("pedido")]
        public async Task<IActionResult> AdicionarPedido(AdicionarPedidoCommand pedido)
        {
            pedido.ClienteId = _user.ObterUserId();
            return CustomResponse(await _mediator.EnviarComando(pedido));
        }

        [HttpGet("pedido/ultimo")]
        public async Task<IActionResult> UltimoPedido()
        {
            var pedidos = await _pedidoQueries.ObterUltimoPedido(_user.ObterUserId());
            return pedidos == null ? NotFound() : CustomResponse(pedidos);
        }

        [HttpGet("pedido/lista-cliente")]
        public async Task<IActionResult> ListaPorCliente()
        {
            var pedidos = await _pedidoQueries.ObterListPorClienteId(_user.ObterUserId());
            return pedidos == null ? NotFound() : CustomResponse(pedidos);
        }
    }
}
