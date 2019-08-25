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
        [Display(Name = "ID")]
        [Required]
        [StringLength(UserEntity.Id_MAX_LENGTH)]
        [RegularExpression(@"[-_a-zA-Z][-_a-zA-Z0-9]*", ErrorMessage = "Måste vara enkelt ID, tänk kod identifier")]
        public string UserId { get; set; }

        [Display(Name = "Ditt namn")]
        [Required]
        [StringLength(UserEntity.Name_MAX_LENGTH)]
        public string UserName { get; set; }

        [Display(Name = "Rums ID")]
        [Required]
        [StringLength(RoomEntity.Id_MAX_LENGTH)]
        public string RoomId { get; set; }
    }
}
