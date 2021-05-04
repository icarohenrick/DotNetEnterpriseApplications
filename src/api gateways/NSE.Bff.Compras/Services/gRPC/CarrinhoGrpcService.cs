using NSE.Bff.Compras.Models;
using NSE.Carrinho.API.Service.gRPC;
using System;
using System.Threading.Tasks;

namespace NSE.Bff.Compras.Services.gRPC
{
    public interface ICarrinhoGrpcService
    {
        Task<CarrinhoDTO> ObterCarrinho();
    }

    public class CarrinhoGrpcService : ICarrinhoGrpcService
    {
        private readonly CarrinhoCompras.CarrinhoComprasClient _carrinhoComprasClient;

        public CarrinhoGrpcService(CarrinhoCompras.CarrinhoComprasClient carrinhoComprasClient)
        {
            _carrinhoComprasClient = carrinhoComprasClient;
        }

        public async Task<CarrinhoDTO> ObterCarrinho()
        {
            var response = await _carrinhoComprasClient.ObterCarrinhoAsync(new ObterCarrinhoRequest());
            return MapCarrinhoClienteProtoToDTO(response);
        }

        private static CarrinhoDTO MapCarrinhoClienteProtoToDTO(CarrinhoClienteResponse carrinhoResponse)
        {
            var carrinhoDTO = new CarrinhoDTO()
            {
                ValorTotal = (decimal)carrinhoResponse.Valortotal,
                Desconto = (decimal)carrinhoResponse.Desconto,
                VoucherUtilizado = carrinhoResponse.Voucherutilizado,
            };

            if(carrinhoResponse.Voucher != null)
            {
                carrinhoDTO.Voucher = new VoucherDTO
                {
                    Codigo = carrinhoResponse.Voucher.Codigo,
                    Percentual = (decimal?)carrinhoResponse.Voucher.Percentual,
                    ValorDesconto = (decimal?)carrinhoResponse.Voucher.Valordesconto,
                    TipoDesconto = carrinhoResponse.Voucher.Tipodesconto
                };
            }

            foreach (var item in carrinhoResponse.Itens)
            {
                carrinhoDTO.Itens.Add(new ItemCarrinhoDTO
                {
                    Nome = item.Nome,
                    ProdutoId = Guid.Parse(item.Id),
                    Quantidade = item.Quantidade,
                    Valor = (decimal)item.Valor,
                    Imagem = item.Imagem
                });
            }

            return carrinhoDTO;
        }
    }
}