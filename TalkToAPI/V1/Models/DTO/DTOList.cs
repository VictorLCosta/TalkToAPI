using System.Collections.Generic;

namespace TalkToAPI.V1.Models.DTO
{
    public class DTOList<T> : DTOBase
    {
        public List<T> Results { get; set; }
    }
}