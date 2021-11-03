using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.IO;

namespace JiraRedirector
{
    enum ReturnCodes
    {
        Success = 0,
        InvalidUrl = 1,
        ErrorOpeningBrowser = 2,
        InvalidArguments = 3,
    }

    class Program
    {
        static readonly Regex pattern = new Regex("([a-zA-Z]{2,3}-[0-9]{1,6})");

        static int Main(string[] args)
        {
            var urlBase = GetJiraBase();
            if (string.IsNullOrEmpty(urlBase))
            {
                return (int)ReturnCodes.InvalidUrl;
            }
            if (!urlBase.EndsWith("/"))
            {
                urlBase += "/";
            }

            var allArguments = string.Join(' ', args);
            var matches = pattern.Matches(allArguments);

            if (matches.Count == 0)
            {
                return (int)ReturnCodes.InvalidArguments;
            }

            foreach (var match in matches)
            {
                var url = urlBase + match.ToString().ToUpperInvariant();

                try
                {
                    var startInfo = new ProcessStartInfo
                    {
                        UseShellExecute = true,
                        FileName = url,
                    };
                    Process.Start(startInfo);
                }
                catch
                {
                    Console.Error.WriteLine("Could not open the web browser.");
                    return (int)ReturnCodes.ErrorOpeningBrowser;
                }
            }

            return (int)ReturnCodes.Success;
        }

        static string GetJiraBase()
        {
            var defaultFileName = "JiraRedirector.settings";
            var filePathsToCheck = new string[] {
                Path.Combine(Environment.CurrentDirectory, defaultFileName),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), defaultFileName),
                Path.Combine(Environment.SystemDirectory, defaultFileName),
            };

            foreach (var filePath in filePathsToCheck)
            {
                if (File.Exists(filePath))
                {
                    try
                    {
                        var content = File.ReadAllText(filePath);
                        var uri = new Uri(content);
                        return uri.ToString();
                    }
                    catch (UriFormatException)
                    {
                        Console.Error.WriteLine($"The settings file '{filePath}' was found but did not contain a valid URL.");
                    }
                    catch (Exception)
                    {
                        Console.Error.WriteLine($"The settings file '{filePath}' was found but could not be read.");
                    }
                }
            }

            Console.Error.WriteLine("You must create a file containing the base URL to your Jira instance. The following locations are valid:");
            foreach (var filePath in filePathsToCheck)
            {
                Console.Error.WriteLine($" - {filePath}");
            }
            return null;
        }
    }
}
