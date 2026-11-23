using System.Security.Claims;
using InstructorInfoSystem.Data;
using InstructorInfoSystem.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InstructorInfoSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly PasswordHasher<UserAccount> _passwordHasher = new();

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Register()
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "Instructor");

            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegistrationViewModel model)
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "Instructor");

            if (!ModelState.IsValid)
                return View(model);

            var instructorId = model.InstructorId.Trim();
            var email = model.Email.Trim();
            var normalizedId = instructorId.ToUpperInvariant();
            var normalizedEmail = email.ToUpperInvariant();

            if (await _context.UserAccounts.AnyAsync(a =>
                    a.NormalizedInstructorId == normalizedId))
            {
                ModelState.AddModelError(nameof(model.InstructorId),
                    "This Instructor ID is already registered.");
            }

            if (await _context.UserAccounts.AnyAsync(a =>
                    a.NormalizedEmail == normalizedEmail))
            {
                ModelState.AddModelError(nameof(model.Email),
                    "This email address is already registered.");
            }

            if (!ModelState.IsValid)
                return View(model);

            var account = new UserAccount
            {
                InstructorId = instructorId,
                FirstName = model.FirstName.Trim(),
                LastName = model.LastName.Trim(),
                Email = email,
                NormalizedInstructorId = normalizedId,
                NormalizedEmail = normalizedEmail,
                CreatedAtUtc = DateTime.UtcNow
            };

            account.PasswordHash = _passwordHasher.HashPassword(
                account, model.Password);

            _context.UserAccounts.Add(account);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError(string.Empty,
                    "An account with that Instructor ID or email already exists.");
                return View(model);
            }

            TempData["SuccessMessage"] =
                "Registration successful! You can now log in.";

            return RedirectToAction(nameof(Login));
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "Instructor");

            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(
            LoginViewModel model, string? returnUrl = null)
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "Instructor");

            if (!ModelState.IsValid)
                return View(model);

            var loginInput = model.EmailOrId.Trim().ToUpperInvariant();

            var account = await _context.UserAccounts.FirstOrDefaultAsync(a =>
                a.NormalizedEmail == loginInput ||
                a.NormalizedInstructorId == loginInput);

            if (account == null)
            {
                ModelState.AddModelError(string.Empty,
                    "Invalid email, Instructor ID, or password.");
                return View(model);
            }

            var verification = _passwordHasher.VerifyHashedPassword(
                account, account.PasswordHash, model.Password);

            if (verification == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError(string.Empty,
                    "Invalid email, Instructor ID, or password.");
                return View(model);
            }

            if (verification == PasswordVerificationResult.SuccessRehashNeeded)
            {
                account.PasswordHash = _passwordHasher.HashPassword(
                    account, model.Password);
                await _context.SaveChangesAsync();
            }

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, account.Id.ToString()),
                new(ClaimTypes.Name, $"{account.FirstName} {account.LastName}"),
                new(ClaimTypes.Email, account.Email),
                new("InstructorId", account.InstructorId)
            };

            var identity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties
                {
                    IsPersistent = false,
                    AllowRefresh = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
                });

            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction("Index", "Instructor");
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction(nameof(Login));
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
