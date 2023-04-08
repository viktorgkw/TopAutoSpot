namespace TopAutoSpot.Tests
{
    using Microsoft.AspNetCore.Mvc.Testing;

    /// <summary>
    /// This class contains Unit Tests to verify that the views of the project are functioning correctly.
    /// </summary>
    public class ViewsTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public ViewsTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        /// <summary>
        /// This test method verifies that each page that does not require users to be authorized will return status Ok.
        /// </summary>
        /// <param name="url">URL of the page.</param>
        /// <returns>Nothing.</returns>
        [Theory]
        [InlineData("/Index")]
        [InlineData("/NotFound")]
        [InlineData("/Error")]
        [InlineData("/UnknownError")]
        [InlineData("/PrivacyPolicy/Index")]
        [InlineData("/Contacts/Index")]
        public async Task UnauthorizedPagesShouldReturnSuccess(string url)
        {
            // Arrange
            var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false,
            });

            // Act
            var response = await client.GetAsync(url);
            
            // Assert
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal("text/html; charset=utf-8",
                response.Content.Headers.ContentType!.ToString());
        }

        /// <summary>
        /// This test method verifies that each administrator view will redirect the user so they can't access it.
        /// </summary>
        /// <param name="url">URL of the page.</param>
        /// <returns>Nothing.</returns>
        [Theory]
        [InlineData("/AdministratorViews/ApprovalViews/ApproveAuction")]
        [InlineData("/AdministratorViews/ApprovalViews/ApproveVehicle")]
        [InlineData("/AdministratorViews/ApprovalViews/RefuseAuction")]
        [InlineData("/AdministratorViews/ApprovalViews/RefuseAuctionReason")]
        [InlineData("/AdministratorViews/ApprovalViews/RefuseVehicle")]
        [InlineData("/AdministratorViews/ApprovalViews/RefuseVehicleReason")]
        [InlineData("/AdministratorViews/AuctionsCD/Close")]
        [InlineData("/AdministratorViews/AuctionsCD/Delete")]
        [InlineData("/AdministratorViews/ListingsCD/Close")]
        [InlineData("/AdministratorViews/ListingsCD/Delete")]
        [InlineData("/AdministratorViews/UsersCRUD/Close")]
        [InlineData("/AdministratorViews/UsersCRUD/Edit")]
        [InlineData("/AdministratorViews/UsersCRUD/Preview")]
        [InlineData("/AdministratorViews/AuctionsApproval")]
        [InlineData("/AdministratorViews/ListingsApproval")]
        [InlineData("/AdministratorViews/ManageAuctions")]
        [InlineData("/AdministratorViews/ManageListings")]
        [InlineData("/AdministratorViews/ManageUsers")]
        public async Task AdministratorPagesShouldRedirect(string url)
        {
            // Arrange
            var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            // Act
            var response = await client.GetAsync(url);
            int statusCode = (int)response.StatusCode;
            bool statusCodeIsRedirect = statusCode >= 300 && statusCode <= 399;

            // Assert
            Assert.True(statusCodeIsRedirect);
        }

        /// <summary>
        /// This test method verifies that each page that does require users to be authorized will redirect users to Register/Login page.
        /// </summary>
        /// <param name="url">URL of the page.</param>
        /// <returns>Nothing.</returns>
        [Theory]
        // Auction Views
        [InlineData("/AuctionViews/AuctionBid")]
        [InlineData("/AuctionViews/Create")]
        [InlineData("/AuctionViews/Index")]
        [InlineData("/AuctionViews/Join")]
        [InlineData("/AuctionViews/JoinedAuctions")]
        [InlineData("/AuctionViews/Leave")]
        [InlineData("/AuctionViews/NoVehiclesToAuction")]

        // Buy Views
        [InlineData("/Buy/BoatListing")]
        [InlineData("/Buy/BusListing")]
        [InlineData("/Buy/CarListing")]
        [InlineData("/Buy/Index")]
        [InlineData("/Buy/MotorcycleListing")]
        [InlineData("/Buy/TrailerListing")]
        [InlineData("/Buy/TruckListing")]

        // Trade View
        [InlineData("/Trade/Index")]

        // InterestedIn Views
        [InlineData("/InterestedIn/Index")]
        [InlineData("/InterestedIn/InterestedInVehicle")]
        [InlineData("/InterestedIn/RemoveInterest")]

        // Notifications Views
        [InlineData("/Notifications/Index")]
        [InlineData("/Notifications/NotificationRemoved")]
        [InlineData("/Notifications/Preview")]

        // Premium Account Views
        [InlineData("/PremiumAccount/Index")]
        [InlineData("/PremiumAccount/PaymentResult")]
        [InlineData("/PremiumAccount/PurchaseDetails")]

        // Vehicle Preview Views
        [InlineData("/VehiclePreview/AuctionPreivew")]
        [InlineData("/VehiclePreview/BoatPreview")]
        [InlineData("/VehiclePreview/BusPreview")]
        [InlineData("/VehiclePreview/CarPreview")]
        [InlineData("/VehiclePreview/MotorcyclePreview")]
        [InlineData("/VehiclePreview/TrailerPreview")]
        [InlineData("/VehiclePreview/TruckPreview")]

        // My Vehicles Views
        [InlineData("/MyVehicles/Index")]
        [InlineData("/MyVehicles/AuctionsCRUD/Delete")]
        [InlineData("/MyVehicles/AuctionsCRUD/Edit")]
        [InlineData("/MyVehicles/BoatCRUD/Create")]
        [InlineData("/MyVehicles/BoatCRUD/Delete")]
        [InlineData("/MyVehicles/BoatCRUD/Edit")]
        [InlineData("/MyVehicles/BusCRUD/Create")]
        [InlineData("/MyVehicles/BusCRUD/Delete")]
        [InlineData("/MyVehicles/BusCRUD/Edit")]
        [InlineData("/MyVehicles/CarCRUD/Create")]
        [InlineData("/MyVehicles/CarCRUD/Delete")]
        [InlineData("/MyVehicles/CarCRUD/Edit")]
        [InlineData("/MyVehicles/MotorcycleCRUD/Create")]
        [InlineData("/MyVehicles/MotorcycleCRUD/Delete")]
        [InlineData("/MyVehicles/MotorcycleCRUD/Edit")]
        [InlineData("/MyVehicles/TrailerCRUD/Create")]
        [InlineData("/MyVehicles/TrailerCRUD/Delete")]
        [InlineData("/MyVehicles/TrailerCRUD/Edit")]
        [InlineData("/MyVehicles/TruckCRUD/Create")]
        [InlineData("/MyVehicles/TruckCRUD/Delete")]
        [InlineData("/MyVehicles/TruckCRUD/Edit")]
        public async Task AuthorizedPagesShouldRedirect(string url)
        {
            // Arrange
            var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            // Act
            var response = await client.GetAsync(url);
            int statusCode = (int)response.StatusCode;
            bool statusCodeIsRedirect = statusCode >= 300 && statusCode <= 399;

            // Assert
            Assert.True(statusCodeIsRedirect);
        }
    }
}
