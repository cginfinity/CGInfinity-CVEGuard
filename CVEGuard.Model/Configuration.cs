namespace CVEGuard.Model
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text.Json.Serialization;

    public class Configuration
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonIgnore]
        public int Id { get; set; }

        public int CveId { get; set; }
        public Cve? Cve { get; set; }

        [JsonPropertyName("nodes")]
        public List<Node> Nodes { get; set; }=new List<Node>();
    }


}
