using System;
using System.Collections.Generic;
using System.Text;

namespace MyApp
{
    public class User
    {
        public bool IsAdmin { get; set; }
    }

    public class Reservation
    {
        public User MadeBy { get; set; }

        public bool CanBeCancelledBy(User user)
        {
            return user.IsAdmin || user == MadeBy;
        }
    }
}
