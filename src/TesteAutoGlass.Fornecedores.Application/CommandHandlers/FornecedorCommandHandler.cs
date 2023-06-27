using AutoMapper;
using FluentValidation.Results;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TesteAutoGlass.Fornecedores.Application.Commands;
using TesteAutoGlass.Fornecedores.Domain.Entities;
using TesteAutoGlass.Infraestruture.Data.Repository.Fornecedores.Abstraction;
using TesteAutoGlass.Utils.Abstractions.Command.CommandHandlers;

namespace TesteAutoGlass.Fornecedores.Application.CommandHandlers
{
    public class FornecedorCommandHandler : CommandHandlerBase,
                                            IRequestHandler<CriarFornecedorCommand, ValidationResult>,
                                            IRequestHandler<EditarFornecedorCommand, ValidationResult>,
                                            IRequestHandler<ExcluirFornecedorCommand, ValidationResult>
    {
        private readonly IFornecedorRepository _fornecedorRepository;
        private readonly IMapper _mapper;

        public FornecedorCommandHandler(
            IFornecedorRepository fornecedorRepository,
            IMapper mapper)
        {
            _fornecedorRepository = fornecedorRepository;
            _mapper = mapper;
        }

        public async Task<ValidationResult> Handle(CriarFornecedorCommand request, CancellationToken cancellationToken)
        {
            var fornecedorExistente = _fornecedorRepository.Find(x => x.Cnpj.Equals(request.Cnpj)).Any();

            if (fornecedorExistente)
            {
                AddError($"Fornecedor com cnpj {request.Cnpj} já existe na base de dados");
                return ValidationResult;
            }

            var fornecedor = _mapper.Map<CriarFornecedorCommand, Fornecedor>(request);

            if (!CnpjValido(fornecedor))
            {
                return ValidationResult;
            }

            await _fornecedorRepository.InsertAsync(fornecedor);

            return await SaveData(_fornecedorRepository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(EditarFornecedorCommand request, CancellationToken cancellationToken)
        {
            var fornecedorExistente = _fornecedorRepository.Find(x => x.Id == request.Id || (x.Id != request.Id && x.Cnpj.Equals(request.Cnpj)));

            if (!fornecedorExistente.Any(x => x.Id == request.Id))
            {
                AddError($"Fornecedor não encontrado");
                return ValidationResult;
            }

            if (fornecedorExistente.Any(x => x.Id != request.Id && x.Cnpj.Equals(request.Cnpj)))
            {
                AddError($"Não foi possível editar o fornecedor, pois já existe um fornecedor com o mesmo cnpj {request.Cnpj} na base de dados");
                return ValidationResult;    
            }

            var fornecedorEditado = fornecedorExistente.FirstOrDefault(x => x.Id == request.Id);
            fornecedorEditado.Cnpj = request.Cnpj;
            fornecedorEditado.Nome = request.Nome;

            if (!CnpjValido(fornecedorEditado))
            {
                return ValidationResult;
            }

            await _fornecedorRepository.UpdateAsync(fornecedorEditado);

            return await SaveData(_fornecedorRepository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(ExcluirFornecedorCommand request, CancellationToken cancellationToken)
        {
            var fornecedor = await _fornecedorRepository.GetByIdAsync(request.Id);

            if (fornecedor is null)
            {
                AddError($"Fornecedor não encontrado");
                return ValidationResult;
            }

            fornecedor.Inativar();

            await _fornecedorRepository.UpdateAsync(fornecedor);

            return await SaveData(_fornecedorRepository.UnitOfWork);
        }

        private bool CnpjValido(Fornecedor fornecedor)
        {
            if (!fornecedor.CnpjValido())
            {
                AddError($"Cnpj {fornecedor.Cnpj} não é valido");
                return false;
            }

            return true;
        }
    }
}
