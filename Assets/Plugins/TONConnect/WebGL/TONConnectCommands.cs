using Supyrb;
using Supyrb.Attributes;

public class TONConnectCommands : WebCommands
{
    [WebCommand(Description = "Wallet Status Change")]
    public void StatusChange()
    {
        TONConnect.UpdateWallet();
    }

}