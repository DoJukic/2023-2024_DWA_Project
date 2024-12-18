using System;
using DBScaffold.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace MVC_Module.Systems
{
    public static class BookReservationSystem
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        private static DwaContext context;
        private static int daysExpiration;
#pragma warning restore CS8618

        private static object lockObj = new();

        public static void Configure(string connectionString, int daysExpiration)
        {
            lock (lockObj)
            {
                var optionsBuilder = new DbContextOptionsBuilder<DwaContext>();
                optionsBuilder.UseSqlServer(connectionString);

                context = new(optionsBuilder.Options);

                BookReservationSystem.daysExpiration = daysExpiration;
            }
        }

        public static bool? CheckCurrentBookAvailability(int bookLocationLinkID)
        {
            lock (lockObj)
            {
                var targetBLL = context.BookLocationLinks.FirstOrDefault(x => x.Idbllink == bookLocationLinkID);
                if (targetBLL == null)
                    return null;

                IEnumerable<UserBorrowingReservation> relevantReservations =
                    context.UserBorrowingReservations.Where(x => (x.DateReturned == null && x.BllinkId == targetBLL.Idbllink));

                if (relevantReservations.Count() >= targetBLL.Total)
                    return false;

                return true;
            }
        }

        public static bool? TryReserveBook(int bookLocationLinkID, int userID)
        {
            lock (lockObj)
            {
                var targetBLL = context.BookLocationLinks.FirstOrDefault(x => x.Idbllink == bookLocationLinkID);
                if (targetBLL == null)
                    return null;

                var relevantReservations = context.UserBorrowingReservations.Where(x => (x.DateReturned == null && x.BllinkId == targetBLL.Idbllink)).ToList();

                if (relevantReservations.Count() >= targetBLL.Total)
                    return false;

                context.UserBorrowingReservations.Add(new()
                {
                    BllinkId = bookLocationLinkID,
                    UserId = userID,
                    DateReserved = DateTimeOffset.Now,
                    DateExpiration = DateTimeOffset.Now.AddDays(daysExpiration)
                });

                context.SaveChanges();

                return true;
            }
        }
    }
}
