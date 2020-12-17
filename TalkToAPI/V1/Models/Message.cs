using System;
using System.ComponentModel.DataAnnotations;

namespace TalkToAPI.V1.Models
{
    public class Message
    {
        public int Id { get; set; }
        public ApplicationUser Sender { get; set; }
        public ApplicationUser To { get; set; }

        [Required]
        public string Text { get; set; }
        public DateTime Created { get; set; }

        //Keys
        public string SenderId { get; set; }
        public string ToId { get; set; }
    }
}