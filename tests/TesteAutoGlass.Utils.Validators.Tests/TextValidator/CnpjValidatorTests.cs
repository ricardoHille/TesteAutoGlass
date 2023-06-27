using FluentAssertions;
using TesteAutoGlass.Utils.Validators.TextValidator;
using Xunit;

namespace TesteAutoGlass.Utils.Validators.Tests.TextValidator
{
    public class CnpjValidatorTests
    {
        [Fact]
        public void Cnpj_Deve_Ser_Valido()
        {
            var cnpj = "23405692000153";

            var result = CnpjValidator.CnpjValido(cnpj);

            result.Should().BeTrue();
        }

        [Fact]
        public void Cnpj_Deve_Ser_Invalido()
        {
            var cnpj = "23405692000152";

            var result = CnpjValidator.CnpjValido(cnpj);

            result.Should().BeFalse();
        }

        [Fact]
        public void Cnpj_Deve_Ser_Invalido_Tamanho_Menor()
        {
            var cnpj = "123456789";

            var result = CnpjValidator.CnpjValido(cnpj);

            result.Should().BeFalse();
        }
    }
}

