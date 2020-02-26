using Moq;
using NUnit.Framework;
using System.Linq;
using TacitusLogger.Builders;
using TacitusLogger.Contributors;

namespace TacitusLogger.IntegrationTests.Builders
{
    [TestFixture]
    public class LogContributorsBuilderTests
    {
        [Test]
        public void LogContributorsBuilder_When_Registering_Contributors_Then_Creates_Logger_With_Them()
        {
            // Arrange
            var logContributor1 = new Mock<LogContributorBase>("contributor1").Object;
            var logContributor2 = new Mock<LogContributorBase>("contributor2").Object;

            // Act
            Logger logger = LoggerBuilder.Logger().Contributors()
                                                  .Custom(logContributor1, false)
                                                  .Custom(logContributor2, true)
                                                  .BuildContributors()
                                                  .BuildLogger();

            // Assert
            var contributors = logger.LogContributors.ToList();
            Assert.AreEqual(2, contributors.Count);
            Assert.AreEqual(logContributor1, contributors[0]);
            Assert.AreEqual(logContributor2, contributors[1]);
            Assert.IsFalse(contributors[0].IsActive);
            Assert.IsTrue(contributors[1].IsActive);
        }
        [Test]
        public void LogContributorsBuilder_Changing_Contributor_Status_In_Runtime()
        {
            // Arrange
            var logContributor1 = new Mock<LogContributorBase>("contributor1").Object;
            var status = Setting<bool>.From.Variable(false);
            Logger logger = LoggerBuilder.Logger().Contributors()
                                                  .Custom(logContributor1, status)
                                                  .BuildContributors()
                                                  .BuildLogger();

            var contributorFromLogger = logger.LogContributors.Single();

            // Act
            Assert.AreEqual(false, contributorFromLogger.IsActive.Value);
            status.SetValue(true);
            Assert.AreEqual(true, contributorFromLogger.IsActive.Value);
            status.SetValue(false);
            Assert.AreEqual(false, contributorFromLogger.IsActive.Value);
        }
    }
}
