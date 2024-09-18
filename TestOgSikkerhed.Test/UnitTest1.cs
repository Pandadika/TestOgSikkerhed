using Bunit;
using Bunit.TestDoubles;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using TestOgSikkerhed.Components.Pages;
using TestOgSikkerhed.Data;

namespace TestOgSikkerhed.Test;

public class UnitTest1
{
	UserManager<ApplicationUser> UserManager { get; set; }
	public UnitTest1()
	{
		UserManager = UserManagerHelper.CreateUserManager();
	}
	[Fact]
	public void AuthorizeView_NotAuthorized_ShouldShowNotAuthorizedText()
	{
		// Arrange
		using var ctx = new TestContext();
		ctx.Services.AddSingleton(UserManager);
		ctx.AddTestAuthorization();

		// Act
		var cut = ctx.RenderComponent<Home>();


		// Assert
		var authText = cut.Find("#authorized-message").TextContent;
		authText.MarkupMatches(@"You are not authorized to access this page.");

		var adminText = cut.Find("#admin-message").TextContent;
		adminText.MarkupMatches(@"you are not an admin");

	}

	[Fact]
	public void AuthorizeView_AuthorizedAndAdmin_ShouldShowAuthorizedAndAdminText()
	{
		// Arrange
		using var ctx = new TestContext();
		ctx.Services.AddSingleton(UserManager);
		var authContext = ctx.AddTestAuthorization();
		authContext.SetAuthorized("TEST USER");
		authContext.SetRoles("Admin");

		// Act
		var cut = ctx.RenderComponent<Home>();

		// Assert
		var authText = cut.Find("#authorized-message").TextContent;
		authText.MarkupMatches(@"You are authorized!");

		var adminText = cut.Find("#admin-message").TextContent;
		adminText.MarkupMatches(@"and you are an administrator.");
	}

	[Fact]
	public void AuthorizeView_AuthorizedNotAdmin_ShouldShowAuthorizedAndNotAdminText()
	{
		// Arrange
		using var ctx = new TestContext();
		ctx.Services.AddSingleton(UserManager);
		var authContext = ctx.AddTestAuthorization();
		authContext.SetAuthorized("TEST USER");

		// Act
		var cut = ctx.RenderComponent<Home>();

		// Assert
		var authText = cut.Find("#authorized-message").TextContent;
		authText.MarkupMatches(@"You are authorized!");

		var adminText = cut.Find("#admin-message").TextContent;
		adminText.MarkupMatches(@"you are not an admin");

	}

	[Fact]
	public void AuthorizeView_AuthorizedNoRoles_ShouldShowAuthorizedAndNotAdminText()
	{
		// Arrange
		using var ctx = new TestContext();
		ctx.Services.AddSingleton(UserManager);
		var authContext = ctx.AddTestAuthorization();
		authContext.SetAuthorized("TEST USER");
		authContext.SetRoles(); // No roles

		// Act
		var cut = ctx.RenderComponent<Home>();

		// Assert
		cut.MarkupMatches(@"<h1>Hello, world!</h1>
                        Welcome to your new app.
                        <p id=""authorized-message"">You are authorized!</p>
                        <p id=""admin-message"">you are not an admin</p>");
	}
}

public static class UserManagerHelper
{
	public static UserManager<ApplicationUser> CreateUserManager()
	{
		var store = Substitute.For<IUserStore<ApplicationUser>>();
		var options = Substitute.For<IOptions<IdentityOptions>>();
		var passwordHasher = Substitute.For<IPasswordHasher<ApplicationUser>>();
		var userValidators = new List<IUserValidator<ApplicationUser>> { Substitute.For<IUserValidator<ApplicationUser>>() };
		var passwordValidators = new List<IPasswordValidator<ApplicationUser>> { Substitute.For<IPasswordValidator<ApplicationUser>>() };
		var keyNormalizer = Substitute.For<ILookupNormalizer>();
		var errors = Substitute.For<IdentityErrorDescriber>();
		var services = Substitute.For<IServiceProvider>();
		var logger = Substitute.For<ILogger<UserManager<ApplicationUser>>>();

		return new UserManager<ApplicationUser>(store, options, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger);
	}
}
