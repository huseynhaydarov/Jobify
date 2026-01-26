using Jobify.Application.UseCases.Auths.AuthDtos;
using MediatR;

namespace Jobify.Application.UseCases.Auths.RefreshToken.Commands;

public record RefreshTokenCommand(string AccessToken, string? RefreshToken)
    : IRequest<RefreshTokenResult>;
