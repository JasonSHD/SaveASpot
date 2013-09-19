using System;
using System.Collections.Generic;
using System.Linq;
using SaveASpot.Core;
using SaveASpot.Core.Configuration;
using SaveASpot.Core.Geocoding;
using SaveASpot.Core.Strategies;
using SaveASpot.Repositories.Interfaces.PhasesAndParcels;
using SaveASpot.Repositories.Models;
using SaveASpot.Services.Interfaces;
using SaveASpot.Services.Interfaces.Geocoding;
using SaveASpot.ViewModels;
using SaveASpot.ViewModels.PhasesAndParcels;

namespace SaveASpot.Services.Implementations.Geocoding
{
	public sealed class SquareElementsCalculator : ISquareElementsCalculator
	{
		private readonly IParcelQueryable _parcelQueryable;
		private readonly ISpotQueryable _spotQueryable;
		private readonly IConfigurationManager _configurationManager;
		private readonly IElementIdentityConverter _elementIdentityConverter;
		private readonly IStrategyManager<SquareStrategyArg, SquareElementsResult> _strategyManager;
		public const string MaxSpotsPerPageKey = "MaxSpotsPerPage";

		public SquareElementsCalculator(
			IParcelQueryable parcelQueryable,
			ISpotQueryable spotQueryable,
			IConfigurationManager configurationManager,
			IElementIdentityConverter elementIdentityConverter,
			IStrategyManager<SquareStrategyArg, SquareElementsResult> strategyManager)
		{
			_parcelQueryable = parcelQueryable;
			_spotQueryable = spotQueryable;
			_configurationManager = configurationManager;
			_elementIdentityConverter = elementIdentityConverter;
			_strategyManager = strategyManager;
		}

		public SquareElementsResult FindElementsForSquare(IElementIdentity phase, Point leftBottom, Point rightTop)
		{
			var maxSpotsPerPage = _configurationManager.GetInt32Settings(MaxSpotsPerPageKey).Status;
			var parcels = _parcelQueryable.Filter(e => e.ByPhase(phase)).Select(e => _elementIdentityConverter.ToIdentity(e.Id)).ToList();
			var spots = _spotQueryable.Filter(e => e.ByParcels(parcels)).ToList();

			return _strategyManager.Execute(new SquareStrategyArg(spots, leftBottom, rightTop, maxSpotsPerPage)).First(e => e.IsSuccess).Status;
		}
	}

	public sealed class SquareStrategyArg
	{
		private readonly IList<Spot> _allSpots;
		private readonly Point _leftBottom;
		private readonly Point _rightTop;
		private readonly int _maxSpotsPerPage;

		private IEnumerable<Spot> _spotsInSquare;
		private int _allSpotsCount = -1;
		private int _spotsInSquareCount = -1;
		private Point _center;

		public SquareStrategyArg(IEnumerable<Spot> allSpots, Point leftBottom, Point rightTop, int maxSpotsPerPage)
		{
			_allSpots = allSpots.ToList();
			_leftBottom = leftBottom;
			_rightTop = rightTop;
			_maxSpotsPerPage = maxSpotsPerPage;
		}

		public int AllSpotsCount
		{
			get
			{
				if (_allSpotsCount == -1)
				{
					_allSpotsCount = AllSpots().Count();
				}
				return _allSpotsCount;
			}
		}
		public int SpotsInSquareCount
		{
			get
			{
				if (_spotsInSquareCount == -1)
				{
					_spotsInSquareCount = SpotsInSquare().Count();
				}
				return _spotsInSquareCount;
			}
		}
		public int MaxSpotsPerPage { get { return _maxSpotsPerPage; } }
		public IEnumerable<Spot> AllSpots()
		{
			return _allSpots;
		}

		public IEnumerable<Spot> SpotsInSquare()
		{
			if (_spotsInSquare == null)
			{
				var spotsInSquare = new List<Spot>();

				foreach (var spot in _allSpots)
				{
					if (spot.SpotShape.Any(e => Point.PointInSquare(e, _leftBottom, _rightTop)))
					{
						spotsInSquare.Add(spot);
					}
				}

				_spotsInSquare = spotsInSquare;
			}

			return _spotsInSquare;
		}

		public Point Center()
		{
			if (_center == null)
			{
				_center = new Point
				{
					Latitude = SpotsInSquareCount == 0 ? 0 : _spotsInSquare.SelectMany(e => e.SpotShape).Select(e => e.Latitude).Sum() / SpotsInSquareCount,
					Longitude = SpotsInSquareCount == 0 ? 0 : _spotsInSquare.SelectMany(e => e.SpotShape).Select(e => e.Longitude).Sum() / SpotsInSquareCount
				};
			}

			return _center;
		}
	}

	public sealed class AllSpotsStrategy : IStrategy<SquareStrategyArg, SquareElementsResult>, IStrategyOrder
	{
		private readonly ITypeConverter<Spot, SpotViewModel> _typeConverter;

		public AllSpotsStrategy(ITypeConverter<Spot, SpotViewModel> typeConverter)
		{
			_typeConverter = typeConverter;
		}

		public IMethodResult<SquareElementsResult> Execute(SquareStrategyArg arg)
		{
			if (arg.MaxSpotsPerPage >= arg.AllSpotsCount)
			{
				return new StrategyResult<SquareElementsResult>(true, true, new SquareElementsResult
																																			{
																																				Center = new Point(),
																																				Message = string.Empty,
																																				Spots = arg.AllSpots().Select(e => _typeConverter.Convert(e)).ToList(),
																																				Status = SquareElementsResult.ResultStatus.All
																																			});
			}

			return new MethodResult<SquareElementsResult>(false, new SquareElementsResult());
		}

		public int Order
		{
			get { return 0; }
		}
	}

	public sealed class MuchSpotsStrategy : IStrategy<SquareStrategyArg, SquareElementsResult>, IStrategyOrder
	{
		private readonly ITextService _textService;

		public MuchSpotsStrategy(ITextService textService)
		{
			_textService = textService;
		}

		public IMethodResult<SquareElementsResult> Execute(SquareStrategyArg arg)
		{
			if (arg.SpotsInSquareCount > arg.MaxSpotsPerPage)
			{
				return new StrategyResult<SquareElementsResult>(true, true, new SquareElementsResult
																																			{
																																				Center = arg.Center(),
																																				Message = _textService.ResolveTest(Constants.Errors.MuchSpotsInSquare),
																																				Spots = Enumerable.Empty<SpotViewModel>(),
																																				Status = SquareElementsResult.ResultStatus.Much
																																			});
			}

			return new StrategyResult<SquareElementsResult>(false, false, new SquareElementsResult());
		}

		public int Order
		{
			get { return 10; }
		}
	}

	public sealed class NotFoundSpotsStrategy : IStrategy<SquareStrategyArg, SquareElementsResult>, IStrategyOrder
	{
		private readonly ITextService _textService;

		public NotFoundSpotsStrategy(ITextService textService)
		{
			_textService = textService;
		}

		public IMethodResult<SquareElementsResult> Execute(SquareStrategyArg arg)
		{
			if (arg.SpotsInSquareCount == 0)
			{
				return new BreakStrategyResult<SquareElementsResult>(new SquareElementsResult
																																			{
																																				Center = arg.Center(),
																																				Message = _textService.ResolveTest(Constants.Errors.SpotsInCurrentRegionNotFound),
																																				Spots = Enumerable.Empty<SpotViewModel>(),
																																				Status = SquareElementsResult.ResultStatus.NotFound
																																			});
			}

			return new ContinueStrategyResult<SquareElementsResult>(new SquareElementsResult());
		}

		public int Order
		{
			get { return 20; }
		}
	}

	public sealed class PartSpotsStrategy : IStrategy<SquareStrategyArg, SquareElementsResult>, IStrategyOrder
	{
		private readonly ITextService _textService;

		public PartSpotsStrategy(ITextService textService)
		{
			_textService = textService;
		}

		public IMethodResult<SquareElementsResult> Execute(SquareStrategyArg arg)
		{
			if (arg.SpotsInSquareCount == 0)
			{
				return new BreakStrategyResult<SquareElementsResult>(new SquareElementsResult
																															 {
																																 Center = arg.Center(),
																																 Message = _textService.ResolveTest(Constants.Errors.PartSpotsInSquare),
																																 Spots = Enumerable.Empty<SpotViewModel>(),
																																 Status = SquareElementsResult.ResultStatus.Part
																															 });
			}

			return new ContinueStrategyResult<SquareElementsResult>(new SquareElementsResult());
		}

		public int Order
		{
			get { return 30; }
		}
	}

	public sealed class LastSpotsStrategy : IStrategy<SquareStrategyArg, SquareElementsResult>, IStrategyOrder
	{
		private readonly ITextService _textService;

		public LastSpotsStrategy(ITextService textService)
		{
			_textService = textService;
		}

		public IMethodResult<SquareElementsResult> Execute(SquareStrategyArg arg)
		{
			return new BreakStrategyResult<SquareElementsResult>(new SquareElementsResult
			{
				Center = arg.Center(),
				Message = _textService.ResolveTest(Constants.Errors.UnknowBehavior),
				Spots = Enumerable.Empty<SpotViewModel>(),
				Status = SquareElementsResult.ResultStatus.Last
			});
		}

		public int Order
		{
			get { return Int32.MaxValue; }
		}
	}
}
