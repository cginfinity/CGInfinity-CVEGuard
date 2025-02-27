namespace CVEGuard.Model
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text.Json.Serialization;

    public class CpeMatch
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonIgnore]
        public int Id { get; set; }

        public int NodeId { get; set; }
        public Node? Node { get; set; }

        [JsonPropertyName("vulnerable")]
        public bool Vulnerable { get; set; }

        [JsonPropertyName("criteria")]
        public string? Criteria { get; set; }
    }


}
