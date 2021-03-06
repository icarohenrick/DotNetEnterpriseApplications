﻿using Microsoft.EntityFrameworkCore;
using NSE.Clientes.API.Models;
using NSE.Core.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NSE.Clientes.API.Data.Repository
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly ClientesContext _context;

        public ClienteRepository(ClientesContext context) => _context = context;

        public IUnitOfWork UnitOfWork => _context;

        public async Task<IEnumerable<Cliente>> ObterTodos() 
            => await _context.Clientes.AsNoTracking().ToListAsync();

        public async Task<Cliente> ObterPorCpf(string cpf) 
            => await _context.Clientes.FirstOrDefaultAsync(c => c.CPF.Numero == cpf);

        public void Adicionar(Cliente cliente) => _context.Clientes.Add(cliente);

        public async Task<Endereco> ObterEnderecoPorId(Guid id)
            => await _context.Enderecos.FirstOrDefaultAsync(x => x.ClienteId == id);

        public void AdicionarEndereco(Endereco endereco) => _context.Enderecos.Add(endereco);

        public void Dispose() => _context.Dispose();
    }
}
