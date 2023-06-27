using AutoMapper;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TesteAutoGlass.Fornecedores.Application.Commands;
using TesteAutoGlass.Fornecedores.Application.Dtos;
using TesteAutoGlass.Fornecedores.Application.Dtos.Requests;
using TesteAutoGlass.Fornecedores.Application.Dtos.Responses;
using TesteAutoGlass.Fornecedores.Application.Queries.Abstraction;
using TesteAutoGlass.Utils.Abstractions.Command.MediatorHandler.Abstraction;

namespace TesteAutoGlass.Api.Controllers.Fornecedores
{
    [ApiController]
    [Route("api/[controller]")]
    public class FornecedoresController : ControllerBase
    {
        private readonly IFornecedorQueries _fornecedorQueries;
        private readonly IMapper _mapper;
        private readonly IMediatorHandler _mediator;

        public FornecedoresController(
            IFornecedorQueries fornecedorQueries,
            IMapper mapper,
            IMediatorHandler mediator)
        {
            _fornecedorQueries = fornecedorQueries;
            _mapper = mapper;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IEnumerable<FornecedorListagemDto>> ObterTodos() =>
            await _fornecedorQueries.ObterTodosAsync();

        [HttpGet("{id}")]
        public async Task<FornecedorExibicaoDto> ObterPorId(int id) =>
            await _fornecedorQueries.ObterPorIdAsync(id);

        [HttpPost]
        public async Task<ValidationResult> CadastrarFornecedor([FromBody] FornecedorCriacaoDto dto)
        {
            var command = _mapper.Map<FornecedorCriacaoDto, CriarFornecedorCommand>(dto);

            var result = await _mediator.SendCommandAsync(command);

            return result;
        }

        [HttpPut]
        public async Task<ValidationResult> EditarFornecedor([FromBody] FornecedorEdicaoDto dto)
        {
            var command = _mapper.Map<FornecedorEdicaoDto, EditarFornecedorCommand>(dto);

            var result = await _mediator.SendCommandAsync(command);

            return result;
        }

        [HttpDelete("id")]
        public async Task<ValidationResult> ExcluirFornecedor(int id)
        {
            var command = new ExcluirFornecedorCommand { Id = id };

            var result = await _mediator.SendCommandAsync(command);

            return result;
        }
    }
}
