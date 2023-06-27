using AutoMapper;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TesteAutoGlass.Produtos.Application.Commands;
using TesteAutoGlass.Produtos.Application.Dtos.Filters;
using TesteAutoGlass.Produtos.Application.Dtos.Requests;
using TesteAutoGlass.Produtos.Application.Dtos.Responses;
using TesteAutoGlass.Produtos.Application.Queries.Abstraction;
using TesteAutoGlass.Utils.Abstractions.Command.MediatorHandler.Abstraction;
using TesteAutoGlass.Utils.Abstractions.Pagination.Results;

namespace TesteAutoGlass.Api.Controllers.Produtos
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProdutosController : ControllerBase
    {
        private readonly IProdutoQueries _produtoQueries;
        private readonly IMapper _mapper;
        private readonly IMediatorHandler _mediator;

        public ProdutosController(
            IProdutoQueries ProdutoQueries,
            IMapper mapper,
            IMediatorHandler mediator)
        {
            _produtoQueries = ProdutoQueries;
            _mapper = mapper;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<PagedResult<ProdutoListagemDto>> ObterTodos([FromQuery] PageOptionsProdutoDto pageOptions) =>
            await _produtoQueries.ObterTodosPaginadoAsync(pageOptions);

        [HttpGet("{codigo}")]
        public async Task<ProdutoExibicaoDto> ObterPorCodigo(int codigo) =>
            await _produtoQueries.ObterPorCodigoAsync(codigo);

        [HttpPost]
        public async Task<ValidationResult> CadastrarProduto([FromBody] ProdutoCriacaoDto dto)
        {
            var command = _mapper.Map<ProdutoCriacaoDto, CriarProdutoCommand>(dto);

            var result = await _mediator.SendCommandAsync(command);

            return result;
        }

        [HttpPut]
        public async Task<ValidationResult> EditarProduto([FromBody] ProdutoEdicaoDto dto)
        {
            var command = _mapper.Map<ProdutoEdicaoDto, EditarProdutoCommand>(dto);

            var result = await _mediator.SendCommandAsync(command);

            return result;
        }

        [HttpDelete("id")]
        public async Task<ValidationResult> ExcluirProduto(int id)
        {
            var command = new ExcluirProdutoCommand { Id = id };

            var result = await _mediator.SendCommandAsync(command);

            return result;
        }
    }
}
