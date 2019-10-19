using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Media;
using Ascon.Pilot.SDK;
using Ascon.Pilot.Theme.ColorScheme;
using RubiusTest.Helpers;
using RubiusTest.ViewModels;
using RubiusTest.Views;

namespace RubiusTest
{
    [Export(typeof(INewTabPage))]

    class NotePlugin : INewTabPage
    {
        private readonly ITabServiceProvider _tabServiceProvider;
        private readonly IObjectsRepository _repository;
        private readonly IPilotDialogService _pilotDialogService;
        private readonly IObjectModifier _modifier;

        private const string CommandName = "OpenNotePlugin";
        private const string Title = "Заметки";

        [ImportingConstructor]
        public NotePlugin(
            IObjectsRepository repository, 
            ITabServiceProvider tabServiceProvider,
            IPilotDialogService dialogService)
        {
            _repository = repository;
            _pilotDialogService = dialogService;
            _tabServiceProvider = tabServiceProvider;
            var accentColor = (Color)ColorConverter.ConvertFromString(dialogService.AccentColor);
            ColorScheme.Initialize(accentColor, dialogService.Theme);
        }
        public void BuildNewTabPage(INewTabPageHost host)
        {
            var toolTip = "Открыть плагин заметок";

            var icon = IconLoader.GetIcon("plugin_icon.svg");
            host.AddButton(Title, CommandName, toolTip, icon);
        }

        public void OnButtonClick(string name)
        {
            var activeTabPageTitle = _tabServiceProvider.GetActiveTabPageTitle();

            if (name == CommandName)
            {
                var view = new MainNoteView();
                var viewModel = new MainNoteViewModel(_repository, _pilotDialogService, _tabServiceProvider, _modifier);
                view.DataContext = viewModel;
                _tabServiceProvider.UpdateTabPageContent(activeTabPageTitle, Title, view);
                return;
            }
        }
    }
}
