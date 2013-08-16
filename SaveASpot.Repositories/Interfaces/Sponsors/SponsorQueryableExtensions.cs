using System;
using SaveASpot.Repositories.Models;

namespace SaveASpot.Repositories.Interfaces.Sponsors
{
	public static class SponsorQueryableExtensions
	{
		public static QueryableBuilder<Sponsor, ISponsorFilter, ISponsorQueryable> Filter(this ISponsorQueryable source, Func<ISponsorQueryable, ISponsorFilter> filter)
		{
			return new QueryableBuilder<Sponsor, ISponsorFilter, ISponsorQueryable>(source).And(filter);
		}
	}
}