using App.Data;
using App.Models;

namespace App.Repositories
{
    public class UserRepository : Repository<User, DataContext>
    {
        public UserRepository(DataContext context) : base(context)
        {
        }

        // We can add new methods specific to the user repository here in the future
    }
}
