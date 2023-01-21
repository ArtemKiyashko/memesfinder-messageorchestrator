using FluentValidation;
using Telegram.Bot.Types;

namespace MemesFinderMessageOrchestrator.Validators
{
    public class MessageValidator : AbstractValidator<Message>
    {
        public MessageValidator()
        {
            RuleFor(message => message.Text).Cascade(CascadeMode.Stop).NotNull().NotEmpty();
        }
    }
}
