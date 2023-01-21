using System;
using FluentValidation;
using MemesFinderMessageOrchestrator.Models.AnalysisModels;

namespace MemesFinderMessageOrchestrator.Validators
{
	public class ConversationResponseModelValidator : AbstractValidator<ConversationResponseModel>
    {
		public ConversationResponseModelValidator()
		{
			RuleFor(response => response.Result).NotNull();
            RuleFor(response => response.Result.Prediction).NotNull();
            RuleFor(response => response.Result.Prediction.TopIntent).Cascade(CascadeMode.Stop).NotNull().NotEmpty();
			RuleFor(response => response.Result.Prediction.Entities).NotEmpty();
        }
	}
}

