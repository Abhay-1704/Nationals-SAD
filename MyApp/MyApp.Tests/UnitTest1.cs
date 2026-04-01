using NUnit.Framework;
using MyApp;

namespace MyApp.Tests
{
    [TestFixture]
    public class ReservationTests
    {
        [Test]
        public void CanBeCancelledBy_UserIsAdmin_ReturnsTrue()
        {
            var reservation = new Reservation();
            var result = reservation.CanBeCancelledBy(new User { IsAdmin = true });
            Assert.That(result, Is.True);
        }

        [Test]
        public void CanBeCancelledBy_SameUser_ReturnsTrue()
        {
            var user = new User();
            var reservation = new Reservation { MadeBy = user };
            var result = reservation.CanBeCancelledBy(user);
            Assert.That(result, Is.True);
        }

        [Test]
        public void CanBeCancelledBy_AnotherUser_ReturnsFalse()
        {
            var reservation = new Reservation { MadeBy = new User() };
            var result = reservation.CanBeCancelledBy(new User { IsAdmin = false });
            Assert.That(result, Is.False);
        }
    }
}