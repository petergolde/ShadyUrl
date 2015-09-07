using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ShadyUrl.Models;

namespace ShadyUrl.Controllers
{
    public class HomeController : Controller
    {
        const string htmlDirectory = @"C:\inetpub\shady";
        const string urlPrefix = @"http://6k1d5.gq/";
        const string templateFile = @"C:\inetpub\shady\template.html";

        public ActionResult Index()
        {
            return View(new ShortenUrl());
        }

        [HttpPost]
        public ActionResult Index(ShortenUrl shortenUrl)
        {
            string pathName;
            if (!shortenUrl.ForwardTo.StartsWith("http://"))
                shortenUrl.ForwardTo = "http://" + shortenUrl.ForwardTo;

            shortenUrl.ShadyUrl = GenerateShadyUrl(out pathName);
            FilterFile(templateFile, pathName, shortenUrl.ForwardTo);

            return View(shortenUrl);
        }

        private string GenerateShadyUrl(out string pathname)
        {
            string baseName = GenerateBaseName();
            pathname = Path.Combine(htmlDirectory, baseName);
            return urlPrefix + baseName;
        }

        private string GenerateBaseName()
        {
            // pick first word
            int first = rand.Next(possibleText.Length);

            // pick second word different from first
            int second;
            do {
                second = rand.Next(possibleText.Length);
            } while (second == first);

            // picks third word different from first and second
            int third;
            do {
                third = rand.Next(possibleText.Length);
            } while (third == first || third == second);

            return possibleText[first] + GeneratorRandomSep() + 
                GenerateRandomString(3) + GeneratorRandomSep() + 
                possibleText[second] + GeneratorRandomSep() + 
                GenerateRandomString(3) + GeneratorRandomSep() + 
                possibleText[third] +
                GenerateRandomExtension();
        }

        private string GenerateRandomString(int len)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < len; ++i) {
                builder.Append(letters[rand.Next(letters.Length)]);
            }
            return builder.ToString();
        }

        private string GenerateRandomExtension()
        {
            int index = rand.Next(possibleExtensions.Length);
            return possibleExtensions[index];
        }

        private string GeneratorRandomSep()
        {
            int r = rand.Next(2);
            if (r == 1)
                return "-";
            else
                return "_";
        }

        private void FilterFile(string fromFileName, string toFileName, string forwardTo)
        {
            string input = System.IO.File.ReadAllText(fromFileName);
            string output = input.Replace("%FORWARDTO%", forwardTo);
            System.IO.File.WriteAllText(toFileName, output);
        }

        Random rand = new Random();

        string letters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

        string[] possibleExtensions = {
            ".exe",
            ".jar",
            ".gif",
            ".dll",
            ".msi",
            ".png",
            ".flv",
            ".mid",
            ".xsn",
            ".zip",
            ".htm",
            ".html",
            ".hxt",
            ".java",
            ".midi",
            ".mov",
            ".movie",
            ".mp2",
            ".mp3",
            ".mpa",
            ".mpe",
            ".mpeg",
            ".mpg",
            ".vbs",
            ".m1v",
            ".xaf",
            ".snd",
            ".p7r"
        };

        string[] possibleText = {
            "virus",
            "keylogger",
            "malware",
            "flash",
            "gmail",
            "password",
            "advert",
            "popup",
            "stuxnet",
            "flame",
            "java",
            "penetration",
            "dns",
            "internet",
            "arpa",
            "root",
            "fake",
            "windows",
            "install",
            "detector",
            "keylog",
            "hidden",
            "invis",
            "google",
            "steal",
            "impersonate",
            "hidden",

        };
    }
}