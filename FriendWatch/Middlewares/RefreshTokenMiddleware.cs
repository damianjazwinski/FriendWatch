﻿using System.Net;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;

namespace FriendWatch.Middlewares
{
    public class RefreshTokenMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _authControllerName = "Auth";
        private readonly string _refreshTokenActionName = "RefreshToken";
        public RefreshTokenMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var endpoint = context.GetEndpoint();

            if (endpoint?.Metadata.GetMetadata<IAuthorizeData>() is null)
            {
                await _next(context);
                return;
            }


            // check refresh token
            var controllerName = (string?) context.GetRouteValue("controller");
            var actionName = (string?) context.GetRouteValue("action");
            var tokenType = context.User.Claims.SingleOrDefault(claim => claim.Type == "tokenType")?.Value;

            if (controllerName == _authControllerName && actionName == _refreshTokenActionName && tokenType == "refresh")
            {
                await _next(context);
                return;
            }
            else if (tokenType == "access" && !(controllerName == _authControllerName && actionName == _refreshTokenActionName))
            {
                await _next(context);
                return;
            }

            context.Response.Clear();
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;

            await context.Response.WriteAsync("Unauthorized");
        }
    }
}
