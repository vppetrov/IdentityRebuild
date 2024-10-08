﻿using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
	public class AccountController : Controller
	{
		public async Task<IActionResult> Login()
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

			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.Name, "test@test.com"),
				new Claim("LoggedIn", now.ToString("HH:mm:ss")),
				new Claim("Rebuilt", now.ToString("HH:mm:ss")),
			};

			var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
			await this.HttpContext.SignInAsync(
				CookieAuthenticationDefaults.AuthenticationScheme,
				new ClaimsPrincipal(claimsIdentity),
				authProperties);

			return Redirect("/");
		}
	}
}
