using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        public bool Removed { get; set; }
        public DateTime Updated { get; set; }

        //Keys
        public string SenderId { get; set; }

        [ForeignKey(nameof(To))]
        public string ToId { get; set; }
    }
}