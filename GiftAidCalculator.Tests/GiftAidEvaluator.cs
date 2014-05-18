using System;

namespace GiftAidCalculator.Tests
{
    public class GiftAidEvaluator : IGiftAidEvaluator
    {
        private readonly ITax _taxObject;
        private readonly IGiftAidSupplementer _giftAidSupplementer;

        public GiftAidEvaluator(ITax taxObject)
        {
            _taxObject = taxObject;
        }

        public GiftAidEvaluator(ITax taxObject, IGiftAidSupplementer giftAidSupplementer)
        {
            _taxObject = taxObject;
            _giftAidSupplementer = giftAidSupplementer;
        }

        public decimal Calculate(decimal donationAmount)
        {
            return Math.Round(donationAmount * GiftAidRatio * SupplementPercentage, 2);
        }

        private decimal GiftAidRatio
        {
            get
            {
                return _taxObject.CurrentTaxRate / (100m - _taxObject.CurrentTaxRate);
            }
        }

        private decimal SupplementPercentage 
        { 
            get
            {
                if (_giftAidSupplementer != null)
                {
                    return 1m + _giftAidSupplementer.SupplementPercentage / 100m;
                }
                return 1m;
            } 
        }
    }
}