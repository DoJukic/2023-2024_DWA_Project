using DBScaffold.Models;
using Microsoft.EntityFrameworkCore;

namespace MVC_Module.Systems
{
    public static class BookReservationSystem
    {
        static DwaContext context = new DwaContext();

        static BookReservationSystem()
        {
            context.Database.SetConnectionString("name=ConnectionStrings:DB");
        }
    }
}
