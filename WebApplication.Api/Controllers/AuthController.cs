using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplicationApi.Dati;
using WebApplicationApi.Models.User;

namespace WebApplicationApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		public ILogger<AuthController> logger;
		private readonly IMapper mapper;
		private readonly UserManager<ApiUser> userManager;

		public AuthController(ILogger<AuthController> _logger, IMapper _mapper, UserManager<ApiUser> _userManager)
		{
			logger = _logger;
			mapper = _mapper;
			userManager = _userManager;
		}

		[HttpPost("Register")]
		public async Task<IActionResult> Register(UserDto userDto)
		{
			logger.LogInformation($"Registreation Attempt for {userDto.Email}");

			try
			{
				var utente = mapper.Map<ApiUser>(userDto);
				utente.UserName = userDto.Email;
				var risult = await userManager.CreateAsync(utente, userDto.Password);

				if (risult.Succeeded == false)
				{
					foreach (var error in risult.Errors)
					{
						ModelState.AddModelError(error.Code, error.Description);
					}
					return BadRequest(ModelState);
				}

				await userManager.AddToRoleAsync(utente, "User");
				return Accepted();
			}
			catch (Exception ex)
			{
				logger.LogError(ex, $"Something Went Wrong in the  {nameof(Register)} 😭");
				return Problem($"Something Went Wrong in the {nameof(Register)}", statusCode: 500);
			}
		}


		[HttpPost("login")]
		public async Task<IActionResult> Login(LoginUserDto userDto)
		{
			logger.LogInformation($"Login Attempt for {userDto.Email}");

			try
			{
				var user = await userManager.FindByEmailAsync(userDto.Email);
				var passwordValid = await userManager.CheckPasswordAsync(user, userDto.Password);

				if (user == null || passwordValid == false)
					return NotFound();

				return Accepted();

			}
			catch (Exception ex)
			{
				logger.LogError(ex, $"Something Went Wrong in the  {nameof(Register)} 😭");
				return Problem($"Something Went Wrong in the {nameof(Register)}", statusCode: 500);
			}
		}
	}
}

//try
//{

//}
//catch (Exception ex)
//{
//	logger.LogError(ex, $"Something Went Wrong in the  {nameof(Register)} 😭");
//	return Problem($"Something Went Wrong in the {nameof(Register)}", statusCode: 500);
//}