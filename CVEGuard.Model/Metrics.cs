namespace CVEGuard.Model
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text.Json.Serialization;

    public class Metrics
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonIgnore]
        public int Id { get; set; }

        [JsonPropertyName("cvssMetricV2")]
        public List<CvssMetricV2> CvssMetricsV2 { get; set; }=new List<CvssMetricV2>();
    }


}
