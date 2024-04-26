using System.Threading.Tasks;
using Services.WindowsService;
using UnityEngine;

namespace Services.Factory.UIFactory
{
    public interface IUIFactory
    {
        public Task<GameObject> CreateScreen(string assetAddress, WindowID windowId);
        public Task<T> GetScreenComponent<T>(WindowID windowId) where T : Component;
        public void DestroyScreen(WindowID windowId);
    }
}