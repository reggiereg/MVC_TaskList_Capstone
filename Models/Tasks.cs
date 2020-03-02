using System;
using System.Collections.Generic;

namespace CapStone_TaskList_MVC.Models
{
    public partial class Tasks
    {
        public string TaskDescription { get; set; }
        public DateTime DueDate { get; set; }
        public bool Completion { get; set; }
        public string TaskOwnerId { get; set; }
        public int Id { get; set; }

        public virtual AspNetUsers TaskOwner { get; set; }
    }
}
