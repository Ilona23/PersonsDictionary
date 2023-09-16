using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Domain.Exceptions;
using Serilog;
using System.Linq.Expressions;

namespace Application.PipelineBehaviors
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
       where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            //if (!_validators.Any())
            //{
            //    return await next();
            //}

            //var context = new ValidationContext<TRequest>(request);
            //var errors = _validators
            //    .Select(x => x.Validate(context))
            //    .SelectMany(x => x.Errors)
            //    .Where(x => x != null);

            //if (errors.Any())
            //{
            //    throw new ValidationException(errors);
            //}

            var typeToCheck = typeof(AbstractValidator<>).MakeGenericType(request.GetType());

            var validatorType = request.GetType().Assembly.GetTypes()
                .Where(x => typeToCheck.IsAssignableFrom(x))
                .FirstOrDefault();

            if (validatorType != null)
            {
                var @delegate = Expression.Lambda(Expression.New(validatorType)).Compile();
                var validatorInstance = @delegate.DynamicInvoke();

                var validateMethod = validatorInstance?.GetType().GetMethod("Validate", new[] { request.GetType() });
                var result = validateMethod?.Invoke(validatorInstance, new object[] { request }) as ValidationResult;

                if (!result.IsValid)
                {
                    var exception = new UnprocessableEntityException($"Validation error: '{result}'", result);

                    Log.Error(exception, exception.Message);

                    throw exception;
                }
            }

            var response = await next();

            return response;
        }
    }
}
