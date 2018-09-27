using ReactiveUI;
using System.Reactive;

namespace ToolBelt.Models
{
    // TODO: This should be immutable.  Also should take the tap command out and handle it at a higher level.  Leaving it like this for now for the prototype app.
    public class CustomMenuItem : ReactiveObject
    {
        private string _iconSource;
        private ReactiveCommand<Unit, Unit> _tapCommand;
        private string _title;

        public string IconSource
        {
            get => _iconSource;
            set => this.RaiseAndSetIfChanged(ref _iconSource, value);
        }

        public ReactiveCommand<Unit, Unit> TapCommand
        {
            get => _tapCommand;
            set => this.RaiseAndSetIfChanged(ref _tapCommand, value);
        }

        public string Title
        {
            get => _title;
            set => this.RaiseAndSetIfChanged(ref _title, value);
        }
    }
}
