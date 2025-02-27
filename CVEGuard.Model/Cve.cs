namespace CVEGuard.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text.Json.Serialization;

    public class Cve
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonIgnore]
        public int Id { get; set; }

        [JsonPropertyName("id")]
        public string? CveId { get; set; }

        [JsonPropertyName("sourceIdentifier")]
        public string? SourceIdentifier { get; set; }

        [JsonPropertyName("published")]
        public DateTime Published { get; set; }

        [JsonPropertyName("lastModified")]
        public DateTime LastModified { get; set; }

        [JsonPropertyName("vulnStatus")]
        public string? VulnStatus { get; set; }

        [JsonPropertyName("descriptions")]
        public List<Description> Descriptions { get; set; }= new List<Description>();

        [JsonPropertyName("metrics")]
        public Metrics? Metrics { get; set; }

        [JsonPropertyName("weaknesses")]
        public List<Weakness> Weaknesses { get; set; }=new List<Weakness>();

        [JsonPropertyName("configurations")]
        public List<Configuration> Configurations { get; set; }= new List<Configuration>();

        [JsonPropertyName("references")]
        public List<Reference> References { get; set; }= new List<Reference>();
    }


}
