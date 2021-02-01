using Dapper;
using NSE.Pedidos.API.Application.DTO;
using NSE.Pedidos.Domain.Pedidos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NSE.Pedidos.API.Application.Queries
{
    public interface IPedidoQueries
    {
        Task<PedidoDTO> ObterUltimoPedido(Guid clienteId);
        Task<IEnumerable<PedidoDTO>> ObterListPorClienteId(Guid clienteId);
    }

    public class PedidoQueries : IPedidoQueries
    {
        private readonly IPedidoRepository _pedidoRepository;

        public PedidoQueries(IPedidoRepository pedidoRepository)
        {
            _pedidoRepository = pedidoRepository;
        }

        public async Task<PedidoDTO> ObterUltimoPedido(Guid clienteId)
        {
            const string sql = @"
                                SELECT p.Id as 'ProdutoId', p.Codigo, p.VoucherUtilizado, p.Desconto, p.ValorTotal, p.PedidoStatus,
                                    p.Logradouro, p.Numero, p.Bairro, p.CEP, p.Complemento, p.Cidade, p.Estado,
                                    PIT.Id as 'ProdutoItemId', PIT.ProdutoNome, PIT.Quantidade, PIT.ProdutoImagem, PIT.ValorUnitario
                                FROM Pedidos p
                                    INNER JOIN PedidoItems PIT ON P.Id = PIT.PedidoId
                                WHERE p.ClienteId = @clienteId
                                    AND P.DataCadastro Between DateAdd(minute, -3, GETDATE()) AND DateAdd(minute, 0, GETDATE())
                                    AND p.PedidoStatus = 1
                                ORDER BY p.DataCadastro DESC";

            var pedido = await _pedidoRepository.ObterConexao()
                .QueryAsync<dynamic>(sql, new { clienteId });

            return MapearPedido(pedido);
        }

        public async Task<IEnumerable<PedidoDTO>> ObterListPorClienteId(Guid clienteId)
        {
            var pedidos = await _pedidoRepository.ObterListaPorClienteId(clienteId);

            return pedidos.Select(PedidoDTO.ParaPedidoDTO);
        }

        private PedidoDTO MapearPedido(dynamic result)
        {
            var pedido = new PedidoDTO
            {
                Codigo = result[0].Codigo,
                Status = result[0].PedidoStatus,
                ValorTotal = result[0].ValorTotal,
                Desconto = result[0].Desconto,
                VoucherCodigo = result[0].VoucherUtilizado,

                PedidoItems = new List<PedidoItemDTO>(),
                Endereco = new EnderecoDTO
                {
                    Logradouro = result[0].Logradouro,
                    Bairro = result[0].Bairro,
                    Cep = result[0].Cep,
                    Cidade = result[0].Cidade,
                    Complemento = result[0].Complemento,
                    Estado = result[0].Estado,
                    Numero = result[0].Numero
                }
            };

            foreach (var item in result)
            {
                var pedidoItem = new PedidoItemDTO
                {
                    Nome = item.ProdutoNome,
                    Valor = item.ValorUnitario,
                    Quantidade = item.Quantidade,
                    Imagem = item.ProdutoImagem
                };

                pedido.PedidoItems.Add(pedidoItem);
            }

            return pedido;
        }
    }
}
