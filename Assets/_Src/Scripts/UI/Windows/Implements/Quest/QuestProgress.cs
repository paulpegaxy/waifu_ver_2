using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Game.Model;

namespace Game.UI
{
    public class QuestProgress : ItemProgress
    {
        // [SerializeField] private Image imageFillHighlight;
        [SerializeField] private TMP_Text textProgress;
        [SerializeField] private List<Color> colors;

        private void OnEnable()
        {
            UpdateColor(Process >= 1);
        }

        private void UpdateColor(bool isFinished)
        {
            var index = isFinished ? 2 : 0;
            imageFill.color = colors[index];
            // imageFillHighlight.color = colors[index + 1];
        }

        public void SetData(ModelApiQuestData data)
        {
            var process = (float)data.processed / data.process;
            var currProcess = data.processed > data.process ? data.process : data.processed;
            textProgress.text = $"{currProcess}/{data.process}";

            SetProgress(process);
            UpdateColor(process >= 1);
        }
    }
}