using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.SwaggerGen;
using Hilal.Common;
using Hilal.DataViewModel.Common;
using Hilal.Service.Interface.v1.Admin;
using Hilal.Service.Implementation.v1.Admin;
using Hilal.Service.Implementation.v1.App;
using Hilal.Service.Interface.v1.App;
using Microsoft.Extensions.FileProviders;

namespace Hilal.Api
{
    public class Startup
    {
        private readonly IWebHostEnvironment _env;
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public IConfiguration configuration { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            this.configuration = configuration;
            _env = environment;
        }



        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddApiVersioning(o =>
            {
                o.ReportApiVersions = true;
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.DefaultApiVersion = new ApiVersion(2, 0);
            });


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Swagger  Documentation", Version = "v1" });
                c.SwaggerDoc("Admin-v1", new OpenApiInfo { Title = "Swagger Admin Documentation", Version = "v1" });

                c.OperationFilter<RemoveVersionFromParameter>();
                c.DocumentFilter<ReplaceVersionWithExactValueInPath>();

                // Ensure the routes are added to the right Swagger doc
                c.DocInclusionPredicate((docName, apiDesc) =>
                {
                    if (!apiDesc.TryGetMethodInfo(out MethodInfo methodInfo))
                    {
                        return false;
                    }

                    if (methodInfo.DeclaringType.FullName.Contains("Admin"))
                    {
                        IEnumerable<ApiVersion> versions = methodInfo.DeclaringType
                        .GetCustomAttributes(true)
                        .OfType<ApiVersionAttribute>()
                        .SelectMany(a => a.Versions);

                        return versions.Any(v => $"Admin-v{v.ToString()}" == docName);
                    }
                    else
                    {
                        IEnumerable<ApiVersion> versions = methodInfo.DeclaringType
                        .GetCustomAttributes(true)
                        .OfType<ApiVersionAttribute>()
                        .SelectMany(a => a.Versions);

                        return versions.Any(v => $"v{v.ToString()}" == docName);
                    }
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                    new OpenApiSecurityScheme
                    {
                  Reference = new OpenApiReference
                    {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                    }
                    },
                 new string[] { }
                }
                });
                c.IncludeXmlComments(Path.Combine(
                   Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), $"{this.GetType().Assembly.GetName().Name}.xml"
               ));
                c.OperationFilter<AddRequiredHeaderParameter>();
                c.CustomSchemaIds(x => x.FullName);
            });


            services.ConfigureApplicationCookie(options =>
            {
                options.Events.OnRedirectToLogin = async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    await context.Response.WriteAsync("Some custom error message if required");
                };
            });

            services.AddAuthentication(sharedOptions =>
            {
                sharedOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                sharedOptions.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                sharedOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
               .AddJwtBearer(options =>
               {
                   var keyByteArray = Encoding.ASCII.GetBytes(this.configuration.GetValue<String>("Tokens:Key"));
                   var signinKey = new SymmetricSecurityKey(keyByteArray);

                   options.TokenValidationParameters = new TokenValidationParameters
                   {
                       IssuerSigningKey = signinKey,
                       ValidAudience = "Audience",
                       ValidIssuer = "Issuer",
                       ValidateIssuerSigningKey = true,
                       ValidateLifetime = true,
                       ClockSkew = TimeSpan.FromMinutes(0)
                   };
               });

            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:4200", "http://localhost:4000", "https://hilalmarket.etgop.com", "https://hilalmarketclient", "https://hilalmarketclient.douxl.work", "https://alhilal.douxl.dev", "https://client.alhalalmarket.com", "https://admin.alhalalmarket.com", "https://alhalalmarket.com")
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials();
                    });
            });

            services.AddSingleton<IFileProvider>(
            new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));

            #region Admin
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IAgeService, AgeService>();
            services.AddScoped<IApprovelSettingsService, ApprovelSettingsService>();
            services.AddScoped<IBreedService, BreedService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICitiesService, CitiesService>();
            services.AddScoped<ICommissionService, CommissionService>();
            services.AddScoped<ICountriesService, CountriesService>();
            services.AddScoped<IDashboardSliderService, DashboardSliderService>();
            services.AddScoped<IGenderService, GenderService>();
            services.AddScoped<ISubscriptionService, SubscriptionService>();
            services.AddScoped<IFeaturedAdvertisementService, FeaturedAdvertisementService>();
            services.AddScoped<IBlogService, BlogService>();

            #endregion
            #region APP
            services.AddScoped<IAppAccountService, AppAccountService>();
            services.AddScoped<IBusinessProfileService, BusinessProfileService>();
            services.AddScoped<IAdvertisementService, AdvertisementService>();
            services.AddScoped<IChatStreamService, ChatStreamService>();
            #endregion

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(MyAllowSpecificOrigins);

            app.UseHttpsRedirection();

            app.UseExceptionHandler(appBuilder =>
            {
                appBuilder.Use(async (context, next) =>
                {
                    var error = context.Features[typeof(IExceptionHandlerFeature)] as IExceptionHandlerFeature;

                    if (error != null && error.Error != null)
                    {
                        context.Response.StatusCode = 500;
                        context.Response.ContentType = "application/json";

                        var path = context.Request.Path.Value;
                        var controller = path.Split('/')[2];
                        var action = path.Split('/')[3] ?? "";

                        var userId = context?.GetRouteData()?.Values["userId"]?.ToString();

                        MakeLog Err = new MakeLog();
                        Err.ErrorLog(_env.WebRootPath, "/" + controller + "/" + action + ".txt", "UserId: " + userId + " Error: " + error.Error?.Message ?? "" + "Inner Exception => " + error.Error.InnerException?.Message ?? "");

                        await context.Response.WriteAsync(JsonConvert.SerializeObject(new Response<bool>
                        {
                            IsError = true,
                            Message = Error.ServerError,
                            Exception = "UserId: " + userId + " Error: " + error.Error?.Message ?? "" + "Inner Exception => " + error.Error.InnerException?.Message ?? "",
                            Data = false
                        }));
                    }
                    //when no error, do next.

                    else await next();
                });
            });

            app.UseStaticFiles(new StaticFileOptions()
            {
                OnPrepareResponse = ctx =>
                {
                    ctx.Context.Response.Headers.Append("Access-Control-Allow-Origin", "*");
                    ctx.Context.Response.Headers.Append("Access-Control-Allow-Headers",
                        "Origin, X-Requested-With, Content-Type, Accept");
                },

            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Swagger Documentation V1");
                c.SwaggerEndpoint("/swagger/Admin-v1/swagger.json", "Swagger Admin Documentation V1");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        public class RemoveVersionFromParameter : IOperationFilter
        {
            public void Apply(OpenApiOperation operation, OperationFilterContext context)
            {
                var versionParameter = operation.Parameters.Single(p => p.Name == "version");
                operation.Parameters.Remove(versionParameter);
            }
        }

        public class ReplaceVersionWithExactValueInPath : IDocumentFilter
        {
            public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
            {
                var paths = new OpenApiPaths();
                foreach (var path in swaggerDoc.Paths)
                {
                    paths.Add(path.Key.Replace("v{version}", swaggerDoc.Info.Version), path.Value);
                }
                swaggerDoc.Paths = paths;
            }
        }

        private class AddRequiredHeaderParameter : IOperationFilter
        {
            public void Apply(OpenApiOperation operation, OperationFilterContext context)
            {
                if (operation.Parameters == null)
                    operation.Parameters = new List<OpenApiParameter>();

                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = "language",
                    In = ParameterLocation.Header,
                    Description = "this is Language Header",
                    Required = true
                });
            }
        }
    }
}
