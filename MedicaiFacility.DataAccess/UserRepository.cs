﻿using MedicaiFacility.BusinessObject;
using MedicaiFacility.DataAccess.IRepostory;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MedicaiFacility.DataAccess
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public User FindById(int id) => _context.Users.FirstOrDefault(x => x.UserId == id);

        public User FindByEmail(string email) => _context.Users.FirstOrDefault(x => x.Email == email);

        public User FindByPhoneNumber(string phoneNumber) => _context.Users.FirstOrDefault(x => x.PhoneNumber == phoneNumber);

        public void Add(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public User ValidateLogin(string identifier, string password)
        {
            var user = FindByEmail(identifier);
            if (user == null)
            {
                user = FindByPhoneNumber(identifier);
            }

            if (user != null && user.Password == password && user.Status == true)
            {
                return user;
            }
            return null;
        }

        public void RegisterUser(User user)
        {
            if (FindByEmail(user.Email) != null)
            {
                throw new Exception("Email already exists!");
            }

            user.Image = null;
            user.CreatedAt = DateTime.Now;
            user.UpdatedAt = DateTime.Now;
            user.Status = true;

            Add(user);
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _context.Users.ToList();
        }

        public void UpdateUser(User user)
        {
            var existingUser = FindById(user.UserId);
            if (existingUser == null)
            {
                throw new Exception("User not found!");
            }

            existingUser.FullName = user.FullName;
            existingUser.Email = user.Email;
            existingUser.PhoneNumber = user.PhoneNumber;
            existingUser.UserType = user.UserType;
            existingUser.BankAccount = user.BankAccount;
            existingUser.Status = user.Status;
            existingUser.UpdatedAt = DateTime.Now;

            _context.SaveChanges();
        }
    }
}