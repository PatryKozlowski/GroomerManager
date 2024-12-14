using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GroomerManager.API.Controller;

public abstract class BaseController(IMediator mediator) : ControllerBase
{
}