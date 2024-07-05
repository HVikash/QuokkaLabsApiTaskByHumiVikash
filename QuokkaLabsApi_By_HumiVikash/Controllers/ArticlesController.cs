using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuokkaLabsApi_By_HumiVikash.DatabaseContext;
using QuokkaLabsApi_By_HumiVikash.Models;
using QuokkaLabsApi_By_HumiVikash.Models.DTOs;
using QuokkaLabsApi_By_HumiVikash.Models.DTOs.Response;
using QuokkaLabsApi_By_HumiVikash.Repository.IRepository;
using System.Net;

namespace QuokkaLabsApi_By_HumiVikash.Controllers
{
    //token routing
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    { 
        protected readonly Response _response;
        private readonly IArticlesRepository _IArticlesRepo;

        public ArticlesController(IArticlesRepository IArticlesRepo) 
        {
           
            _IArticlesRepo = IArticlesRepo;
            _response = new();
        }

        //GetList of articles
        [HttpGet]
        public ActionResult<List<Articles>> GetAll()
        {
            try
            {

                var data = _IArticlesRepo.GetAll();
                if (data == null)
                {
                    return NotFound();
                }
                return data;
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }

        }

        //Get articles by id
        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin")]
        public  ActionResult<Articles> GetById(int id)
        {
            try
            {

                var data = _IArticlesRepo.GetById(id);
                if (data == null)
                {
                    return NotFound();
                }
                return data;
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }

        }

        //create article
        [HttpPost("Create")]
        [Authorize(Roles = "Admin")]
        public ActionResult AddArticles(ArticlesDto art)
        {
            try
            {
                var data = _IArticlesRepo.Add(art);
                if (data!=null) {
                    return Ok(data);
                    //return CreatedAtAction("api/Articles", new { id = data.Id }, data);
                }
                return StatusCode(409);
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }
        }

        //update article
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public ActionResult<Articles> UpdateArticles(int id,ArticlesDto art)
        {
            try
            {
                var data = _IArticlesRepo.Update(id, art);
                return data;
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }

        }

        //Delete article
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public ActionResult<Articles> DeleteArticles(int id)
        {
            try
            {
                var data = _IArticlesRepo.Delete(id);
                if (data==null)
                {
                    return NotFound();
                }
                return data;
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }
        }
       
    }
}
