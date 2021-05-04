using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSE.Catalogo.API.Models;
using NSE.WepAPI.Core.Controllers;
using NSE.WepAPI.Core.Identidade;

namespace NSE.Catalogo.API.Controllers
{
    [Authorize]
    public class CatalogoController : MainController
    {
        private readonly IProdutoRepository _produtoRepository;

        public CatalogoController(IProdutoRepository produtoRepository) => _produtoRepository = produtoRepository;

        [AllowAnonymous]
        [HttpGet("catalogo/produtos")]
        public async Task<PagedResult<Produto>> Index(
            [FromQuery] int ps=3, //Quantos Registros
            [FromQuery] int page = 1, //Qual pagina estou
            [FromQuery] string q = null) 
            => await _produtoRepository.ObterProdutos(ps, page, q);

        [ClaimsAuthorize("Catalogo", "Ler")]
        [HttpGet("catalogo/produtos/{id}")]
        public async Task<Produto> ProdutoDetalhe(Guid id) => await _produtoRepository.ObterPorId(id);

        [HttpGet("catalogo/produtos/lista/{ids}")]
        public async Task<IEnumerable<Produto>> ObterProdutosPorId(string ids) => await _produtoRepository.ObterProdutosPorId(ids);
    }
}