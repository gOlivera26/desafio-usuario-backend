using api_sistema_de_empleados.CQRS.Commands.DetallesUsuario;
using desafio_backend.CQRS.Commands;
using desafio_backend.CQRS.Queries;
using desafio_backend.Dto;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static desafio_backend.CQRS.Commands.PostUsuario;
using static desafio_backend.CQRS.Commands.PutUsuario;
using static desafio_backend.CQRS.Queries.GetUsuario;

namespace desafio_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
       private readonly IMediator _mediator;
        public UsuarioController(IMediator mediator)
        {   
                _mediator = mediator;
        }
        [HttpGet]
        [Route("GetUsuario")]
        public async Task<List<UsuarioDto>> GetUsuario()
        {
            return await _mediator.Send(new GetUsuarioQuery());
        }
        [HttpGet]
        [Route("GetDetalleUsuario/{id}")]
        public async Task<IActionResult> GetDetalleUsuario(int id)
        {
            try
            {
                var detallesUsuario = await _mediator.Send(new GetDetalleUsuario.GetDetalleUsuarioQuery { IdUsuario = id });
                return Ok(detallesUsuario);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Route("GetUsuarioFilter")]
        public async Task<IActionResult> GetUsuarioFilter([FromQuery] UsuarioFilterDto filter)
        {
            try
            {
                var query = new GetUsuarioFilter.GetUsuarioFilterQuery { UsuarioFilter = filter };
                var usuarios = await _mediator.Send(query);
                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [Route("PostUsuario")]
        public async Task<IActionResult> PostUsuario([FromBody] PostUsuarioCommand command)
        {
            try
            {
                var postUsuario = await _mediator.Send(command);
                return CreatedAtAction(nameof(PostUsuario), postUsuario);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [Route("PostDetalleUsuario")]
        public async Task<IActionResult> PostDetalleUsuario([FromBody] PostDetalleUsuario.PostDetalleUsuarioCommand command)
        {
            try
            {
                var detalleUsuario = await _mediator.Send(command);
                return CreatedAtAction(nameof(PostDetalleUsuario), detalleUsuario);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDetalleUsuario(int id)
        {
            try
            {
                await _mediator.Send(new DeleteDetalleUsuario.DeleteDetalleUsuarioCommand { Id = id });
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete]
        [Route("DeleteUsuario/{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            try
            {
                var result = await _mediator.Send(new DeleteUsuario.DeleteUsuarioCommand { Id = id });
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDetalleUsuario(int id, [FromBody] PutDetalleUsuario.PutDetalleUsuarioCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest("El ID del detalle de usuario en la ruta no coincide con el ID proporcionado en el cuerpo.");
            }

            try
            {
                await _mediator.Send(command);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Route("GetRoles")]
        public async Task<IActionResult> GetRoles()
        {
            try
            {
                var roles = await _mediator.Send(new GetRoles.GetRolesQuery());
                return Ok(roles);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("PutUsuario/{id}")]
        public async Task<IActionResult> PutUsuario(int id, [FromBody] PutUsuarioCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest("El ID del usuario en la ruta no coincide con el ID proporcionado en el cuerpo.");
            }

            try
            {
                var usuarioDto = await _mediator.Send(command);
                if (usuarioDto == null)
                {
                    return NotFound("El usuario especificado no existe.");
                }

                return Ok(usuarioDto);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocurrió un error interno en el servidor.");
            }
        }

        [HttpGet]
        [Route("Ping")]
        public IActionResult Ping()
        {
            return Ok("Pong");
        }
        [HttpPost]
        [Route("PostRol")]
        public async Task<IActionResult> PostRol([FromBody] PostRol.PostRolCommand command)
        {
            try
            {
                var postRol = await _mediator.Send(command);
                return CreatedAtAction(nameof(PostRol), postRol);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }   
    }
}
