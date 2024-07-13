using Microsoft.AspNetCore.Mvc.Filters;

namespace MusicStore.Api.Filters
{
	public class FilterExceptions : ExceptionFilterAttribute
	{
		private readonly ILogger<FilterExceptions> _logger;

		public FilterExceptions(ILogger<FilterExceptions> logger)
		{
			_logger = logger;
		}

		public override void OnException(ExceptionContext context)
		{
			_logger.LogError(context.Exception, context.Exception.Message);
			base.OnException(context);
		}
	}
}
