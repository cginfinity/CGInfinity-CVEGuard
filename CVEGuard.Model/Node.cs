namespace CVEGuard.Model
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text.Json.Serialization;

    public class Node
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonIgnore]
        public int Id { get; set; }

        public int ConfigurationId { get; set; }
        public Configuration? Configuration { get; set; }

        [JsonPropertyName("operator")]
        public string? Operator { get; set; }

        [JsonPropertyName("negate")]
        public bool Negate { get; set; }

        [JsonPropertyName("cpeMatch")]
        public List<CpeMatch> CpeMatches { get; set; }=new List<CpeMatch>();
    }


}
