﻿using System.Collections.Generic;
using SaveASpot.Core;
using SaveASpot.Services.Interfaces;
using SaveASpot.Repositories.Interfaces.Sponsors;
using SaveASpot.Repositories.Models;

namespace SaveASpot.Services.Implementations
{
	public sealed class SponsorsService : ISponsorsService
	{
		private readonly ISponsorRepository _sponsorRepository;
		private readonly ISponsorQueryable _sponsorQueryable;

		public SponsorsService(ISponsorRepository sponsorRepository, ISponsorQueryable sponsorQueryable)
		{
			_sponsorRepository = sponsorRepository;
			_sponsorQueryable = sponsorQueryable;
		}

		public IMethodResult<CreateSponsorResult> AddSponsor(SponsorArg sponsorArg)
		{
			var sponsor = _sponsorRepository.AddSponsor(new Sponsor()
				                              {
					                              CompanyName = sponsorArg.CompanyName,
					                              Sentence = sponsorArg.Sentence,
					                              Logo = sponsorArg.Logo,
					                              Url = sponsorArg.Url
				                              });
			
			return string.IsNullOrEmpty(sponsor.Identity) 
				? new MethodResult<CreateSponsorResult>(true, new CreateSponsorResult { SponsorId = sponsor.Identity })
				: new MethodResult<CreateSponsorResult>(false, new CreateSponsorResult { MessageKet = "Error occured during sponsor creating" });
		}

		public IEnumerable<Sponsor> GetAllSponsors()
		{
			return _sponsorQueryable.Find(_sponsorQueryable.All());
		}
	}
}