using System.Collections.Generic;

namespace TalkToAPI.V1.Models.DTO
{
    public class DTOBase
    {
        public List<DTOLink> Links { get; set; } = new List<DTOLink>();
    }
}