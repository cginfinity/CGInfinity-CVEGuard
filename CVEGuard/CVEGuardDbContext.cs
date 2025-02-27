using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVEGuard.Model
{
    public class CVEGuardDbContext : DbContext
    {
        public CVEGuardDbContext(DbContextOptions<CVEGuardDbContext> options) : base(options) { }
        public DbSet<VulnerabilityResponse> VulnerabilityResponses { get; set; }
        public DbSet<Vulnerability> Vulnerabilities { get; set; }
        public DbSet<Cve> Cves { get; set; }
        public DbSet<Description> Descriptions { get; set; }
        public DbSet<CvssMetricV2> CvssMetricsV2 { get; set; }
        public DbSet<CvssData> CvssData { get; set; }
        public DbSet<Weakness> Weaknesses { get; set; }
        public DbSet<Configuration> Configurations { get; set; }
        public DbSet<Node> Nodes { get; set; }
        public DbSet<CpeMatch> CpeMatches { get; set; }
        public DbSet<Reference> References { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // VulnerabilityResponse - One to Many with Vulnerabilities
            modelBuilder.Entity<VulnerabilityResponse>()
                .HasMany(vr => vr.Vulnerabilities)
                .WithOne()
                .HasForeignKey("VulnerabilityResponseId") // Shadow Foreign Key
                .OnDelete(DeleteBehavior.Cascade);

            // Vulnerability - One to One with Cve
            modelBuilder.Entity<Vulnerability>()
                .HasOne(v => v.Cve)
                .WithOne()
                .HasForeignKey<Vulnerability>("CveId")
                .OnDelete(DeleteBehavior.Cascade);

            // Cve - One to Many with Descriptions
            modelBuilder.Entity<Cve>()
                .HasMany(c => c.Descriptions)
                .WithOne(d => d.Cve)
                .HasForeignKey(d => d.CveId)
                .OnDelete(DeleteBehavior.Cascade);

            // Cve - One to One with Metrics
            modelBuilder.Entity<Cve>()
                .HasOne(c => c.Metrics)
                .WithOne()
                .HasForeignKey<Metrics>("CveId")
                .OnDelete(DeleteBehavior.Cascade);

            // Metrics - One to Many with CvssMetricV2
            modelBuilder.Entity<Metrics>()
                .HasMany(m => m.CvssMetricsV2)
                .WithOne(cm => cm.Metrics)
                .HasForeignKey(cm => cm.MetricsId)
                .OnDelete(DeleteBehavior.Cascade);

            // CvssMetricV2 - One to One with CvssData
            modelBuilder.Entity<CvssMetricV2>()
                .HasOne(cm => cm.CvssData)
                .WithOne()
                .HasForeignKey<CvssData>("CvssMetricV2Id")
                .OnDelete(DeleteBehavior.Cascade);

            // Cve - One to Many with Weaknesses
            modelBuilder.Entity<Cve>()
                .HasMany(c => c.Weaknesses)
                .WithOne(w => w.Cve)
                .HasForeignKey(w => w.CveId)
                .OnDelete(DeleteBehavior.Cascade);

            // Cve - One to Many with Configurations
            modelBuilder.Entity<Cve>()
                .HasMany(c => c.Configurations)
                .WithOne(cfg => cfg.Cve)
                .HasForeignKey(cfg => cfg.CveId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configuration - One to Many with Nodes
            modelBuilder.Entity<Configuration>()
                .HasMany(cfg => cfg.Nodes)
                .WithOne(n => n.Configuration)
                .HasForeignKey(n => n.ConfigurationId)
                .OnDelete(DeleteBehavior.Cascade);

            // Node - One to Many with CpeMatches
            modelBuilder.Entity<Node>()
                .HasMany(n => n.CpeMatches)
                .WithOne(cm => cm.Node)
                .HasForeignKey(cm => cm.NodeId)
                .OnDelete(DeleteBehavior.Cascade);

            // Cve - One to Many with References
            modelBuilder.Entity<Cve>()
                .HasMany(c => c.References)
                .WithOne(r => r.Cve)
                .HasForeignKey(r => r.CveId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}