using Kosystem.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Kosystem.Web.ViewModels
{
    public class LoginViewModel
    {
        [Display(Name = "Ditt namn")]
        [Required]
        [StringLength(UserEntity.Name_MAX_LENGTH)]
        public string UserName { get; set; }

        [Display(Name = "Rums ID")]
        [Required]
        public Guid RoomId { get; set; }
    }
}
