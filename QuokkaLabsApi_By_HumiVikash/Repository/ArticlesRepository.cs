using Azure;
using Microsoft.AspNetCore.Http.HttpResults;
using QuokkaLabsApi_By_HumiVikash.DatabaseContext;
using QuokkaLabsApi_By_HumiVikash.Models;
using QuokkaLabsApi_By_HumiVikash.Models.DTOs;
using QuokkaLabsApi_By_HumiVikash.Models.DTOs.Response;
using QuokkaLabsApi_By_HumiVikash.Repository.IRepository;
using System.Net;

namespace QuokkaLabsApi_By_HumiVikash.Repository
{
    public class ArticlesRepository : IArticlesRepository
    {
        private readonly ApplicationDBContext _db;
        //protected readonly Response _response;

        public ArticlesRepository(ApplicationDBContext db)
        {
            _db = db;
            //_response = new();
        }
        public Articles GetById(int id)
        {
            try
            {
                var data= _db.Articles.Find(id);
                return data;
            }
            catch (Exception e)
            {
                return null;
            }
           
        }
        public List<Articles> GetAll()
        {
            try
            {
                var data = _db.Articles.ToList();
                return data;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public Articles Add(object obj)
        {
            try
            {
                var data = obj as ArticlesDto;
                var isExists = _db.Articles.FirstOrDefault(x => x.ArticalName == data.ArticalName);
                if (isExists == null)
                {
                    var artObj = new Articles()
                    {
                        ArticalName = data.ArticalName,
                        ArticalDescription = data.ArticalDescription,
                        //CreatedOn = DateTime.Now

                    };
                    var result=_db.Articles.Add(artObj);
                    Save();
                    return result.Entity;
                }
                return null;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public Articles Delete(int obj)
        {
          
            try
            {
                var isExists = _db.Articles.Find(obj);

                if (isExists != null)
                {
                    _db.Articles.Remove(isExists);
                    Save();
                    return isExists;
                }
                return null;
            }
            catch (Exception exp)
            {
                return null;
            }
        }

   

        public void Save()
        {
            _db.SaveChanges();
        }

        public Articles Update(int id, object obj)
        {
            var data = obj as ArticlesDto;
            try
            {
                var isExists = _db.Articles.Find(id);

                if (isExists != null)
                {
                    var alreadyExists = _db.Articles.Any(x => x.ArticalName == data.ArticalName);

                    if (!alreadyExists)
                    {
                        isExists.ArticalName=data.ArticalName;
                        isExists.ArticalDescription = data.ArticalDescription;
                        //isExists.UpdatedOn = DateTime.Now;
                      
                        Save();
                        return isExists;
                    }
                    
                }
                return null;
            }
            catch (Exception exp)
            {
                return null;
            }
        }
    }
}
