using CVEGuard.Model;
using Microsoft.EntityFrameworkCore;
using System.IO.Compression;
using System.Reflection;
using System.Text.Json;
using System.Text;

namespace CVEGuard
{
    public static class DatabaseSeeder
    {
        public static void ExtractCVEs(CVEGuardDbContext context)
        {
            // Get the executing assembly
            Assembly assembly = Assembly.GetExecutingAssembly();

            // Get all embedded ZIP file names
            string[] resourceNames = assembly.GetManifestResourceNames();

            foreach (string resourceName in resourceNames)
            {
                if (resourceName.EndsWith(".zip"))
                {
                    Console.WriteLine($"Processing ZIP: {resourceName}");
                    using (Stream? zipStream = assembly.GetManifestResourceStream(resourceName))
                    {
                        if (zipStream == null)
                        {
                            Console.WriteLine($"Error: Unable to find resource {resourceName}");
                            continue;
                        }

                        using (ZipArchive archive = new ZipArchive(zipStream, ZipArchiveMode.Read))
                        {
                            foreach (ZipArchiveEntry entry in archive.Entries)
                            {
                                if (entry.FullName.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
                                {
                                    Console.WriteLine($"Reading JSON file: {entry.FullName}");

                                    using (Stream entryStream = entry.Open())
                                    using (MemoryStream memoryStream = new MemoryStream())
                                    {
                                        entryStream.CopyTo(memoryStream);
                                        byte[] fileData = memoryStream.ToArray();

                                        // ✅ Process the extracted JSON file
                                        ProcessJson(entry.FullName, fileData, context);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            Console.WriteLine("All ZIP files processed in-memory.");
        }
        static void ProcessJson(string fileName, byte[] fileData, CVEGuardDbContext context)
        {
            Console.WriteLine($"Processing JSON file: {fileName} ({fileData.Length} bytes)");

            try
            {
                // Convert byte array to string
                string jsonString = Encoding.UTF8.GetString(fileData);

                // Deserialize JSON into a dictionary (modify based on actual structure)
                var jsonData = JsonSerializer.Deserialize<VulnerabilityResponse>(jsonString);

                // ✅ Process JSON data (replace with actual logic)
                Console.WriteLine($"JSON Data Extracted from {fileName}");
                if (jsonData != null)
                {
                    context.Vulnerabilities.AddRange(jsonData.Vulnerabilities);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing JSON file {fileName}: {ex.Message}");
            }
        }
    }
}
