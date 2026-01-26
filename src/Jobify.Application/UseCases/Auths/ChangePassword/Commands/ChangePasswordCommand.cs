using MediatR;

namespace Jobify.Application.UseCases.Auths.ChangePassword.Commands;

public record ChangePasswordCommand(
    string CurrentPassword,
    string NewPassword,
    string ConfirmNewPassword) : IRequest<Unit>;
