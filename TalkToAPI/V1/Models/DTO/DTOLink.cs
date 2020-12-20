namespace TalkToAPI.V1.Models.DTO
{
    public class DTOLink
    {
        public string Rel { get; set; }
        public string Href { get; set; }
        public string Method { get; set; }

        public DTOLink(string rel, string href, string method)
        {
            Rel = rel;
            Href = href;
            Method = method;
        }
    }
}