using System;


namespace HotelReservation.Domain.Entities
{
    public class Reservation
    {
        public Guid Id { get; private set; }
        public DateOnly CheckInDate { get; private set; }
        public DateOnly CheckOutDate { get; private set; }

        public Guid CustomerId { get; private set; }
        public Customer Customer { get; private set; }
        public Guid RoomId { get; private set; }
        public Room Room { get; private set; }
        public int NumberOfGuests { get; private set; }

        //yine de pricing service e koy çünkü sezonluk fiyatlandırma, indirimler vs. olabilir
        public decimal TotalPrice { get; private set; }

        public Reservation(DateOnly checkInDate, DateOnly checkOutDate, Guid customerId, Guid roomId, int numberOfGuests, decimal totalPrice)
        {
            if (checkInDate > checkOutDate)
                throw new ArgumentException("Check-out date must be on or after check-in date.");
            if(checkInDate <= DateOnly.FromDateTime(DateTime.UtcNow))
                throw new ArgumentException("Check-in date must be in the future.");
            if (numberOfGuests <= 0)
                throw new ArgumentException("Number of guests must be greater than zero.");
            if(totalPrice <= 0)
                throw new ArgumentException("Total price must be greater than zero.");
            Id = Guid.NewGuid();
            CheckInDate = checkInDate;
            CheckOutDate = checkOutDate;
            CustomerId = customerId;
            RoomId = roomId;
            NumberOfGuests = numberOfGuests;
            TotalPrice = totalPrice;
        }

        public void Update(DateOnly checkInDate, DateOnly checkOutDate, int numberOfGuests, decimal roomPrice)
        {
            if (checkInDate > checkOutDate)
                throw new ArgumentException("Check-out date must be on or after check-in date.");
            if (checkInDate <= DateOnly.FromDateTime(DateTime.UtcNow))
                throw new ArgumentException("Check-in date must be in the future.");
            if (numberOfGuests <= 0)
                throw new ArgumentException("Number of guests must be greater than zero.");
            CheckInDate = checkInDate;
            CheckOutDate = checkOutDate;
            NumberOfGuests = numberOfGuests;
            TotalPrice = roomPrice;
        }
    }
}
