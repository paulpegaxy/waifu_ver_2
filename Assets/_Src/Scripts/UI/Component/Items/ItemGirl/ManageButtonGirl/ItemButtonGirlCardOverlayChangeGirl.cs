// Author: ad   -
// Created: 17/10/2024  : : 04:10
// DateUpdate: 17/10/2024

using Doozy.Runtime.Signals;
using Game.Extensions;
using Template.Defines;

namespace Game.UI
{
    public class ItemButtonGirlCardOverlayChangeGirl : AItemButtonGirlCardOverlayPremium
    {
        protected override void OnSetData()
        {
            
        }

        protected override void OnClick()
        {
            // this.PostEvent(TypeGameEvent.OpenPremiumGallery, true);
            Signal.Send(StreamId.UI.OpenGallery);
        }
    }
}