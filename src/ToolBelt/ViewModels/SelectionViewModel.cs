using ReactiveUI;

namespace ToolBelt.ViewModels
{
    /// <summary>
    /// A ViewModel which wraps an object of a given type, providing an additional
    /// <see cref="IsSelected" /> state, for use in Selectable User Controls.
    /// </summary>
    public class SelectionViewModel : ReactiveObject
    {
        private bool _isSelected;

        /// <summary>
        /// Default constructor creates a new instance of the <see cref="SelectionViewModel" /> class
        /// in an unselected state, with a default underlying instance.
        /// </summary>
        public SelectionViewModel()
            : this(null, false)
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="SelectionViewModel" /> class in an unselected
        /// state, with the given item value.
        /// </summary>
        /// <param name="item">The value to assign the item.</param>
        public SelectionViewModel(object item)
            : this(item, false)
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="SelectionViewModel" /> class, with the given
        /// item value and selection state.
        /// </summary>
        /// <param name="item">The value to assign the item.</param>
        /// <param name="isSelected">The default selected state to use.</param>
        public SelectionViewModel(object item, bool isSelected)
        {
            this.Item = item;
            this.IsSelected = isSelected;
        }

        /// <summary>
        /// Get or Set the selection state of the underlying item.
        /// </summary>
        public bool IsSelected
        {
            get => _isSelected;
            set => this.RaiseAndSetIfChanged(ref _isSelected, value);
        }

        /// <summary>
        /// Gets the value of the underlying item.
        /// </summary>
        public object Item { get; }
    }

    /// <summary>
    /// A ViewModel which wraps an object of a given type, providing an additional
    /// <see cref="IsSelected" /> state, for use in Selectable User Controls.
    /// </summary>
    /// <typeparam name="TItem">The type of the underlying item.</typeparam>
    public class SelectionViewModel<TItem> : SelectionViewModel
    {
        /// <summary>
        /// Default constructor creates a new instance of the <see cref="SelectionViewModel{T}" />
        /// class in an unselected state, with a default underlying instance.
        /// </summary>
        public SelectionViewModel()
            : base()
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="SelectionViewModel{T}" /> class in an unselected
        /// state, with the given item value.
        /// </summary>
        /// <param name="item">The value to assign the item.</param>
        public SelectionViewModel(TItem item)
            : base(item)
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="SelectionViewModel{T}" /> class, with the given
        /// item value and selection state.
        /// </summary>
        /// <param name="item">The value to assign the item.</param>
        /// <param name="isSelected">The default selected state to use.</param>
        public SelectionViewModel(TItem item, bool isSelected) : base(item, isSelected)
        {
        }

        /// <summary>
        /// Gets the value of the underlying item.
        /// </summary>
        public new TItem Item => (TItem)base.Item;
    }
}
