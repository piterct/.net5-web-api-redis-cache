﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Redis.Cache.API.Commands;
using Redis.Cache.Application.Inrterfaces.Repositories;
using Redis.Cache.Application.Models;

namespace Redis.Cache.API.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class LikeController : Controller
    {
        private readonly ILogger<LikeController> _logger;
        private readonly ILikeRepository _likeRepository;

        public LikeController(ILogger<LikeController> logger, ILikeRepository likeRepository)
        {
            _logger = logger;
            _likeRepository = likeRepository;
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Post(Guid id)
        {
            try
            {
                var like = await _likeRepository.Get(id);
                return Ok(like);
            }
            catch (Exception exception)
            {
                _logger.LogError("An exception has occurred at {dateTime}. " +
                 "Exception message: {message}." +
                 "Exception Trace: {trace}", DateTime.UtcNow, exception.Message, exception.StackTrace);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Post(CreateLikeCommand command)
        {
            try
            {
                var like = await _likeRepository.Add(new Like(command.Name));
                return Ok(like);
            }
            catch (Exception exception)
            {
                _logger.LogError("An exception has occurred at {dateTime}. " +
                 "Exception message: {message}." +
                 "Exception Trace: {trace}", DateTime.UtcNow, exception.Message, exception.StackTrace);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
