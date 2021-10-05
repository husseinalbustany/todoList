using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace todoList.Models
{
    public class todo
    {
        public int Id { get; set; }


        [StringLength(60, MinimumLength = 3)]
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }



        [Display(Name = "Due Date")]
        [DataType(DataType.Date)]
        [Required]
        public DateTime DueDate { get; set; }
        public bool Done { get; set; }
        public DateTime DoneDate { get; set; }

    }



    
    
    

    
}
