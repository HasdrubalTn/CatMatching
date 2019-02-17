using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace MS.CatMatching.Services
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        /// This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connection = Configuration["ConnectionStrings:DefaultConnection"];

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            //Configure Azure AD
            //services.AddAuthentication(AzureADDefaults.JwtBearerAuthenticationScheme)
            //.AddAzureADBearer(options => Configuration.Bind("AzureAd", options));
            //services.AddMvc(config =>
            //{
            //    var policy = new AuthorizationPolicyBuilder()
            //    .RequireAuthenticatedUser()
            //    .Build();
            //    config.Filters.Add(new AuthorizeFilter(policy));
            //}).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            //Configure Swagger
            services.AddSwaggerGen(sw =>
            {
                var url = "https://login.microsoftonline.com";

                sw.SwaggerDoc("v1", new Info
                {
                    Contact = new Contact
                    {
                        Email = "mselmi@outlook.com",
                        Name = "Ali MSELMI",
                        Url = "https://www.linkedin.com/in/mselmi/"
                    },
                    Description = "Cat Matching API V1",
                    License = new License { Name = "MIT", Url = "" },
                    Title = "Cat Matching API",
                    Version = "V1.0",
                    TermsOfService = ""
                });
                sw.AddSecurityDefinition("oauth2", new OAuth2Scheme
                {
                    Type = "oauth2",
                    Flow = "implicit",
                    AuthorizationUrl = $"{url}/{Configuration["AzureAd:TenantId"]}/oauth2/authorize",
                    Scopes = new Dictionary<string, string>
                    {
                        { "https://mselmioutlook.onmicrosoft.com/94fb9c85-05ae-4f2c-b34c-6d906c12e057/user_impersonation", "Access API" }
                    }
                });
                sw.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                {
                    { "oauth2", new[]
                        {
                            "https://mselmioutlook.onmicrosoft.com/94fb9c85-05ae-4f2c-b34c-6d906c12e057/user_impersonation"
                        }
                    }
                });
                var xmlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"MS.CatMatching.Services.xml");
                sw.IncludeXmlComments(xmlPath);
                sw.UseReferencedDefinitionsForEnums();
            });

            //Configure DI
            services
                .AddLogging()
                .AddScoped<IServiceProvider, ServiceProvider>()
                .BuildServiceProvider();

        }

        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseCors(builder => {
                builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().AllowCredentials();
            });

            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(sw =>
            {
                sw.SwaggerEndpoint("/swagger/v1/swagger.json", "Cat Matching API");
                sw.DefaultModelExpandDepth(2);
                sw.DefaultModelRendering(ModelRendering.Model);
                sw.DefaultModelsExpandDepth(-1);
                sw.DisplayOperationId();
                sw.DisplayRequestDuration();
                sw.DocExpansion(DocExpansion.None);
                sw.EnableDeepLinking();
                sw.EnableFilter();
                sw.MaxDisplayedTags(5);
                sw.ShowExtensions();
                sw.EnableValidator();
                sw.SupportedSubmitMethods(SubmitMethod.Get, SubmitMethod.Head);

                //Swagger AD AOuth configuration
                sw.OAuthClientId(Configuration["Swagger:ClientId"]);
                sw.OAuthClientSecret(Configuration["Swagger:ClientSecret"]);
                sw.OAuthRealm(Configuration["AzureAD:ClientId"]);
                sw.OAuthAppName("Cat Matching V1");
                sw.OAuthScopeSeparator("");
                sw.OAuth2RedirectUrl($"{Configuration["Api:BaseUrl"]}/swagger/oauth2-redirect.html");
                sw.OAuthAdditionalQueryStringParams(new Dictionary<string, string>
                {
                    { "resource",Configuration["AzureAD:ClientId"] }
                });
            });
        }
    }
}
