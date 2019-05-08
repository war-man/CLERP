﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CLERP.API.Infrastructure.Security.Hashing;
using CLERP.API.Infrastructure.Contexts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MediatR;
using System.Reflection;
using AutoMapper;
using CLERP.API.Infrastructure.Conventions;
using Newtonsoft.Json;
using FluentValidation.AspNetCore;
using CLERP.API.Infrastructure.Exceptions;
using CLERP.API.Infrastructure.Behavior;
using CLERP.API.Infrastructure.Utilities;
using Microsoft.AspNetCore.Http;
using CLERP.API.Infrastructure.Middleware;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using CLERP.API.Infrastructure.Security.Tokens;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace CLERP.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            var currentAssembly = GetType().Assembly;

            // load custom settings
            var settingsSection = Configuration.GetSection(nameof(AppSettings));
            var settings = settingsSection.Get<AppSettings>();

            var jwtSection = Configuration.GetSection(nameof(JwtOptions));
            var jwtOptions = jwtSection.Get<JwtOptions>();

            services.Configure<AppSettings>(settingsSection);
            services.Configure<JwtOptions>(jwtSection);

            services.AddDbContext<ClerpContext>(options => options.UseSqlServer(settings.ConnectionStringLocal));

            var signInConfigurations = new SignInConfigurations();
            services.AddSingleton(signInConfigurations);

            // configure jwt auth
            services.AddAuthentication(auth => 
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(jwtBearerOptions =>
            {
                jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = signInConfigurations.Key,
                    ClockSkew = TimeSpan.Zero,
                    RequireSignedTokens = true
                };

                jwtBearerOptions.RequireHttpsMetadata = true;
            });

            services.AddMvc(options =>
            {
                options.Conventions.Add(new GroupByApiRootConvention());

                // add auth policy globally so every reqest has be authenticated, unless the controller is decorated with the "AllowAnonymous" attribute
                var authPolicy = new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    .Build();
                options.Filters.Add(new AuthorizeFilter(authPolicy));
            })
            .AddJsonOptions(options =>
            {
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            })
            .AddFluentValidation(config =>
            {
                config.RegisterValidatorsFromAssemblyContaining<Startup>();
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddApiVersioning();

            services.AddCors();

            // configure swagger doc
            services.AddSwaggerDocument(config => 
            {
                config.PostProcess = document =>
                {
                    document.Info.Version = "v1";
                    document.Info.Title = "CLERP API";
                    document.Info.Description = "REST Web-Api-Backend for the CLERP application";
                };
            });

            // Register MediatR and custom behavior for pipeline
            services.AddMediatR(currentAssembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(DBContextTransactionPipelineBehavior<,>));

            services.AddAutoMapper(currentAssembly);

            services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; } ); // Disable built in validation error response

            services.AddScoped<IPasswordHasher, PasswordHasher>(); // Register hashing implentation for password hashing within the application
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            services.AddScoped<ICurrentUserAccessor, CurrentUserAccessor>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
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

            app.UseCustomMiddleware();

            app.UseCors(c => c
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod());

            app.UseHttpsRedirection();
            app.UseMvc();

            app.UseAuthentication();

            // Register the Swagger generator and the Swagger UI middlewares
            app.UseSwagger();
            app.UseSwaggerUi3();
        }
    }
}