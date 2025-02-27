namespace CVEGuard.Model
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text.Json.Serialization;

    public class CvssData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonIgnore]
        public int Id { get; set; }

        [JsonPropertyName("version")]
        public string? Version { get; set; }

        [JsonPropertyName("vectorString")]
        public string? VectorString { get; set; }

        [JsonPropertyName("baseScore")]
        public double BaseScore { get; set; }

        [JsonPropertyName("accessVector")]
        public string? AccessVector { get; set; }

        [JsonPropertyName("accessComplexity")]
        public string? AccessComplexity { get; set; }

        [JsonPropertyName("authentication")]
        public string? Authentication { get; set; }

        [JsonPropertyName("confidentialityImpact")]
        public string? ConfidentialityImpact { get; set; }

        [JsonPropertyName("integrityImpact")]
        public string? IntegrityImpact { get; set; }

        [JsonPropertyName("availabilityImpact")]
        public string? AvailabilityImpact { get; set; }
    }


}
