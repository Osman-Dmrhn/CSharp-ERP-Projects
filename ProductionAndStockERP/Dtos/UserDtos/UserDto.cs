﻿namespace ProductionAndStockERP.Dtos.UserDtos
{
    public class UserDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }

        public bool IsActive { get; set; }
    }
}
