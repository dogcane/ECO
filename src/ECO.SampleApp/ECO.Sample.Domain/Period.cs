using System;

namespace ECO.Sample.Domain
{
    public class Period : IValueObject<Period>
    {
        #region Properties

        public DateTime StartDate { get; private set; }

        public DateTime EndDate { get; private set; }

        #endregion

        #region Ctor

        protected Period()
        {

        }

        public Period(DateTime startDate, DateTime endDate)
        {
            StartDate = startDate;
            EndDate = endDate;
        }

        #endregion

        #region IEquatable<Period> Membri di

        public bool Equals(Period other)
        {
            return StartDate.Equals(other.StartDate) && EndDate.Equals(other.EndDate);
        }

        #endregion
    }
}
