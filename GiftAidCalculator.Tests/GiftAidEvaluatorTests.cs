using Moq;
using NUnit.Framework;

namespace GiftAidCalculator.Tests
{
    [TestFixture]
    public class GiftAidEvaluatorTests
    {
        private IGiftAidEvaluator _giftAidEvaluator;
        private Mock<ITax> _taxRateValueSource;
        private Mock<IGiftAidSupplementer> _giftAidSupplementer;

        [SetUp]
        public void SetUp()
        {
            _taxRateValueSource = new Mock<ITax>();
            _giftAidSupplementer = new Mock<IGiftAidSupplementer>();
        }

        [Test]
        public void GivenDonationOfZero_ReturnZeroGiftAidAmount()
        {
            _giftAidEvaluator = new GiftAidEvaluator(_taxRateValueSource.Object);
            var result = _giftAidEvaluator.Calculate(0);
            Assert.AreEqual(0, result);
        }

        [TestCase(100.00, 25.00)]
        public void GivenDonationAmountAnd20PercentTaxRate_ReturnGiftAidAmount(decimal donationAmount, decimal expectedGiftAidAmount)
        {
            SetUpSystemUnderTest(20.0m);
            var result = _giftAidEvaluator.Calculate(donationAmount);
            Assert.AreEqual(expectedGiftAidAmount, result);
        }

        [TestCase(200.00, 50.00, 200.00)]
        public void GivenDonationAmountAndVariableTaxRate_ReturnGiftAidAmount(decimal donationAmount, decimal taxRateValue, decimal expectedGiftAidAmount)
        {
            SetUpSystemUnderTest(taxRateValue);
            var result = _giftAidEvaluator.Calculate(donationAmount);
            Assert.AreEqual(expectedGiftAidAmount, result);
        }

        [TestCase(105.50, 26.38)]
        [TestCase(94.05, 23.51)]
        public void GivenDonationAmount_ReturnGiftAidAmountRoundedTo2DecimalPlaces(decimal donationAmount, decimal expectedGiftAidAmount)
        {
            SetUpSystemUnderTest(20.0m);
            var result = _giftAidEvaluator.Calculate(donationAmount);
            Assert.AreEqual(expectedGiftAidAmount, result);
        }

        [TestCase(100.00, 5.00, 26.25)]
        [TestCase(100.00, 3.00, 25.75)]
        [TestCase(100.00, 0.00, 25.00)]
        public void GivenDonationAmountAndSupplementPercentage_ReturnSupplementedGiftAidAmount(decimal donationAmount, decimal supplementPercentageValue, decimal expectedGiftAidAmount)
        {
            SetUpSystemUnderTest(20.0m, supplementPercentageValue);
            var result = _giftAidEvaluator.Calculate(donationAmount);
            Assert.AreEqual(expectedGiftAidAmount, result);
        }

        private void SetUpSystemUnderTest(decimal taxRateValue)
        {
            _giftAidEvaluator = new GiftAidEvaluator(_taxRateValueSource.Object);
            _taxRateValueSource.Setup(x => x.CurrentTaxRate).Returns(taxRateValue);
        }

        private void SetUpSystemUnderTest(decimal taxRateValue, decimal supplementPercentageValue)
        {
            _giftAidEvaluator = new GiftAidEvaluator(_taxRateValueSource.Object, _giftAidSupplementer.Object);
            _taxRateValueSource.Setup(x => x.CurrentTaxRate).Returns(taxRateValue);
            _giftAidSupplementer.Setup(x => x.SupplementPercentage).Returns(supplementPercentageValue);
        }
    }
}