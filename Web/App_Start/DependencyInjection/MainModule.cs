﻿using Ninject.Modules;
using System;
using System.Reflection;
using System.Web.Hosting;
using System.Web.Mvc;
using Template.Components.Logging;
using Template.Components.Mvc;
using Template.Components.Security;
using Template.Controllers;
using Template.Data.Core;
using Template.Services;

namespace Template.Web.DependencyInjection
{
    public class MainModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ILogger>().To<Logger>();
            Bind<AContext>().To<Context>();
            Bind<IUnitOfWork>().To<UnitOfWork>();
            Bind<IEntityLogger>().To<EntityLogger>();

            Bind<IExceptionFilter>().To<ExceptionFilter>();

            Bind<IMvcSiteMapParser>().To<MvcSiteMapParser>();
            Bind<IRoleProvider>().ToConstant(CreateRoleProvider());
            String siteMapPath = HostingEnvironment.MapPath("~/Mvc.sitemap");
            Bind<IMvcSiteMapProvider>().To<MvcSiteMapProvider>().WithConstructorArgument("siteMapPath", siteMapPath);

            Bind<IAuthService>().To<AuthService>();
            Bind<IHomeService>().To<HomeService>();
            Bind<IRolesService>().To<RolesService>();
            Bind<IProfileService>().To<ProfileService>();
            Bind<IAccountsService>().To<AccountsService>();
        }

        private IRoleProvider CreateRoleProvider()
        {
            Assembly controllersAssembly = typeof(BaseController).Assembly;
            IUnitOfWork unitOfWork = Kernel.GetService(typeof(IUnitOfWork)) as IUnitOfWork;

            return new RoleProvider(controllersAssembly, unitOfWork);
        }
    }
}