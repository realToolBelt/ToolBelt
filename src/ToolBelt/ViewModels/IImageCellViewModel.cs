namespace ToolBelt.ViewModels
{
    /// <summary>
    /// An interface defining the basic requirements for a view-model that will be used in the image cell.
    /// </summary>
    public interface IImageCellViewModel
    {
        /// <summary>
        /// Gets the text that is shown underneath the first line, in a smaller font.
        /// </summary>
        string Detail { get; }

        /// <summary>
        /// Gets the image to display next to the text.
        /// </summary>
        byte[] ImageSource { get; }

        /// <summary>
        /// Gets the text that is shown on the first line, in large font..
        /// </summary>
        string Text { get; }
    }
}
