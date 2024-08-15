using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace WebApplication1
{
	public class CustomCookieAuthenticationEvents : CookieAuthenticationEvents
	{

		public override async Task ValidatePrincipal(CookieValidatePrincipalContext context)
		{
			var authProperties = new AuthenticationProperties
			{
				//AllowRefresh = <bool>,
				// Refreshing the authentication session should be allowed.

				//ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
				// The time at which the authentication ticket expires. A 
				// value set here overrides the ExpireTimeSpan option of 
				// CookieAuthenticationOptions set with AddCookie.

				//IsPersistent = true,
				// Whether the authentication session is persisted across 
				// multiple requests. When used with cookies, controls
				// whether the cookie's lifetime is absolute (matching the
				// lifetime of the authentication ticket) or session-based.

				//IssuedUtc = <DateTimeOffset>,
				// The time at which the authentication ticket was issued.

				//RedirectUri = <string>
				// The full path or absolute URI to be used as an http 
				// redirect response value.
			};

			var now = DateTime.Now;
			var userPrincipal = context.Principal;

			if (userPrincipal == null)
			{
				context.RejectPrincipal();

				await context.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			}
			else
			{
				var claims = new List<Claim>
				{
					new Claim(ClaimTypes.Name, userPrincipal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value ?? "n/a"),
					new Claim("LoggedIn", userPrincipal.Claims.FirstOrDefault(x => x.Type == "LoggedIn")?.Value ?? "n/a"),
					new Claim("Rebuilt", now.ToString("HH:mm:ss")),
				};
				var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

				// Option 1: SignOut / SignIn
				await context.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
				await context.HttpContext.SignInAsync(
					CookieAuthenticationDefaults.AuthenticationScheme,
					new ClaimsPrincipal(claimsIdentity),
					authProperties);

				// Option 2: SignIn only
				//await context.HttpContext.SignInAsync(
				//	CookieAuthenticationDefaults.AuthenticationScheme,
				//	new ClaimsPrincipal(claimsIdentity),
				//	authProperties);

				// Option 3: Change claims only
				//foreach (var claim in userPrincipal.Claims.ToList())
				//{
				//	(userPrincipal.Identity as ClaimsIdentity).RemoveClaim(claim);
				//}
				//foreach (var claim in claims)
				//{
				//	(userPrincipal.Identity as ClaimsIdentity).AddClaim(claim);
				//}


			}
		}
	}
}
