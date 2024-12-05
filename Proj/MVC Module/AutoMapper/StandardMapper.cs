using AutoMapper;
using DBScaffold.Models;
using MVC_Module.ViewModels;

namespace MVC_Module.AutoMapper
{
    public static class StdMapper
    {
        private static readonly Mapper mapper;
        static StdMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<RegisterVM, User>();
                cfg.CreateMap<RegisterVM, Login>()
                .AfterMap((register, login) =>
                    {
                        login.PasswordSalt = FIS_API.Security.PasswordHashProvider.GetSalt();
                        login.PasswordHash = FIS_API.Security.PasswordHashProvider.GetHash(register.Password, login.PasswordSalt);
                    });
                cfg.CreateMap<SecLoginCreateVM, Login>()
                .AfterMap((loginVM, login) =>
                {
                    login.PasswordSalt = FIS_API.Security.PasswordHashProvider.GetSalt();
                    login.PasswordHash = FIS_API.Security.PasswordHashProvider.GetHash(loginVM.Password, login.PasswordSalt);
                });
            });

            mapper = new Mapper(config);
        }

        public static TDestination Map<TDestination>(object source)
        {
            return mapper.Map<TDestination>(source);
        }
    }
}
