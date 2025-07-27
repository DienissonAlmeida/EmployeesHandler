using EmployeeManagement.Application.Commands;
using EmployeeManagement.Domain.Contracts;
using FluentValidation;

namespace EmployeeManagement.Application.Validations
{
    public class CreateEmployeeValidator : AbstractValidator<CreateEmployeeCommand>
    {
        private readonly IEmployeeRepository _repository;

        public CreateEmployeeValidator(IEmployeeRepository repository)
        {
            _repository = repository;

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("A valid email is required.");

            RuleFor(x => x.DocumentNumber)
                .NotEmpty().WithMessage("Document number is required.")
                .MustAsync(ExistsByDocumentNumber).WithMessage("Document number must be unique.");

            RuleForEach(x => x.PhoneNumbers)
                .NotEmpty().WithMessage("Phone number cannot be empty.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters.");

            // Optional field: ManagerId can be null or must exist
            When(x => x.ManagerId.HasValue, () =>
            {
                RuleFor(x => x.ManagerId.Value)
                    .MustAsync(ExistsManagerId).WithMessage("Manager must be a valid employee.");
            });

            RuleFor(x => x.BirthDate)
                .NotEmpty().WithMessage("Birth date is required.")
                .Must(BeAtLeast18).WithMessage("Employee must be at least 18 years old.");
        }

        private bool BeAtLeast18(DateTime birthDate)
        {
            var age = DateTime.Today.Year - birthDate.Year;
            if (birthDate > DateTime.Today.AddYears(-age)) age--;
            return age >= 18;
        }
        private async Task<bool> ExistsByDocumentNumber(string document, CancellationToken cancellationToken)
        {
            return !await _repository.ExistsByDocumentAsync(document);
        }

        private async Task<bool> ExistsManagerId(Guid managerId, CancellationToken cancellationToken)
        {
            return await _repository.ExistsByIdAsync(managerId);
        }
    }

}
