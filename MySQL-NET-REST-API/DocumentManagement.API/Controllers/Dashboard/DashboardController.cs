using System.Threading.Tasks;
using DocumentManagement.MediatR.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DocumentManagement.API.Controllers.Chart
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        public IMediator _mediator { get; set; }

        public DashboardController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("GetDocumentByCategory")]
        public async Task<IActionResult> GetDocumentByCategory()
        {
            var getDocumentQuery = new GetDocumentsByCategoryQuery();
            var response = await _mediator.Send(getDocumentQuery);
            return Ok(response);
        }

        /// <summary>
        /// Gets the daily reminders.
        /// </summary>
        /// <param name="month">The month.</param>
        /// <param name="year">The year.</param>
        /// <returns></returns>
        [HttpGet("dailyreminder/{month}/{year}")]
        [Produces("application/json", "application/xml", Type = typeof(int))]
        public async Task<IActionResult> GetDailyReminders(int month, int year)
        {
            var monthlyEventQuery = new GetDailyReminderQuery { Month = month, Year = year };
            var result = await _mediator.Send(monthlyEventQuery);
            return Ok(result);
        }

        /// <summary>
        /// Gets the weekly reminders.
        /// </summary>
        /// <param name="month">The month.</param>
        /// <param name="year">The year.</param>
        /// <returns></returns>
        [HttpGet("weeklyreminder/{month}/{year}")]
        [Produces("application/json", "application/xml", Type = typeof(int))]
        public async Task<IActionResult> GetWeeklyReminders(int month, int year)
        {
            var monthlyEventQuery = new GetWeeklyReminderQuery { Month = month, Year = year };
            var result = await _mediator.Send(monthlyEventQuery);
            return Ok(result);
        }

        /// <summary>
        /// Gets the monthly reminders.
        /// </summary>
        /// <param name="month">The month.</param>
        /// <param name="year">The year.</param>
        /// <returns></returns>
        [HttpGet("monthlyreminder/{month}/{year}")]
        [Produces("application/json", "application/xml", Type = typeof(int))]
        public async Task<IActionResult> GetMonthlyReminders(int month, int year)
        {
            var monthlyEventQuery = new GetMonthlyReminderQuery { Month = month, Year = year };
            var result = await _mediator.Send(monthlyEventQuery);
            return Ok(result);
        }

        /// <summary>
        /// Gets the quarterly reminders.
        /// </summary>
        /// <param name="month">The month.</param>
        /// <param name="year">The year.</param>
        /// <returns></returns>
        [HttpGet("quarterlyreminder/{month}/{year}")]
        [Produces("application/json", "application/xml", Type = typeof(int))]
        public async Task<IActionResult> GetQuarterlyReminders(int month, int year)
        {
            var monthlyEventQuery = new GetQuarterlyReminderQuery { Month = month, Year = year };
            var result = await _mediator.Send(monthlyEventQuery);
            return Ok(result);
        }

        /// <summary>
        /// Gets the half yearly reminders.
        /// </summary>
        /// <param name="month">The month.</param>
        /// <param name="year">The year.</param>
        /// <returns></returns>
        [HttpGet("halfyearlyreminder/{month}/{year}")]
        [Produces("application/json", "application/xml", Type = typeof(int))]
        public async Task<IActionResult> GetHalfYearlyReminders(int month, int year)
        {
            var monthlyEventQuery = new GetHalfYearlyReminderQuery { Month = month, Year = year };
            var result = await _mediator.Send(monthlyEventQuery);
            return Ok(result);
        }

        /// <summary>
        /// Gets the yearly reminders.
        /// </summary>
        /// <param name="month">The month.</param>
        /// <param name="year">The year.</param>
        /// <returns></returns>
        [HttpGet("yearlyreminder/{month}/{year}")]
        [Produces("application/json", "application/xml", Type = typeof(int))]
        public async Task<IActionResult> GetYearlyReminders(int month, int year)
        {
            var monthlyEventQuery = new GetYearlyReminderQuery { Month = month, Year = year };
            var result = await _mediator.Send(monthlyEventQuery);
            return Ok(result);
        }

        /// <summary>
        /// Gets the one time reminder.
        /// </summary>
        /// <param name="month">The month.</param>
        /// <param name="year">The year.</param>
        /// <returns></returns>
        [HttpGet("onetimereminder/{month}/{year}")]
        [Produces("application/json", "application/xml", Type = typeof(int))]
        public async Task<IActionResult> GetOneTimeReminder(int month, int year)
        {
            var monthlyEventQuery = new GetOneTimeReminderQuery { Month = month, Year = year };
            var result = await _mediator.Send(monthlyEventQuery);
            return Ok(result);
        }
    }
}
