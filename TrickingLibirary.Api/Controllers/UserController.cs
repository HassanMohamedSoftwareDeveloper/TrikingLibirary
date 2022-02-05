﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrickingLibirary.Api.Helpers;
using TrickingLibirary.Domain.Entities;
using TrickingLibirary.Domain.Interfaces;
namespace TrickingLibirary.Api.Controllers;
[Authorize(Tricking_LibiraryConstants.Policies.User)]
public class UserController : ApiController
{
    #region Fields :
    private readonly IDbContext dbContext;
    #endregion
    #region CTORS :
    public UserController(IDbContext dbContext)
    {
        this.dbContext = dbContext;
    }
    #endregion
    #region Endpoints :
    [HttpGet("me")]
    public async Task<IActionResult> GetMe()
    {
        string userId = UserId;
        if (string.IsNullOrWhiteSpace(userId)) return BadRequest();

        var user = await dbContext.Users.FirstOrDefaultAsync(x => x.Id.Equals(userId));
        if (user is not null) return Ok(user);

        user = new User
        {
            Id = userId,
            Username = Username,
        };
        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();
        return Ok(user);
    }
    [HttpGet("{id}")]
    public IActionResult GetUser(string id) => Ok();
    [HttpGet("{id}/submissions")]
    public Task<List<Submission>> GetUserSubmissions(string id)
    => dbContext.Submissions.Include(x => x.Video).Where(x => x.UserId.Equals(id)).ToListAsync();

    #endregion
}
