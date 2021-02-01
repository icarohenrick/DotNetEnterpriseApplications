using FluentValidation;
using NSE.Core.Messages;
using NSE.Pedidos.API.Application.DTO;
using System;
using System.Collections.Generic;

namespace NSE.Pedidos.API.Application.Commands
{
    public class AdicionarPedidoCommand : Command
    {
        // Pedido
        public Guid ClienteId { get; set; }
        public decimal ValorTotal { get; set; }
        public List<PedidoItemDTO> PedidoItems { get; set; }

        //Voucher
        public string VoucherCodigo { get; set; }
        public bool VoucherUtilizado { get; set; }
        public decimal Desconto { get; set; }

        //Endereco
        public EnderecoDTO Endereco { get; set; }

        //Cartao
        public string NumeroCartao { get; set; }
        public string NomeCartao { get; set; }
        public string ExpiracaoCartao { get; set; }
        public string CvvCartao { get; set; }

        public override bool EhValido()
        {
            ValidationResult = new AdicionarPedidoValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class AdicionarPedidoValidation : AbstractValidator<AdicionarPedidoCommand>
    {
        public AdicionarPedidoValidation()
        {
            RuleFor(c => c.ClienteId)
                .NotEqual(Guid.Empty)
                .WithMessage("Id do cliente inválido");

            RuleFor(c => c.PedidoItems.Count)
                .GreaterThan(0)
                .WithMessage("O pedido precisa ter no mínimo 1 item");

            RuleFor(c => c.ValorTotal)
                .GreaterThan(0)
                .WithMessage("Valor pedido inválido");

            RuleFor(c => c.NumeroCartao)
                .CreditCard()
                .WithMessage("Número do Cartão inválido");

            RuleFor(c => c.NomeCartao)
                .NotNull()
                .WithMessage("Número do Cartão inválido");

            RuleFor(c => c.NumeroCartao)
                .CreditCard()
                .WithMessage("Nome do portador do cartão requerido");

            RuleFor(c => c.CvvCartao.Length)
                .GreaterThan(2)
                .LessThan(5)
                .WithMessage("O CVV do Cartão precisa 3 ou 4 números.");

            RuleFor(c => c.ExpiracaoCartao)
                .NotNull()
                .WithMessage("Data de expiraçaõ do Cartão requerida.");
        }
    }
}