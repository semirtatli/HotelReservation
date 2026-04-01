using HotelReservation.Application.DTO;
using HotelReservation.Application.Interfaces;
using HotelReservation.Application.RepositoryInterfaces;
using HotelReservation.Domain.Entities;
using HotelReservation.Domain.Exceptions;
using HotelReservation.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelReservation.Infrastructure.Repositories
{
    public class HotelRepository : IHotelRepository
    {
        private readonly AppDbContext _context;
        public HotelRepository(AppDbContext _context)
        {
            this._context = _context;
        }
        public void AddHotel(Hotel hotel)
        {
            _context.Hotels.Add(hotel);
            _context.SaveChanges();
        }

        public Hotel UpdateHotel(Guid id, Hotel hotel)
        {
            var existingHotel = _context.Hotels.Find(id);
            if (existingHotel is null) throw new NotFoundException("Hotel not found");
            existingHotel.UpdateName(hotel.Name);
            _context.SaveChanges();
            return existingHotel;
        }

        public List<Hotel> GetAllHotels() {
            return _context.Hotels.ToList();
        }

        public Hotel GetHotelById(Guid id)
        {
            var hotel = _context.Hotels.Find(id);
            if (hotel is null) { 
                throw new NotFoundException("Hotel not found");
            }
            return hotel;
        }

        public Hotel DeleteHotel(Guid id)
        {
            var hotel = _context.Hotels.Find(id);
            if (hotel != null)
            {
                
                _context.Hotels.Remove(hotel);
                _context.SaveChanges();
                return hotel;
            }
            throw new NotFoundException("Hotel not found");
        }
    }
}
