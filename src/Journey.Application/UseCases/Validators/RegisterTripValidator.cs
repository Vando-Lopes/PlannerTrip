using FluentValidation;
using Journey.Communication.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Journey.Application.UseCases.Validators
{
    public class RegisterTripValidator : AbstractValidator<RequestRegisterTripJson>
    {
        public RegisterTripValidator()
        {
            RuleFor(request => request.Name).NotEmpty().WithMessage("Nome não pode ser vazio");
            RuleFor(request => request.StartDate.Date).GreaterThanOrEqualTo(DateTime.UtcNow.Date)
                .WithMessage("A viagem não pode ser registrada para uma data passada");
            RuleFor(request => request).Must(request => request.EndDate.Date >= request.StartDate.Date)
                .WithMessage("A viagem deve terminar após a data de inicio.");
        }
    }
}
