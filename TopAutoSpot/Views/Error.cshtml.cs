namespace TopAutoSpot.Views
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using System.Diagnostics;

    /// Represents a page model for handling errors.
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [IgnoreAntiforgeryToken]
    public class ErrorModel : PageModel
    {
        /// Gets or sets the request ID.
        public string? RequestId { get; set; }

        /// Gets a value indicating whether the request ID should be shown.
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        /// Called when the model is initialized.
        public void OnGet()
        {
            // Sets the RequestId property to the current Activity's Id or the HttpContext's TraceIdentifier.
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
        }
    }
}