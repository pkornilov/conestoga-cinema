//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace conestogacinema_project.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class room
    {
        public room()
        {
            this.showtimes = new HashSet<showtime>();
        }
    
        public int room_id { get; set; }
        public string room_title { get; set; }
        public int room_seats { get; set; }
        public string room_projector { get; set; }
        public int type_id { get; set; }
    
        public virtual room_types room_types { get; set; }
        public virtual ICollection<showtime> showtimes { get; set; }
    }
}