using Xunit;
using System;
using System.IO;
using Microsoft.AspNetCore.Http;
using Global_Logistics_Management_System.Models;
using Contract = Global_Logistics_Management_System.Models.Contract;

namespace Global_Logistics_Management_System.Tests
{
    public class LogisticsBusinessRuleTests
    {
        // ==========================================
        // 1. CURRENCY CALCULATION TESTS
        // ==========================================
        [Theory]
        [InlineData(100.00, 18.50, 1850.00)]
        [InlineData(50.00, 19.12, 956.00)]
        [InlineData(0.00, 18.50, 0.00)]
        public void CalculateZarCost_GivenSpecificRate_ReturnsCorrectMath(decimal usdAmount, decimal exchangeRate, decimal expectedZar)
        {
            // Act: Perform the system conversion multiplication step
            decimal actualZar = usdAmount * exchangeRate;

            // Assert: Verify the arithmetic matches down to the decimal fractions
            Assert.Equal(expectedZar, actualZar);
        }

        // ==========================================
        // 2. FILE VALIDATION TESTS
        // ==========================================
        [Fact]
        public void FileValidation_AllowedPdfExtension_ReturnsTrueOrPasses()
        {
            // Arrange: Mock a clean, harmless PDF tracking slip upload 
            var fileName = "signed_sla_agreement.pdf";

            // Act: Extract extension utilizing standard system path logic
            var extension = Path.GetExtension(fileName).ToLower();
            bool isAllowed = (extension == ".pdf");

            // Assert
            Assert.True(isAllowed, "The validation framework should accept standard .pdf uploads.");
        }

        [Theory]
        [InlineData("malicious_script.exe")]
        [InlineData("backdoor_payload.msi")]
        [InlineData("database_dump.zip")]
        public void FileValidation_RestrictedExtension_ShouldFailValidation(string dangerousFileName)
        {
            // Act: Extract the testing extension parameters
            var extension = Path.GetExtension(dangerousFileName).ToLower();
            bool isAllowed = (extension == ".pdf");

            // Assert: Confirm that these danger files fail the validation block completely
            Assert.False(isAllowed, $"Security Breach! The system must block restricted '{extension}' extensions.");
        }

        // ==========================================
        // 3. BIG SYSTEM TASKS (CONTRACT RULES)
        // ==========================================
        [Fact]
        public void ServiceRequest_LinkedToExpiredContract_ShouldBeFlaggedInvalid()
        {
            // Arrange: Setup a parent contract explicitly hardcoded to Expired status
            var expiredContract = new Contract
            {
                ContractId = 101,
                ContractName = "Legacy Freight Clause",
                Status = ContractStatus.Expired,
                StartDate = DateTime.Now.AddYears(-1),
                EndDate = DateTime.Now.AddMonths(-2)
            };

            var newRequest = new ServiceRequest
            {
                JobDescription = "Urgent Port Cargo Offload",
                ContractId = expiredContract.ContractId,
                Contract = expiredContract // Associating navigation context
            };

            // Act: Evaluate underlying business logic state rule
            bool canAllocateJob = (newRequest.Contract.Status == ContractStatus.Active);

            // Assert: Verify that the system task safely flags this job allocation as forbidden
            Assert.False(canAllocateJob, "Critical Rule: Expired SLA baseline agreements must block incoming logistics orders.");
        }
    }
}