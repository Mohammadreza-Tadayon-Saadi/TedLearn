﻿namespace Services.DTOs.Account;

public class RegisterDto
{
    public string UserName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string RePassword { get; set; } = string.Empty;
}
