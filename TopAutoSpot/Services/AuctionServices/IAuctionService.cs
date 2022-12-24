namespace TopAutoSpot.Services.AuctionServices
{
    public interface IAuctionService
    {
        Task StartingAuctionsCheck();
        Task DailyCheckAndRemind();
    }
}
