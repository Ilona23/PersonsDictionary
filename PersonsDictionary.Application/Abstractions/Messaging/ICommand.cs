using MediatR;

namespace Application.Abstractions.Messaging
{
    public interface ICommand<out TResult> : IRequest<TResult>
    {

    }
}
