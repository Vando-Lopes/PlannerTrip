using FluentValidation;
using Journey.Communication.Requests;

namespace Journey.Application.UseCases.Validators
{
    public class RegisterActiviryValidator : AbstractValidator<RequestRegisterActivityJson>
    {
        public RegisterActiviryValidator() 
        {
            RuleFor(request => request.Name).NotEmpty().WithMessage("Nome não pode vir vazio");
        }  
    }
}
