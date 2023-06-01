﻿using System.ComponentModel.DataAnnotations;

namespace MyCar.Web.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Не указан никнейм")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Не указан пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Не указана почта")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароль введен неверно")]
        [Required(ErrorMessage = "Не указан повторно пароль")]
        public string ConfirmPassword { get; set; }
    }
}
