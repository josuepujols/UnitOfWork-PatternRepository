using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Test.Core.Repositories;
using Test.Models;
using Test.DTO;
using System.Linq.Expressions;

namespace Test.Controllers
{
    [Route("api/User")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private UnitOfWork _unitOfWork;
        private GenericRepository<User> _repoUser;

        public UserController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _repoUser = _unitOfWork.Repository<User>();
        }

        [HttpGet("All")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _repoUser.GetList(
                    include: x => x.Gender
                );
                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,e );
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
        public async Task<IActionResult> Insert(UserPostDTO user)
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
                    User NewUser = new()
                    {
                        Id = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        GenderId = user.GenderId,
                        Gender = null
                    };
                    return Ok(await _repoUser.Add(NewUser));
                }         
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(UserPostDTO user)
        {
            try
            {
                User userToUpdate = new()
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    GenderId = user.GenderId,
                    Gender = null
                };
                return Ok(await _repoUser.Update(userToUpdate));
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
                var entity = await _repoUser.FindWhere(
                    predicate: x => x.FirstName.Contains(Nombre),
                    include: x => x.Gender
                    );
                return Ok(entity);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }
    }
}
