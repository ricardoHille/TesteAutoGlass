using AutoMapper;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System.Threading;
using System.Threading.Tasks;
using TesteAutoGlass.Infraestruture.Data.Repository.Produtos.Abstraction;
using TesteAutoGlass.Produtos.Application.Commands;
using TesteAutoGlass.Produtos.Domain.Entities;
using TesteAutoGlass.Utils.Abstractions.Command.CommandHandlers;

namespace TesteAutoGlass.Produtos.Application.CommandHandlers
{
    public class ProdutoCommandHandler : CommandHandlerBase,
                                         IRequestHandler<CriarProdutoCommand, ValidationResult>,
                                         IRequestHandler<EditarProdutoCommand, ValidationResult>,
                                         IRequestHandler<ExcluirProdutoCommand, ValidationResult>
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IMapper _mapper;

        public ProdutoCommandHandler(
            IProdutoRepository produtoRepository,
            IMapper mapper)
        {
            _produtoRepository = produtoRepository;
            _mapper = mapper;
        }

        public async Task<ValidationResult> Handle(CriarProdutoCommand request, CancellationToken cancellationToken)
        {
            var produto = _mapper.Map<CriarProdutoCommand, Produto>(request);
         
            if (!DataFabricacaoValida(produto))
            {
                return ValidationResult;
            }

            await _produtoRepository.InsertAsync(produto);

            return await SaveData(_produtoRepository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(EditarProdutoCommand request, CancellationToken cancellationToken)
        {
            var produto = await _produtoRepository.GetByIdAsync(request.Id);

            if (produto is null)
            {
                AddError("Produto não encontrado");
                return ValidationResult;
            }

            produto.FornecedorId = request.FornecedorId;
            produto.Fabricacao = request.Fabricacao;
            produto.Nome = request.Nome;
            produto.Validade = request.Validade;

            if (!DataFabricacaoValida(produto))
            {
                return ValidationResult;
            }

            await _produtoRepository.UpdateAsync(produto);

            return await SaveData(_produtoRepository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(ExcluirProdutoCommand request, CancellationToken cancellationToken)
        {
            var produto = await _produtoRepository.GetByIdAsync(request.Id);

            if (produto is null)
            {
                AddError("Produto não encontrado");
                return ValidationResult;
            }

            produto.Inativar();

            await _produtoRepository.UpdateAsync(produto);

            return await SaveData(_produtoRepository.UnitOfWork);
        }

        private bool DataFabricacaoValida(Produto produto)
        {
            if (!produto.DataFabricacaoValida())
            {
                AddError("Data de fabricação não pode ser igual ou maior a data de validade");
                return false;
            }

            return true;
        }
    }
}
