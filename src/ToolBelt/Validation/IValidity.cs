namespace ToolBelt.Validation
{
    public interface IValidity : System.ComponentModel.IChangeTracking
    {
        bool IsValid { get; }

        void ClearValidationErrors();

        bool Validate();
    }
}
