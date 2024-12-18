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

                cfg.CreateMap<Book, UserBookSearchDataVM>()
                .AfterMap((book, bookVM) =>
                {
                    bookVM.Genre = (book.Genre == null ? "None" : book.Genre.Name);
                    bookVM.Availability = book.BookLocationLinks.Any(x => x.Total > x.UserBorrowingReservations.Count) ? "Currently Available" : "Currently Unavailable";
                });

                cfg.CreateMap<Book, UserBookReserveVM>()
                .AfterMap((book, bookVM) =>
                {
                    bookVM.Genre = (book.Genre == null ? "None" : book.Genre.Name);
                    bookVM.Availability = book.BookLocationLinks.Any(x => x.Total > x.UserBorrowingReservations.Count) ? "Currently Available" : "Currently Unavailable";

                    var locations = new List<int>();
                    foreach (var item in book.BookLocationLinks)
                        locations.Add(item.Location.Idlocation);
                    bookVM.LocationIDs = locations;
                });

                cfg.CreateMap<UserBorrowingReservation, UserBookIndexVM>().
                AfterMap((reservation, viewmodel) =>
                {
                    var bll = reservation.Bllink;

                    viewmodel.Location = bll.Location.Name;

                    viewmodel.Name = bll.Book.Name;
                    viewmodel.Description = bll.Book.Description;
                    viewmodel.Genre = bll.Book.Genre?.Name ?? "None";
                });

                cfg.CreateMap<Login, UserProfileVM>();
                cfg.CreateMap<User, UserProfileVM>().
                AfterMap((user, viewmodel) =>
                {
                    viewmodel.FirstName = user.Name;
                    viewmodel.LastName = user.Surname;
                });

                cfg.CreateMap<UserProfileVM, Login>();
                cfg.CreateMap<UserProfileVM, User>().
                AfterMap((viewmodel, user) =>
                {
                    user.Name = viewmodel.FirstName;
                    user.Surname = viewmodel.LastName;
                });
            });

            mapper = new Mapper(config);
        }

        public static TDestination Map<TDestination>(object source)
        {
            return mapper.Map<TDestination>(source);
        }
        public static void Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            mapper.Map<TSource, TDestination>(source, destination);
        }
    }
}
