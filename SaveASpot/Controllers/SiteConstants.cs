using SaveASpot.Controllers.Artifacts;

namespace SaveASpot.Controllers
{
	public sealed class SiteConstants
	{
		public const string QFilter = "qFilter";
		public const string MapControllerAlias = "mapTab";
<<<<<<< HEAD
		public const string CustomersControllerAlias = "customersTab";
		public const string PhasesAndParcelsControllerAlias = "parcelsAndSpotsTab";
		public static string MainMenuTabSpecificReadyAlias { get { return typeof(MainMenuTabAttribute).Name; } }
		public static string ParcelsAndSpotsTabSpecificReadyAlias { get { return typeof(PhasePageTabAttribute).Name; } }
		public const string UploadPhasesAndParcelsControllerAlias = "uploadPhasesAndParcelsTab";
		public const string PhasesControllerAlias = "phasesTab";
		public const string ParcelsControllerAlias = "parcelsTab";
		public const string SpotsControllerAlias = "spotsTab";
=======
		public const string CustomersControllerAlias = "customerTab";
		public const string PhasesAndParcelsControllerAlias = "phasesTab";
		public const string AdminTabControlHeader = "AdminTabControlHeader";
		public const string TabSpecificReadyAlias = "tabSpecificReadyAlias";
>>>>>>> 69c9ebf3750dc4ebe298d09f86679f1667ba5937

		public sealed class Layouts
		{
			public const string DefaultMvcLayout = "~/Views/Shared/_Layout.cshtml";
<<<<<<< HEAD
			public const string MainMenuTabs = @"~\Views\Shared\_LayoutMainMenuTabs.cshtml";
			public const string BootstrapLayout = @"~\Views\Shared\_BootstrapLayout.cshtml";
			public const string SetupAreaLayout = @"~\Areas\Setup\Views\Shared\_SetupLayout.cshtml";
			public const string ParcelsAndSpotsLayout = @"~\Views\Shared\ParcelsAndSpots\_LayoutParcelsAndSpots.cshtml";
			public const string ParcelsAndSpotsAjaxLayout = @"~\Views\Shared\ParcelsAndSpots\_LayoutAjaxParcelsAndSpots.cshtml";
=======
			public const string AdminTabMasterName = @"~\Views\Shared\_LayoutTabDetailAdmin.cshtml";
			public const string BootstrapLayout = @"~\Views\Shared\_BootstrapLayout.cshtml";
			public const string SetupAreaLayout = @"~\Areas\Setup\Views\Shared\_SetupLayout.cshtml";
>>>>>>> 69c9ebf3750dc4ebe298d09f86679f1667ba5937
		}

		public sealed class Controls
		{
			public const string ModalDialogContentView = @"~\Views\Shared\Controls\ModalDialog.cshtml";
			public const string LogOnContainerView = @"~\Views\Shared\Controls\LogOnContainer.cshtml";
			public const string UserInfoContainerView = @"~\Views\Shared\Controls\UserInfoContainer.cshtml";
<<<<<<< HEAD
			public const string ParcelsAndSpotsMenuTabsView = @"~\Views\Shared\ParcelsAndSpots\ParcelsAndSpotsMenuTabs.cshtml";
=======
>>>>>>> 69c9ebf3750dc4ebe298d09f86679f1667ba5937
		}
	}
}