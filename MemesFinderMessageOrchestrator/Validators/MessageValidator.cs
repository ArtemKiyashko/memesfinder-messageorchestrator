using FluentValidation;
using MemesFinderMessageOrchestrator.Extentions;
using Telegram.Bot.Types;

namespace MemesFinderMessageOrchestrator.Validators
{
    public class MessageValidator : AbstractValidator<Message>
    {
        public MessageValidator()
        {
            RuleFor(message => message.GetEffectiveText()).Cascade(CascadeMode.Stop).NotNull().NotEmpty();
        }
    }
}
