using AutoMapper;
using FluentValidation;
using MediatR;
using ShopsRUs.Data;
using ShopsRUs.Data.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ShopsRUs.Core.Core.Application.Commands
{
    public class CreateACustomerCommand : IRequest<(bool, string)>
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserTypeTitle { get; set; }
    }

    public class CreateACustomerCommandValidator : AbstractValidator<CreateACustomerCommand>
    {
        public CreateACustomerCommandValidator()
        {
            RuleFor(x => x.UserName).NotNull().NotEmpty();
            RuleFor(x => x.FirstName).NotNull().NotEmpty();
            RuleFor(x => x.LastName).NotNull().NotEmpty();
            RuleFor(x => x.UserTypeTitle).NotNull().NotEmpty();
        }
    }

    public class ACustomerCommandHandler : IRequestHandler<CreateACustomerCommand, (bool, string)>
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ACustomerCommandHandler(AppDbContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<(bool, string)> Handle(CreateACustomerCommand request, CancellationToken cancellationToken)
        {
            var customerToCreate = _mapper.Map<Customer>(request);

            //confirm if a customer with same username already exists in database
            if (_context.Customers.Any(c => c.UserName == request.UserName)) return (false, "Username already exists");

            //confirm that the usertype exists
            var userType = _context.UserTypes.FirstOrDefault(u => u.Title == request.UserTypeTitle);
            if (userType==null) return (false, $"There is no usertype with title: {request.UserTypeTitle}");

            if (userType.Title == "Regular") return (false, "You can only become a regular user, after 2 years of registration, select the NewComer option");

            customerToCreate.UserType = userType;
            customerToCreate.UserTypeId = string.Empty;
            customerToCreate.UserTypeId = userType.UserTypeId;
            customerToCreate.Orders = new List<Order>();

            _context.Customers.Add(customerToCreate);

            var resp = await SaveAsync();
            if (resp) return (true, "User Created");
            return (false, "Unsuccessful");
        }

        private async Task<bool> SaveAsync()
        {
            var ValueReturned = false;
            if (await _context.SaveChangesAsync() > 0)
                ValueReturned = true;
            else
                ValueReturned = false;
            return ValueReturned;
        }
    }
}
