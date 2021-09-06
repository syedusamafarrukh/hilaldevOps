using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Hilal.Common;
using Hilal.Data.Context;
using Hilal.Data.DTOs;
using Hilal.DataViewModel.Request.Admin.v1;
using Hilal.DataViewModel.Response.Admin.v1;
using Hilal.Service.Interface.v1.Admin;
using Hilal.DataViewModel.Common;
using Hilal.DataViewModel.Enum;

namespace Hilal.Service.Implementation.v1.Admin
{
    public class BlogService : IBlogService
    {
        public GetBlogResponse GetBlog(ListGeneralModel pBlog)
        {
            try
            {
                GetBlogResponse response = new GetBlogResponse();

                using (var db = new HilalDbContext())
                {
                    var query = db.Blogs.OrderByDescending(x=> x.CreatedDate)
                        .Where(x => x.IsActive == true)
                        .Select(x => new BlogViewModel
                        {
                            Id = x.Id,
                            Description = x.Description,
                            HeaderImage = x.HeaderImage,
                            Title = x.Title,
                            PostedDate = x.CreatedDate
                        }).AsQueryable();

                    if (!string.IsNullOrEmpty(pBlog.Search))
                    {
                        var date = new DateTime();
                        var sdate = DateTime.TryParse(pBlog.Search, out date);
                        int totalCases = -1;
                        var isNumber = Int32.TryParse(pBlog.Search, out totalCases);

                        query = query.Where(
                        x => x.Title.ToLower().Contains(pBlog.Search.ToLower())
                        || x.Description.ToLower().Contains(pBlog.Search.ToLower())
                    );
                    }

                    response.Page = pBlog.Page;
                    response.PageSize = pBlog.PageSize;
                    response.TotalRecords = query.Count();
                    if (pBlog.PageSize > 0)
                    {
                        response.ItemList = query.Skip(pBlog.Page).Take(pBlog.PageSize).ToList();
                    }
                    else
                    {
                        response.ItemList = query.ToList();
                    }
                }
                return response;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> SaveBlog(CreateBlogRequest BlogRequest, Guid userId)
        {
            try
            {
                bool response = false;

                if (BlogRequest.Id == null)
                {
                    using (var db = new HilalDbContext())
                    {
                        using (var trans = db.Database.BeginTransaction())
                        {
                            try
                            {
                                await db.Blogs.AddAsync(new Blogs
                                {
                                    Id = SystemGlobal.GetId(),
                                    Title = BlogRequest.Title,
                                    Description = BlogRequest.Description,
                                    HeaderImage = BlogRequest.HeaderImage,
                                    IsPublished = true,
                                    IsActive = true,
                                    CreatedBy = userId.ToString(),
                                    CreatedDate = DateTime.UtcNow
                                });

                                await db.SaveChangesAsync();
                                trans.Commit();

                                response = true;
                            }
                            catch (Exception ex)
                            {
                                trans.Rollback();
                                throw ex;
                            }
                        }
                    }
                }
                else
                {
                    using (var db = new HilalDbContext())
                    {
                        using (var trans = db.Database.BeginTransaction())
                        {
                            try
                            {
                                
                                var category = db.Blogs.FirstOrDefault(x => x.Id.Equals(BlogRequest.Id));
                                
                                category.Title = BlogRequest.Title;
                                category.Description = BlogRequest.Description;
                                category.HeaderImage = BlogRequest.HeaderImage;
                                category.IsActive = true;
                                category.UpdateBy = userId.ToString();
                                category.UpdatedDate = DateTime.UtcNow;

                                db.Entry(category).State = EntityState.Modified;
                                await db.SaveChangesAsync();
                                trans.Commit();

                                response = true;
                            }
                            catch (Exception ex)
                            {
                                trans.Rollback();
                                throw ex;
                            }
                        }
                    }
                }

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public CreateBlogRequest GetEditBlog(Guid id, Guid userId)
        {
            try
            {
                using (var db = new HilalDbContext())
                {
                    return db.Blogs
                        .Where(x => x.Id.Equals(id))
                        .Select(x => new CreateBlogRequest
                        {
                            Id = x.Id,
                            Title = x.Title,
                            HeaderImage = x.HeaderImage,
                            Description = x.Description
                        })
                        .FirstOrDefault() ?? new CreateBlogRequest();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> ControllBlogActivation(Guid BlogId, bool activation, Guid userId)
        {
            try
            {
                bool response = false;

                using (var db = new HilalDbContext())
                {
                    var category = db.Blogs.FirstOrDefault(x => x.Id.Equals(BlogId));

                    if (category == null) throw new Exception("BlogId Doesn't Exists");

                    category.IsActive = activation;
                    category.UpdateBy = userId.ToString();
                    category.UpdatedDate = DateTime.UtcNow;

                    db.Entry(category).State = EntityState.Modified;
                    await db.SaveChangesAsync();

                    response = true;
                }

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> ControllBlogPublish(Guid BlogId, bool activation, Guid userId)
        {
            try
            {
                bool response = false;

                using (var db = new HilalDbContext())
                {
                    var category = db.Blogs.FirstOrDefault(x => x.Id.Equals(BlogId));

                    if (category == null) throw new Exception("BlogId Doesn't Exists");

                    category.IsPublished = activation;
                    category.UpdateBy = userId.ToString();
                    category.UpdatedDate = DateTime.UtcNow;

                    db.Entry(category).State = EntityState.Modified;
                    await db.SaveChangesAsync();

                    response = true;
                }

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<BlogViewModel> GetBlogList(Guid? BlogId, int LanguageId)
        {
            try
            {
                List<BlogViewModel> response = new List<BlogViewModel>();

                using (var db = new HilalDbContext())
                {
                    var query = db.Blogs
                        .Where(x => x.IsActive == true && x.IsPublished == true)
                        .Select(x => new BlogViewModel
                        {
                            Id = x.Id,
                            Title = x.Title,
                            Description = x.Description,
                            HeaderImage = x.HeaderImage,
                            PostedDate = x.CreatedDate
                        }).AsQueryable();

                    response = query.OrderByDescending(x => x.PostedDate).ToList();
                }
                return response;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
