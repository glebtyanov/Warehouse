﻿using DAL.Enum;

namespace DAL.Entities
{
    public class Worker
    {
        public int WorkerId { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Address { get; set; }

        public string? ContactNumber { get; set; }

        public string? Email { get; set; }

        public int PositionId { get; set; } = (int)Enums.Positions.Regular;

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }

        public DateTime HireDate { get; set; }

        // refers one
        public Position? Position { get; set; }

        // refered by many
        public List<Order>? Orders { get; set; }

        public List<Department>? Departments { get; set; }
    }
}
