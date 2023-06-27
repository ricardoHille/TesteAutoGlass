using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TesteAutoGlass.Fornecedores.Application.Dtos;
using TesteAutoGlass.Fornecedores.Application.Dtos.Responses;
using TesteAutoGlass.Fornecedores.Application.Queries.Abstraction;
using TesteAutoGlass.Fornecedores.Domain.Entities;
using TesteAutoGlass.Infraestruture.Data.Repository.Fornecedores.Abstraction;

namespace TesteAutoGlass.Fornecedores.Application.Queries
{
    public class FornecedorQueries : IFornecedorQueries
    {
        private readonly IFornecedorRepository _fornecedorRepository;
        private readonly IMapper _mapper;

        public FornecedorQueries(
            IFornecedorRepository fornecedorRepository,
            IMapper mapper)
        {
            _fornecedorRepository = fornecedorRepository;
            _mapper = mapper;
        }

        public async Task<FornecedorExibicaoDto> ObterPorIdAsync(int id)
        {
            var fornecedor = await _fornecedorRepository.GetByIdAsync(id);

            return _mapper.Map<Fornecedor, FornecedorExibicaoDto>(fornecedor);
        }

        public async Task<IEnumerable<FornecedorListagemDto>> ObterTodosAsync()
        {
            var fornecedores = await _fornecedorRepository.GetAllAsync();

            return fornecedores.Where(x => x.Ativo).Select(x => _mapper.Map<Fornecedor, FornecedorListagemDto>(x));
        }
    }
}
