using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Test.Core.Repositories;
using Test.Models;

namespace Test.Controllers
{
    [Route("api/User")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private UnitOfWork _unitOfWork;
        private GenericRepository<User> _repoUser;

        public UserController()
        {
            _unitOfWork = new UnitOfWork();
            _repoUser = _unitOfWork.Repository<User>();
        }

        [HttpGet("All")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _repoUser.GetAll();
                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al recuperar datos de la base de datos.");
            }
        }

        [HttpGet("All/:id")]
        public async Task<IActionResult> GetById(Guid Id)
        {
            try
            {
                var result = await _repoUser.GetById(Id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Insert(User user)
        {
            try
            {
                //Check if the user exists 
                var response = await _repoUser.Exists(x => x.FirstName == user.FirstName);
                if (response)
                {
                    return Ok("El usuario ya existe.");
                }
                else
                {
                    return Ok(await _repoUser.Add(user));
                }         
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(User user)
        {
            try
            {
                return Ok(await _repoUser.Update(user));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(Guid Id)
        {
            try
            {
                var response = await _repoUser.Exists(x => x.Id == Id);
                if (!response)
                    return Ok("El usuario no existe");

                return Ok(await _repoUser.Delete(Id));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }


        [HttpGet("Search")]
        public async Task<IActionResult> SearchByName(string Nombre)
        {
            try
            {
                var entity = await _repoUser.FindWhere(x => x.FirstName.Contains(Nombre));
                return Ok(entity);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }
    }
}
