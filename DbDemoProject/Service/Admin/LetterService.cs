using DbDemoProject.Data;
using DbDemoProject.Model.Data.Model;
using DbDemoProject.Model.ResponseDTO;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using WordText = DocumentFormat.OpenXml.Wordprocessing.Text;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace DbDemoProject.Service.Admin
{
    public class LetterService : ILetterService
    {
        private readonly DbContextHelper _dbContextHelper;
        private readonly IConfiguration _configuration;

        public LetterService(
            DbContextHelper dbContextHelper,
            IConfiguration configuration)
        {
            _dbContextHelper = dbContextHelper;
            _configuration = configuration;
        }

        public List<LetterModel> GetLetter()
        {
            return _dbContextHelper.Letter
                .Where(letter => letter.IsActive)
                .ToList();
        }

        public int SaveLetter(LetterRequestResponse letterModel)
        {
            using var memoryStream = new MemoryStream();

            if (letterModel.FileByte != null)
            {
                letterModel.FileByte.CopyTo(memoryStream);
            }

            var letter = new LetterModel
            {
                LetterUUID = Guid.NewGuid(),
                DocumentName = letterModel.DocumentName,
                DocumentType = letterModel.DocumentType,
                FileByte = memoryStream.ToArray(),
                IsActive = true,
                CreatedOn = DateTime.UtcNow
            };

            _dbContextHelper.Letter.Add(letter);

            return _dbContextHelper.SaveChanges();
        }

        public async Task<byte[]> GenerateAppointmentLetter(
            Guid? appointmentUUID)
        {
            if (!appointmentUUID.HasValue ||
                appointmentUUID.Value == Guid.Empty)
            {
                throw new ArgumentException(
                    "A valid appointment UUID is required.",
                    nameof(appointmentUUID));
            }

            var appointment = await (
                from appoint in _dbContextHelper.Appointment
                join member in _dbContextHelper.Member
                    on appoint.MemberUUID equals member.MemberUUID
                where appoint.AppointmentUUID == appointmentUUID.Value
                select new AppointmentRequestModel
                {
                    MemberID = member.MemberID,
                    MemberUUID = member.MemberUUID,
                    DateOfBirth = member.DateOfBirth,
                    PhoneNumber = member.PhoneNumber,
                    FirstName = member.FirstName,
                    LastName = member.LastName,
                    AppointmentUUID = appoint.AppointmentUUID,
                    AppointmentDate = appoint.AppointmentDate
                })
                .FirstOrDefaultAsync();

            if (appointment == null)
            {
                throw new KeyNotFoundException(
                    "The requested appointment was not found.");
            }

            byte[] templateBytes = await GetFileByte();

            var replacements = new Dictionary<string, string>
            {
                ["{{MemberName}}"] =
                    $"{appointment.FirstName} {appointment.LastName}".Trim(),

                ["{{MemberID}}"] =
                    appointment.MemberID?.ToString() ?? string.Empty,

                ["{{MemberUUID}}"] =
                    appointment.MemberUUID.ToString(),

                ["{{PhoneNumber}}"] =
                    appointment.PhoneNumber?.ToString() ?? string.Empty,

                ["{{DateOfBirth}}"] =
                    appointment.DateOfBirth?.ToString("dd MMMM yyyy")
                    ?? string.Empty,

                ["{{AppointmentUUID}}"] =
                    appointment.AppointmentUUID.ToString(),

                //["{{AppointmentDate}}"] =
                //    appointment.AppointmentDate.ToString("dd MMMM yyyy")
            };

            var tempDirectory = Path.Combine(
                Path.GetTempPath(),
                "DbDemoProject",
                Guid.NewGuid().ToString("N"));

            Directory.CreateDirectory(tempDirectory);

            var docxPath = Path.Combine(
                tempDirectory,
                "AppointmentLetter.docx");

            var pdfPath = Path.Combine(
                tempDirectory,
                "AppointmentLetter.pdf");

            try
            {
                await File.WriteAllBytesAsync(docxPath, templateBytes);

                ReplacePlaceholders(docxPath, replacements);

                var libreOfficePath =
                    _configuration["AppointmentLetter:LibreOfficePath"];

                if (string.IsNullOrWhiteSpace(libreOfficePath) ||
                    !File.Exists(libreOfficePath))
                {
                    throw new FileNotFoundException(
                        "Configure a valid LibreOffice executable path " +
                        "in AppointmentLetter:LibreOfficePath.");
                }

                var startInfo = new ProcessStartInfo
                {
                    FileName = libreOfficePath,
                    WorkingDirectory = tempDirectory,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                };

                startInfo.ArgumentList.Add("--headless");
                startInfo.ArgumentList.Add("--convert-to");
                startInfo.ArgumentList.Add("pdf");
                startInfo.ArgumentList.Add("--outdir");
                startInfo.ArgumentList.Add(tempDirectory);
                startInfo.ArgumentList.Add(docxPath);

                using var process = new Process
                {
                    StartInfo = startInfo
                };

                process.Start();

                var outputTask =
                    process.StandardOutput.ReadToEndAsync();

                var errorTask =
                    process.StandardError.ReadToEndAsync();

                await process.WaitForExitAsync();

                var output = await outputTask;
                var error = await errorTask;

                if (process.ExitCode != 0 || !File.Exists(pdfPath))
                {
                    throw new InvalidOperationException(
                        $"DOCX-to-PDF conversion failed. " +
                        $"Exit code: {process.ExitCode}. " +
                        $"Output: {output}. Error: {error}");
                }

                // Return the generated PDF as bytes.
                return await File.ReadAllBytesAsync(pdfPath);
            }
            finally
            {
                try
                {
                    if (Directory.Exists(tempDirectory))
                    {
                        Directory.Delete(
                            tempDirectory,
                            recursive: true);
                    }
                }
                catch (IOException)
                {
                    // Temporary-file cleanup can be retried separately.
                }
                catch (UnauthorizedAccessException)
                {
                    // Do not hide the primary generation result.
                }
            }
        }

        private async Task<byte[]> GetFileByte()
        {
            var template = await _dbContextHelper.Letter
                .Where(letter =>
                    letter.IsActive &&
                    letter.DocumentType == "appointment letter" &&
                    letter.FileByte != null &&
                    letter.FileByte.Length > 0)
                .FirstOrDefaultAsync();

            if (template == null)
            {
                throw new FileNotFoundException(
                    "No active appointment letter template was found. " +
                    "Check the DocumentType and uploaded template.");
            }

            return template.FileByte;
        }

        private static void ReplacePlaceholders(
            string docxPath,
            Dictionary<string, string> replacements)
        {
            using var document =
                WordprocessingDocument.Open(docxPath, true);

            var mainPart = document.MainDocumentPart
                ?? throw new InvalidOperationException(
                    "The Word document has no main document part.");

            var parts = new List<OpenXmlPart> { mainPart };

            parts.AddRange(mainPart.HeaderParts);
            parts.AddRange(mainPart.FooterParts);

            foreach (var part in parts)
            {
                if (part.RootElement == null)
                {
                    continue;
                }

                foreach (var paragraph in
                         part.RootElement.Descendants<Paragraph>())
                {
                    var textNodes = paragraph
                        .Descendants<WordText>()
                        .ToList();

                    if (textNodes.Count == 0)
                    {
                        continue;
                    }

                    var originalText = string.Concat(
                        textNodes.Select(node => node.Text));

                    var updatedText = originalText;

                    foreach (var replacement in replacements)
                    {
                        updatedText = updatedText.Replace(
                            replacement.Key,
                            replacement.Value,
                            StringComparison.Ordinal);
                    }

                    if (updatedText == originalText)
                    {
                        continue;
                    }

                    textNodes[0].Text = updatedText;
                    textNodes[0].Space =
                        SpaceProcessingModeValues.Preserve;

                    foreach (var node in textNodes.Skip(1))
                    {
                        node.Text = string.Empty;
                    }
                }

                part.RootElement.Save();
            }
        }
    }
}