using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using KilgiAPI.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KilgiAPI.Controllers
{
    [ApiController]
    [Authorize]
    public abstract class BaseApiController : ControllerBase
    {
        protected readonly AppDbContext _context;

        protected BaseApiController(AppDbContext context)
        {
            _context = context;
        }

        protected string? CurrentUserId
        {
            get
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                    ?? User.FindFirst("userId")?.Value
                    ?? User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

                if (!string.IsNullOrEmpty(userId))
                {
                    return userId;
                }

                var username = User.Identity?.Name ?? User.FindFirst(ClaimTypes.Name)?.Value;
                if (!string.IsNullOrEmpty(username))
                {
                    return _context.Users
                        .Where(u => u.Username == username)
                        .Select(u => u.UserId)
                        .FirstOrDefault();
                }

                return null;
            }
        }
    }
}

