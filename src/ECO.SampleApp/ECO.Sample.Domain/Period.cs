using System;

namespace ECO.Sample.Domain
{
    public struct Period : IValueObject<Period>
    {
        #region Properties

        public DateTime StartDate { get; private set; }

        public DateTime EndDate { get; private set; }

        #endregion

        #region Ctor

        public Period(DateTime startDate, DateTime endDate)
            : this()
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
