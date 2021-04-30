﻿using Dapper;
using Microsoft.EntityFrameworkCore;
using NSE.Catalogo.API.Models;
using NSE.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NSE.Catalogo.API.Data.Repository
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly CatalogoContext _context;

        public ProdutoRepository(CatalogoContext context) => _context = context;

        public IUnitOfWork UnitOfWork => _context;

        public async Task<PagedResult<Produto>> ObterProdutos(int pageSize, int pageIndex, string query = null) 
        {
            //EF Paginação
            //return await _context.Produtos
            //    .Skip(pageSize*(pageSize - 1)) //descubro em que página de dados estou
            //    .Take(pageSize) //Informa quantidade de dados a ser retornado (Top xx)
            //    .AsNoTracking().ToListAsync();

            var sql = $@"SELECT * FROM Produtos
                        WHERE (@Nome IS NULL OR Nome LIKE '%' + @Nome + '%')
                        ORDER BY [Nome]
                        OFFSET { pageSize * (pageIndex - 1) } ROWS
                        FETCH NEXT { pageSize } ROWS ONLY
                        SELECT COUNT(Id) FROM Produtos
                        WHERE (@Nome IS NULL OR Nome LIKE '%' + @Nome + '%')";

            var multi = await _context.Database.GetDbConnection()
                .QueryMultipleAsync(sql, new { Nome = query });

            var produtos = multi.Read<Produto>();
            var total = multi.Read<int>().FirstOrDefault();

            return new PagedResult<Produto>
            {
                List = produtos,
                PageIndex = pageIndex,
                PageSize = pageSize,
                Query = query,
                TotalResults = total
            };           
        }

        public async Task<List<Produto>> ObterProdutosPorId(string ids)
        {
            var idsGuid = ids.Split(',')
                .Select(id => (Ok: Guid.TryParse(id, out var x), Value: x));

            if (!idsGuid.All(nid => nid.Ok)) return new List<Produto>();

            var idsValue = idsGuid.Select(id => id.Value);

            return await _context.Produtos.AsNoTracking()
                .Where(p => idsValue.Contains(p.Id) && p.Ativo).ToListAsync();
        }

        public async Task<Produto> ObterPorId(Guid id) => await _context.Produtos.FindAsync(id);

        public void Adicionar(Produto produto) => _context.Produtos.Add(produto);

        public void Atualizar(Produto produto) => _context.Produtos.Update(produto);

        public void Dispose() => _context?.Dispose();
    }
}
