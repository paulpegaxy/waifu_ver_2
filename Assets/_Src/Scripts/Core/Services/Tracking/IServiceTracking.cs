// Author: ad   -
// Created: 04/10/2024  : : 00:10
// DateUpdate: 04/10/2024

namespace Game.Runtime
{
    public interface IServiceTracking : IService
    {
        void SendTrackingSessionLength();
        void UpdateExitTime();
    }
}