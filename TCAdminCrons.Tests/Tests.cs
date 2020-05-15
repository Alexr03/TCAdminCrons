using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using TCAdminCrons.Models;
using TCAdminCrons.Models.MinecraftVanilla;

namespace TCAdminCrons.Tests
{
    [TestFixture]
    public class Tests
    {
        // [Test]
        public void TcAdminConnectedSuccessfully()
        {
            Console.WriteLine("Current dir: " + Directory.GetCurrentDirectory());
            Program.RegisterToTcAdmin();
            Assert.True(new TCAdmin.GameHosting.SDK.Objects.Server(1).Find());
        }

        [Test]
        public void MinecraftManifestsCreated()
        {
            var minecraftVersionManifest = MinecraftVersionManifest.GetManifests();
            foreach (var version in minecraftVersionManifest.Versions.Where(x => x.Type.ToLower() == "snapshot")
                .Take(2))
            {
                Console.WriteLine(
                    $"Version: {version.Id} with URL: {version.Url} | {version.GetMetadata().Downloads.Server.Url} ({version.GetMetadata().Id})");
            }

            Assert.NotNull(minecraftVersionManifest);
        }
    }
}