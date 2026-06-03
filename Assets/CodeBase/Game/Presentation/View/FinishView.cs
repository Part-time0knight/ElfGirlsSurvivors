using Core.MVVM.View;
using Game.Presentation.ViewModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game.Presentation.View
{
    public class FinishView : AbstractPayloadView<FinishViewModel>
    {
        [SerializeField] private TMP_Text _winTitle;
        [SerializeField] private TMP_Text _loseTitle;
        [SerializeField] private TMP_Text _statisticsTitle;
        [SerializeField] private Button _restartButton;

        [Inject]
        protected override void Construct(FinishViewModel viewModel)
        {
            base.Construct(viewModel);
            _restartButton.onClick.AddListener(InvokeRestart);
            _viewModel.OnShow += InvokeShow;
        }

        protected override void OnDestroy()
        {
            _viewModel.OnShow -= InvokeShow;
            _restartButton?.onClick.RemoveListener(InvokeRestart);
        }

        private void InvokeShow(FinishViewModel.Dto dto)
        {
            _winTitle.gameObject.SetActive(dto.IsWin);
            _loseTitle.gameObject.SetActive(!dto.IsWin);
            _statisticsTitle.text = dto.Statistic;
        }

        private void InvokeRestart()
        {
            _viewModel.Restart();
        }
    }
}