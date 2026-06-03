using Core.MVVM.View;
using Game.Presentation.ViewModel;
using TMPro;
using UnityEngine;
using Zenject;

namespace Game.Presentation.View
{
    public class HUDView : AbstractPayloadView<HUDViewModel>
    {
        [SerializeField] private TMP_Text _healthCounter;

        [Inject]
        protected override void Construct(HUDViewModel viewModel)
        {
            base.Construct(viewModel);
            _viewModel.OnUpdate += InvokeUpdate;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _viewModel.OnUpdate -= InvokeUpdate;
        }

        private void InvokeUpdate(string health)
            => _healthCounter.text = health;
    }
}